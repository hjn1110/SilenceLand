using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Silent.MapObject.SearchObject;

public abstract class Enemies : MonoBehaviour
{
    //可配置属性 
    protected float hearing { get { return setting._hearing; } }//听力
    protected float vision { get { return setting._vision; }  }//视力
    protected float fleeVision { get { return setting._fleeVision; } }//逃逸视力
    protected float patrolVision { get { return setting._patrolVision; }}
    protected float hp { get { return setting._maxHp; } set { hp = setting._maxHp; } }//血量
    protected float moveSpeed { get { return setting._moveSpeed; } set { moveSpeed = setting._moveSpeed; } }//移动速度
    protected float angleSpeed { get { return setting._angleSpeed; } set { angleSpeed = setting._angleSpeed; } }//转身速度
    protected float hearingReduceSpeed { get { return setting._hearingReduceSpeed; } }//听力记忆衰减速度

    //字段配置表
    protected GlobalSettings globalSetting;
    protected abstract EnemySetting setting { get; }

    //-------------------------------------------------------------------------

    //状态机
    protected FSMSystem fsm;
    protected abstract void initFSM();

    //听视觉状态属性
    public bool hearAnything{get{if (theSeekTarget == Vector3.zero){return false;}else{return true;}}}//听觉判断/搜寻依据
    public bool SeePlayer{ get { if (theFollowTarget == null) { return false; } else { if ((theFollowTarget != null) && (IfSeeDirectly(theFollowTarget))) { return true; } else return false; } } }//视觉判断/追踪依据
    public bool LostPlayer { get { if (theFollowTarget == null) { return true; } else { return false; } } }

    //听视觉状态属性参数字段
    private Vector3 theSeekTarget;//通过听觉获取到的搜寻目标
    private Dictionary<Vector3, float> soundSourceList;//存储听到及记得的声源信息，包括坐标和音量记忆
    private Transform theFollowTarget;//通过视觉获取到的追踪目标
    private List<Transform> followTargetList;//储存看到的对象

    //视觉Raycast参数
    private Global global;
    private GameObject player { get { return global.player; } }
    private ContactFilter2D filter;
    private LayerMask seeObbMask;
    private int hitNum;
    private RaycastHit2D[] hits;

    //寻路代理
    private NavMeshAgent agent;

    //路径属性
    //public PathList path;//挂载和当前AI匹配的路径
    [SerializeField]
    private Vector3 thePatrolTarget;//巡逻指向的下一个路线目标
    //[HideInInspector]
    public LinkedList<PathNods> NodsInView;//用于存储当前视野中的nods
    //[HideInInspector]
    public PathNods theLastNodInView;//用于存储最近一次看到的且已不在NodsInView中的nod

    //指定默认状态
    public StateID defaultState = StateID.Pend;
    public StateID currentState;


    private PathsManager manager;


    //-------------------------------------------------------------------------

    //初始化
    private void Awake()
    {
        soundSourceList = new Dictionary<Vector3, float>();
        followTargetList = new List<Transform>();
        NodsInView = new LinkedList<PathNods>();
        //用于订阅搜索事件
        gameObject.AddComponent<SubscribeSearch>();
    }
    private void Start()
    {
        AddRigid();
        global = Global.instance;
        globalSetting = GlobalSettings.instance;
        manager = PathsManager.instance;
        InitRayCast();
        AddTrigger();
        initFSM();
        AddNavMeshAgent();
        initClosestTarget();


    }


 

    //主循环
    private void Update()
    {
        if (Time.frameCount % 10 == 0)//10帧检查一次状态。FPS=30时约1秒检查3次，60时则6次
        {
            //状态机更新
            fsm.Update(gameObject);
            currentState = fsm.currentFSMState.ID;

            //print(gameObject.name+"当前的状态是：" + fsm.currentFSMState);
            //print("seekTarget="+theSeekTarget);
            //音量记忆衰减
            HearingReduce();
            //根据音量记忆排序，确定搜寻对象
            seekTarget();

            selectTargetFormMinDis();
        }
    }

    //-------------------------------------------------------------------------
    //碰撞相关
    protected abstract void AddRigid();

    //-------------------------------------------------------------------------
    //Path Patrol相关

    public void SetNextTarget(PathNods nod)
    {
        Debug.Log("抵达目标，切换下个目标");
        if (nod.nextNods != null)
        {
            thePatrolTarget = nod.nextNods.transform.position;

        }
    }

