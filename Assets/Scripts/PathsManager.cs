using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsManager : MonoBehaviour
{
    //字段声明
    [ReadOnly]
    public int pathNum=0;
    [ReadOnly]
    public List<PathNods> AllNods;//用于存储全局所有nods
    [ReadOnly]
    public List<PathList> AllPaths;//用于存储全局所有path

    //单例声明
    #region Singleton
    public static PathsManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    //------------------------------------------------------------------------------
    //以下为创建paths相关的公共方法集，由编辑器类PathsEditor调用

    //条件方法集：由PathsEditor调用并关联相应业务操作方法，并在条件触发时调用
    //满足删除path的条件
    public bool OnDeleteOne()
    {
        if ((AllPaths != null) && (AllPaths.Count != 0))
        {
            return true;
        }
        return false;
    }

    //满足清空所有paths的条件
    public bool OnClearAllPaths()
    {

        if ((AllPaths != null) && (AllPaths.Count != 0))
        {
            return true;

        }
        return false;
    }

    //------------------------------------------------------------------------------

    //工具方法
    //移除当前allNods中为空的对象（执行批量destory但未从list中移除的对象）
    public void RemoveAllNullInAllNodsList(List<PathNods> nods)
    {
        if ((nods != null) && (nods.Count != 0))
        {
            List<PathNods> nodsToRemove = new List<PathNods>();
            for (int i = 0; i < nods.Count; i++)
            {
                if (nods[i] == null)
                {
                    nodsToRemove.Add(nods[i]);
                }
            }
            if ((nodsToRemove != null) && (nodsToRemove.Count != 0))
            {
                for (int i = 0; i < nodsToRemove.Count; i++)
                {
                    if (nods.Contains(nodsToRemove[i]))
                    {
                        nods.Remove(nodsToRemove[i]);
                    }
                }
            }
        }
    }

    //------------------------------------------------------------------------------

    //业务操作方法
    //新建path
    public void AddPath()
    {
        if (pathNum == 0)
        {
            AllPaths = new List<PathList>();
        }
        GameObject pathObject = new GameObject("Path_" + pathNum);
        pathObject.transform.SetParent(gameObject.transform);
        PathList path = pathObject.AddComponent<PathList>();
        path.manager = this;
        AllPaths.Add(path);
        pathNum++;

    }

    //删除最近创建的一个path
    public void DeleteOne()
    {
        DestroyImmediate(AllPaths[AllPaths.Count - 1].gameObject);
        AllPaths.Remove(AllPaths[AllPaths.Count - 1]);
        pathNum--;
        RemoveAllNullInAllNodsList(AllNods);
    }

    //删除所有path
    public void ClearAllPaths()
    {
        for (int i = 0; i < AllPaths.Count; i++)
        {
            if (AllPaths[i] != null)
            {
                DestroyImmediate(AllPaths[i].gameObject);

            }
        }
        AllPaths = new List<PathList>();
        pathNum = 0;
        RemoveAllNullInAllNodsList(AllNods);
    }

}
