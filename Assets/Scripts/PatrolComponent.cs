using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPatrolComponent
{
    //由Entity调用
    void Patrol();
    void Return();
}

public interface IPatrolComponentEditor
{
    //由Trigger调用
    void RefreshNodsCache(PathNods nods);
    void ClearNodsCache(PathNods nods);
    void SetLastNod(PathNods nod);
}


[CreateAssetMenu]
public class PatrolSetting : ScriptableObject
{
    public float patrolVision;
}

public class PatrolComponent : IPatrolComponent, IPatrolComponentEditor
{
    IMoveComponent moveComponent;
    Transform parent;
    [HideInInspector]
    public Vector3 thePatrolTarget;//巡逻指向的下一个路线目标
    [HideInInspector]
    public LinkedList<PathNods> NodsInView { get; set; }//用于存储当前视野中的nods
    [HideInInspector]
    public PathNods theLastNodInView;//用于存储最近一次看到的且已不在NodsInView中的nod

    public PatrolComponent(IMoveComponent moveComponent, Transform parent)
    {
        this.moveComponent = moveComponent;
        this.parent = parent;
    }

    public void Patrol()
    {
        moveComponent.MoveTo(thePatrolTarget);
    }
    public void Return()
    {
        SearchClosestTarget();
        moveComponent.MoveTo(thePatrolTarget);

    }

    public void RefreshNodsCache(PathNods nod)
    {
        NodsInView.AddLast(nod);
    }
    public void ClearNodsCache(PathNods nod)
    {
        NodsInView.Remove(nod);
    }
    public void SetLastNod(PathNods nod)
    {
        theLastNodInView = nod;
    }





    void Ctor(Transform parent, PatrolSetting patrolSetting)
    {
        //加载Patrol搜寻path视野Trigger
        GameObject PatrolViewField = TriggerCreater.instance.AddTriggerObject(patrolSetting.patrolVision, parent, "PatrolField");
        PatrolViewField.AddComponent<PatrolField>().Ctor(this);
    }



    public void initClosestTarget()
    {
        if (PathsManager.instance.AllNods == null)
        {
            Debug.LogError("AllNods为空");
        }
        Debug.Log("初始化Patrol最近目标");
        thePatrolTarget = theClosestNodOfList(PathsManager.instance.AllNods, parent.position).transform.position;
    }



    public void SearchClosestTarget()
    {
        if (thePatrolTarget != Vector3.zero)
        {
            Debug.Log("执行算法3:上一次巡逻过的目标非空，前往:" + thePatrolTarget);
        }
        else

        if (theLastNodInView != null)
        {
            Debug.Log("执行算法2:上一次视野中记录过目标，前往:" + theLastNodInView.gameObject.name);
            thePatrolTarget = theLastNodInView.transform.position;
        }
        else

         if ((NodsInView != null) && (NodsInView.Count != 0))
        {
            Debug.Log("执行算法1:视野中存在目标，前往:" + theClosestNodOfLinkedList(NodsInView, parent.position).gameObject.name);
            thePatrolTarget = theClosestNodOfLinkedList(NodsInView, parent.position).transform.position;
        }
        else


        if ((PathsManager.instance.AllNods != null) && (PathsManager.instance.AllNods.Count != 0))
        {
            thePatrolTarget = theClosestNodOfList(PathsManager.instance.AllNods, parent.position).transform.position;
            Debug.Log("执行算法4:搜索全体nods中最近点，前往:" + theClosestNodOfList(PathsManager.instance.AllNods, parent.position).gameObject.name);

        }

    }
    //工具方法，返回一个List中距离自己最近的nod
    //待优化：用坐标平方之和代替Vector2.Distance进行比较，减少平方根计算
    //待优化：成员用gameobject代替类，这样可以把距离比较的方法封装成一个通用的
    private PathNods theClosestNodOfLinkedList(LinkedList<PathNods> nods, Vector3 pos)
    {
        float dis = Mathf.Infinity;
        PathNods target = null;
        foreach (PathNods nod in nods)
        {
            float _dis = Vector2.Distance(nod.transform.position, pos);
            if (_dis < dis)
            {
                dis = _dis;
                target = nod;
            }
        }

        return target;

    }
    //工具方法，返回一个List中距离自己最近的nod
    //待优化：用坐标平方之和代替Vector2.Distance进行比较，减少平方根计算
    //待优化：成员用gameobject代替类，这样可以把距离比较的方法封装成一个通用的
    private PathNods theClosestNodOfList(List<PathNods> nods, Vector3 pos)
    {
        float dis = Mathf.Infinity;
        PathNods target = null;
        if ((nods != null) && (nods.Count != 0))
        {
            Debug.Log("nods.Count=" + nods.Count);
            for (int i = 0; i < nods.Count; i++)
            {
                float _dis = Vector2.Distance(nods[i].transform.position, pos);
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


}