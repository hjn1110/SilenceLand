using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface ISeeComponent
{
    //由Entity调用
    bool Lost { get; }//丢失判断
    Transform SeeTarget();
}

public interface ISeeComponentEditor
{
    //由Trigger调用
    void AddViewTarget(Transform targetTrans);
    void RemoveViewTarget(Transform targetTrans);
}


[CreateAssetMenu]
public class SeeSetting : ScriptableObject
{

}

public class SeeComponent : ISeeComponent, ISeeComponentEditor
{
    public SeeComponent(SeeSetting seeSetting,Transform parent)
    {
        this.parent = parent;
        followTargetList = new List<Transform>();
    }

    Transform parent;

    public bool SeePlayer { get { if (SeeTarget() == null) { return false; } else { if ((SeeTarget() != null) && (IfSeeDirectly(SeeTarget(), parent))) { return true; } else return false; } } }//视觉判断/追踪依据
    public bool Lost { get { if (SeeTarget() == null) { return true; } else { return false; } } }


    //视觉状态属性参数字段

    private List<Transform> followTargetList;//储存看到的对象

    public int vision { get; set; }//视力
    protected int fleeVision { get; set; }//逃逸视力
    protected int patrolVision { get; set; }

     


    //视觉Raycast参数
    //private GameObject player { get { return Global.instance.player; } }
    private ContactFilter2D filter;
    private LayerMask seeObbMask;
    private int hitNum;
    private RaycastHit2D[] hits;


 


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
    protected void Ctor(Transform transform,float fleeVision,float vision)
    {
        //待优化：现有的视野是圆形，但应该设计成锥形，否则不能做潜行背刺

        //加载初始视野Trigger
        //if (TriggerCreater.instance == null) { Debug.Log("trigger==null"); }
        GameObject ViewField = TriggerCreater.instance.AddTriggerObject(vision, transform, "ViewField");
        ViewField.AddComponent<ViewField>().Ctor(this);
        
        //加载逃逸视野Trigger
        GameObject FleeField = TriggerCreater.instance.AddTriggerObject(fleeVision, transform, "FleeField");
        FleeField.AddComponent<FleeField>().Ctor(this);

    }



    //视觉方法
    //传出视觉对象：FollowTargetList中最近的一个；若FollowTargetList为空，则返回false
    public Transform SeeTarget()
    {
        if ((followTargetList != null) && (followTargetList.Count != 0))
        {
            return TargetFilter(parent);
        }
        else
        {
            return null;
        }

    }
    


    //对视觉list进行排序，选择距离最近的作为追踪对象
    protected Transform TargetFilter(Transform trans)
    {
        float dis = Mathf.Infinity;
        float _d;
        Transform target = null;
        if (followTargetList.Count != 0)
        {
            Debug.Log("followTarget0=" + followTargetList[0]);

        }
        for (int i = 0; i < followTargetList.Count; i++)
        {
            //if (IfSeeDirectly(followTargetList[i]))
            {
                //Debug.Log("当前检测目标能看见");
                _d = Vector2.Distance(trans.position, followTargetList[i].position);
                if (dis > _d)
                {
                    dis = _d;
                    target = followTargetList[i];
                }
            }

        }
        return target;
    }



    //由状态属性调用，判断监视者和监视对象之间是否没有障碍物
    private bool IfSeeDirectly(Transform trans,Transform targetTrans)
    {
        float dis = Vector2.Distance(targetTrans.position, trans.position);
        hitNum = Physics2D.Raycast(trans.position, targetTrans.position - trans.position, filter, hits, dis);
        if (hitNum > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    //用于被FleeTrigger调用，将离开Enemy视野的对象从视觉list中移除
    public void RemoveViewTarget(Transform targetTrans)
    {
        followTargetList.Remove(targetTrans);
        //TargetFilter(trans);

    }

    //用于被ViewTrigger调用，将进入Enemy视野的对象添加到视觉list
    public void AddViewTarget(Transform targetTrans)
    {
        if (!followTargetList.Contains(targetTrans))
        {
            followTargetList.Add(targetTrans);

        }
    }
}