    //工具方法，返回一个List中距离自己最近的nod
    //待优化：用坐标平方之和代替Vector2.Distance进行比较，减少平方根计算
    //待优化：成员用gameobject代替类，这样可以把距离比较的方法封装成一个通用的
    private PathNods theClosestNodOfList(List<PathNods> nods)
    {
        float dis = Mathf.Infinity;
        PathNods target = null;
        if ((nods != null) && (nods.Count != 0))
        {
            Debug.Log("nods.Count="+ nods.Count);
            for (int i = 0; i < nods.Count; i++)
            {
                float _dis = Vector2.Distance(nods[i].transform.position, gameObject.transform.position);
                if (_dis < dis)
                {
                    dis = _dis;
                    target = nods[i];
                }
            }
        }
        else
        {
            Debug.Log("nods为空");
        }
        

        return target;

    }
    //工具方法，返回一个List中距离自己最近的nod
    //待优化：用坐标平方之和代替Vector2.Distance进行比较，减少平方根计算
    //待优化：成员用gameobject代替类，这样可以把距离比较的方法封装成一个通用的
    private PathNods theClosestNodOfLinkedList(LinkedList<PathNods> nods)
    {
        float dis = Mathf.Infinity;
        PathNods target = null;
        foreach (PathNods nod in nods)
        {
            float _dis = Vector2.Distance(nod.transform.position, gameObject.transform.position);
            if (_dis < dis)
            {
                dis = _dis;
                target = nod;
            }
        }

        return target;

    }
    
    public void initClosestTarget()
    {
        if (manager.AllNods == null)
        {
            Debug.LogError("AllNods为空");    
        }
        Debug.Log("初始化Patrol最近目标");
        thePatrolTarget = theClosestNodOfList(manager.AllNods).transform.position;
    }
    

    //LostPlayer、LostTarget后，返回巡逻状态，先调用该方法搜索距离自己最近的nod作为目标
    public void SearchClosestTarget()
    {
       
        if ((NodsInView != null)&&(NodsInView.Count!=0))
        {
            //Debug.Log("执行算法1:视野中存在目标，前往:"+ theClosestNodOfLinkedList(NodsInView).gameObject.name);
            thePatrolTarget = theClosestNodOfLinkedList(NodsInView).transform.position;
        }
        else
        if (theLastNodInView != null)
        {
            //Debug.Log("执行算法2:上一次视野中记录过目标，前往:" + theLastNodInView.gameObject.name);
            thePatrolTarget = theLastNodInView.transform.position;
        }
        else
        if (thePatrolTarget != Vector3.zero)
        {
            //Debug.Log("执行算法3:上一次巡逻过的目标非空，前往:" + thePatrolTarget);
        }
        else
        if((manager.AllNods!=null)&&(manager.AllNods.Count!=0))
        {
            thePatrolTarget = theClosestNodOfList(manager.AllNods).transform.position;
            //Debug.Log("执行算法4:搜索全体nods中最近点，前往:" + theClosestNodOfList(PathNods.AllNods).gameObject.name);

        }

    }



