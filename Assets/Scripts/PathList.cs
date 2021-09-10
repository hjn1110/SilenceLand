using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathList : MonoBehaviour
{
    //字段声明
    public int NodsNum = 2;
    [ReadOnly]
    public List<PathNods> nods;

    //全局变量容器
    public PathsManager manager;

    //------------------------------------------------------------------------------

    //条件方法集：由PathsEditor调用并关联相应业务操作方法，并在条件触发时调用
    //满足创建Nods的条件
    public bool OnAddNods()
    {
        //若当前存在可能被覆盖的nods，则二次确认提示，否则不提示直接创建
        if ((nods != null) && (nods.Count != 0))
        {
            return true;
        }
        return false;
    }

    //满足清除nods并初始化nodsNum的条件
    public bool OnClear()
    {
        return true;
    }

    //满足裁剪当前path路径，将nods删至NodsNum的条件
    public bool OnDelete()
    {
        if ((nods != null) && (nods.Count != 0))
        {
            if (NodsNum < nods.Count)
            {
                for (int i = NodsNum; i < nods.Count; i++)
                {
                    DestroyImmediate(nods[i].gameObject);
                    manager.AllNods.Remove(nods[i]);
                }

                manager.RemoveAllNullInAllNodsList(nods);
                Close();
                return true;
            }
            return false;
        }
        return false;
    }

    //------------------------------------------------------------------------------
    //业务操作方法
    //以下为私有方法集，由公共方法组合调用

    //创建nod
    private GameObject Add(int i)
    {
        GameObject nodObject = new GameObject("Nod_" + i);
        nodObject.transform.SetParent(gameObject.transform);
        PathNods nod = nodObject.AddComponent<PathNods>();
        nods.Add(nod);
        if ((manager.AllNods==null)||(manager.AllNods.Count == 0))
        {
            manager.AllNods = new List<PathNods>();
        }
        else
        {
            Debug.Log("manager.AllNods为空");
        }
        manager.AllNods.Add(nod);
        Debug.Log("AddAllNods:"+ manager.AllNods.Count);

        nod.transform.position = Place(i);
        return nod.gameObject;
    }

    //放置nods，使新创建的nod与既有的离散，便于可视化，避免视觉上发生重叠
    private Vector2 Place(int i)
    {
        float x;
        float y;
        if (i == 0)
        {
            x = 0; y = 1;
        }
        else
        {
            x = nods[i - 1].transform.position.x;
            y = nods[i - 1].transform.position.y;
            switch ((i - 1) % 4)
            {
                case 0:
                    y += 1;
                    break;
                case 1:
                    x += 1;
                    break;
                case 2:
                    y -= 1;
                    break;
                case 3:
                    x += 1;
                    break;
            }
        }
        return new Vector2(x, y);
    }
    
    //------------------------------------------------------------------------------
    //以下为创建nods相关的公共方法集，由编辑器类调用

    //清除所有nods
    public void DeleteAll()
    {
        if ((nods != null) && (nods.Count != 0))
        {
            for (int i = 0; i < nods.Count; i++)
            {
                DestroyImmediate(nods[i].gameObject);
                manager.AllNods.Remove(nods[i]);
            }

            nods = new List<PathNods>(NodsNum);
            Debug.Log("已清空");
        }
        else
        {
            Debug.Log("待删除列表为空！");
        }
    }

    public void CreatAll()
    {
        DeleteAll();
        int num = NodsNum;
        nods = new List<PathNods>(num);
        for (int i = 0; i < num; i++)
        {
            Add(i);
        }
        for (int i = 0; i < num; i++)
        {
            if (i != num - 1)
            {
                nods[i].nextNods = nods[i + 1].gameObject;
            }
        }
        Close();
    }

    //创建一个nod，并初始化路径关系
    public void AddOne()
    {
        Open();
        if ((nods == null) || (nods.Count == 0))
        {
            nods = new List<PathNods>();
        }
        NodsNum = nods.Count+1;
        int i = nods.Count;
        GameObject nod = Add(i);
        if (i - 1 >= 0)
        {
            nods[i - 1].nextNods = nod.gameObject;
        }
        Close();
    }

    public void Clear()
    {
        DeleteAll();
        NodsNum = 2;
    }

    //打开当前path路径
    public bool Open()
    {
        int childCount = transform.childCount;
        if ((childCount>=1)&&(nods[childCount - 1].nextNods != null))
        {
            nods[childCount - 1].nextNods = null;
            //UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            //立刻刷新既有对象之间的连线，否则会延迟至少2秒左右刷新，因为OnDraw函数不会每帧刷新
            //由于调用Editor相关方法，改为在返回true后在Editor类中调用
            return true;
        }
        return false;
    }

    //闭合当前path路径
    public bool Close()
    {
        int childCount = transform.childCount;
        if (childCount >= 1)
        {
            nods[childCount - 1].nextNods = nods[0].gameObject;
            //UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            //立刻刷新既有对象之间的连线，否则会延迟至少2秒左右刷新，因为OnDraw函数不会每帧刷新
            //由于调用Editor相关方法，改为在返回true后在Editor类中调用
            return true;
        }
        return false;
    }
}


