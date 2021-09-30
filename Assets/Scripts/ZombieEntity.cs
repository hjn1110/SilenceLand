using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieEntity : MonoBehaviour
{
    
    //数据层
    public MoveSetting moveSetting;
    public NavMeshAgentSetting agentSetting;
    public SeeSetting seeSetting;
    public HearSetting hearSetting;
    public ZombieFSMSetting fsmSetting;

    private PathsManager pathsManager;


    //路径属性
    [HideInInspector]
    public Vector3 thePatrolTarget;//巡逻指向的下一个路线目标
    [HideInInspector]
    public LinkedList<PathNods> NodsInView;//用于存储当前视野中的nods
    [HideInInspector]
    public PathNods theLastNodInView;//用于存储最近一次看到的且已不在NodsInView中的nod


    //行为层
    //Move组件
    //IMoveComponent moveComponent;
    //AIMove组件
    //IAIMoveComponent aIMoveComponent;
    IMoveComponent aIMoveComponent;
    //See组件
    ISeeComponent seeComponent;
    //Hear组件
    IHearComponent hearComponent;
    //Patrol组件
    IPatrolComponent patrolComponent;
    //Follow组件
    IFollowCompenent followCompenent;
    //Seek组件
    ISeekComponent seekComponent;

    //逻辑层
    //状态机
    IZombieFSM zombieFSM;


    /// <summary>
    /// 创建组件
    /// 注入依赖
    /// </summary>
    private void Start()
    {
        aIMoveComponent = new AIMoveComponent(agentSetting);
        //moveComponent = new MoveComponent(moveSetting);


        seeComponent = new SeeComponent(seeSetting,transform);
        hearComponent = new HearComponent(hearSetting);
        zombieFSM = new ZombieFSM(this,fsmSetting);
        //patrolComponent = new PatrolComponent(moveSetting,transform);
        followCompenent = new FollowComponent(aIMoveComponent);
        seekComponent = new SeekComponent(aIMoveComponent);

    }


    /// <summary>
    /// 行为层实现
    /// </summary>
    public void Follow()
    {
        if (seeComponent.SeeTarget() != null)
        {
            followCompenent.Follow(seeComponent.SeeTarget());
        }
    }
    public void Seek()
    {
        if (hearComponent.Hear())
        {
            seekComponent.Seek(hearComponent.HearTarget);
        }
        
    }
    public void Patrol()
    {
        //patrolComponent.Patrol();
    }
    public void ReturnToPatrol()
    {
        //patrolComponent.Return();
    }


    /// <summary>
    /// 逻辑层主循环
    /// </summary>
    public void Update()
    {
        //状态机更新
        zombieFSM.Update(gameObject);

        //音量记忆衰减
        hearComponent.HearWeaken();

        //Debug
        //print(gameObject.name+"当前的状态是：" + fsm.currentFSMState);
        //print("seekTarget="+theSeekTarget);

    }


}