    //-------------------------------------------------------------------------
    //寻路相关
    protected void AddNavMeshAgent()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.agentTypeID = 0;
        agent.baseOffset = 0.1f;
        agent.speed = moveSpeed;
        agent.acceleration = 2f;
        agent.stoppingDistance = 0.25f;
        agent.autoBraking = true;
        agent.radius = 0.1f;
        agent.height = 0.2f;
        agent.avoidancePriority = 1;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        //下面两行是为了使用navMesh2D(即navMeshPlus)所必须做的设置，因为该脚本对navMesh的导航坐标系(xz)做了翻转(xy)，不再能使用其自身的旋转方法
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //下面两行是使用重写的转向方法，弥补了navMesh自身的转向方法的失效
        Rotate_Smoothly rotateSmooth = gameObject.AddComponent<Rotate_Smoothly>();
        rotateSmooth.rotateSpeed = angleSpeed;
    }

    protected void continuelyGoto(Vector3 position)
    {
        agent.SetDestination(position);
    }
    public void Follow()
    {
        beginGoto();
        if (theFollowTarget != null)
        {
            continuelyGoto(theFollowTarget.position);
        }
    }
    public void Seek()
    {
        beginGoto();
        continuelyGoto(theSeekTarget);
    }
    public void Patrol()
    {
        beginGoto();
        continuelyGoto(thePatrolTarget);
    }
    public void BackToPatrol()
    {
        beginGoto();
        SearchClosestTarget();
        continuelyGoto(thePatrolTarget);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }
    protected void beginGoto()
    {
        agent.isStopped = false;
    }

    //-------------------------------------------------------------------------
    //听觉相关

    //音量记忆衰减函数
    protected void HearingReduce()
    {
        List<Vector3> soundPos = new List<Vector3>(soundSourceList.Keys);
        if (ifHear())
        {
            for (int i = 0; i < soundPos.Count; i++)
            {
                if (soundSourceList[soundPos[i]] > 0)
                {
                    soundSourceList[soundPos[i]] -= hearingReduceSpeed;
                    //Debug.Log("音量记忆衰减:" + soundSourceList[soundPos[i]]);
                }
                else
                {
                    soundSourceList.Remove(soundPos[i]);
                    //Debug.Log("移除一个声源");
                }
            }
        }
    }

    //判断声音记忆List是否为空，作为排序依据
    public bool ifHear()
    {
        if ((soundSourceList != null) && (soundSourceList.Count != 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    //听觉判定的主方法，每帧执行一次，从list中搜寻听觉对象
    protected Vector3 seekTarget()
    {
        if (ifHear())
        {
            theSeekTarget = selectTargetFormMaxVolume();
            return theSeekTarget;
        }
        else
        //没有听到任何东西的时候
        {
            theSeekTarget = Vector3.zero;
            return theSeekTarget;
        }

    }

    //对音量记忆list排序，选择音量最大的作为搜寻对象
    Vector3 selectTargetFormMaxVolume()
    {
        float volume = 0;
        Vector3 target = Vector3.zero;
        foreach (Vector3 pos in soundSourceList.Keys)
        {
            if (volume < soundSourceList[pos])
            {
                volume = soundSourceList[pos];
                target = pos;
            }
        }
        return target;
    }

    //用于被SondSpread类的声音算法调用，在每次声音传递中，将符合条件的对象添加进监听者的音量记忆list
    public void Hear(Vector3 soundSourcePos, float soundVolume)
    {
        if (soundVolume >= hearing)
        {
            //如果该声音目标未添加，则添加该目标
            if (!soundSourceList.ContainsKey(soundSourcePos))
            {
                soundSourceList.Add(soundSourcePos, soundVolume);
            }
            //如果该声音目标已添加，则更新该目标的声音信号
            else
            {
                soundSourceList[soundSourcePos] = soundVolume;
            }
        }
    }

    //-------------------------------------------------------------------------
    //视觉相关

    //初始化RayCast
    protected void InitRayCast()
    {
        seeObbMask = LayerMask.GetMask("OBB");
        filter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false,
            layerMask = seeObbMask,
        };
        hits = new RaycastHit2D[36];
    }

    //初始化视觉Field所需Trigger
    protected void AddTrigger()
    {
        //待优化：现有的视野是圆形，但应该设计成锥形，否则不能做潜行背刺

        //加载初始视野Trigger
        
        //if (TriggerCreater.instance == null) { Debug.Log("trigger==null"); }
        GameObject ViewField = TriggerCreater.instance.AddTriggerObject(vision, transform, "ViewField");
        ViewField.AddComponent<ViewField>();


        //加载逃逸视野Trigger

        GameObject FleeField = TriggerCreater.instance.AddTriggerObject(fleeVision, transform, "FleeField");
        FleeField.AddComponent<FleeField>();

        //加载Patrol搜寻path视野Trigger
        GameObject PatrolViewField = TriggerCreater.instance.AddTriggerObject(patrolVision, transform, "PatrolField");
        PatrolViewField.AddComponent<PatrolField>();
    }

    //用于被ViewTrigger调用，将进入Enemy视野的对象添加到视觉list
    public void AddViewTarget(Transform targetTrans)
    {
        if (!followTargetList.Contains(targetTrans))
        {
            followTargetList.Add(targetTrans);
            
        }
    }

    //用于被FleeTrigger调用，将离开Enemy视野的对象从视觉list中移除
    public void RemoveViewTarget(Transform targetTrans)
    {
        followTargetList.Remove(targetTrans);
        selectTargetFormMinDis();

    }

    //对视觉list进行排序，选择距离最近的作为追踪对象
    protected Transform selectTargetFormMinDis()
    {
        float dis = Mathf.Infinity;
        float _d;
        Transform target = null;
        if (followTargetList.Count != 0)
        {
            Debug.Log("followTarget0=" + followTargetList[0]);

        }
        for (int i=0;i<followTargetList.Count;i++)
        {
            //if (IfSeeDirectly(followTargetList[i]))
            {
                //Debug.Log("当前检测目标能看见");
                _d = Vector2.Distance(gameObject.transform.position, followTargetList[i].position);
                if (dis > _d)
                {
                    dis = _d;
                    target = followTargetList[i];
                }
            }

        }
        theFollowTarget = target;
        Debug.Log("followTarget="+theFollowTarget);
        return target;
    }

    //由状态属性调用，判断监视者和监视对象之间是否没有障碍物
    private bool IfSeeDirectly(Transform targetTrans)
    {
        float dis = Vector2.Distance(targetTrans.position, gameObject.transform.position);
        hitNum = Physics2D.Raycast(gameObject.transform.position, targetTrans.position - gameObject.transform.position, filter, hits, dis);
        if (hitNum > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    

    //-------------------------------------------------------------------------

}
