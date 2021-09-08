using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PathList : MonoBehaviour
{
    //public static int num { set; get; }
    [HideInInspector]
    public int id;

    public int NodsNum = 2;
    //[SerializeField]
    private List<PathNods> nods;
    //public static List<PathList> AllPaths;

    public PathsManager manager;

     




    //NODS
    //------------------------------------------------------------------------------
    //以下为创建nods相关的私有方法集，由公共方法组合调用

    //创建nod
    private GameObject Add(int i)
    {
        GameObject nodObject = new GameObject("Nod_" + i);
        nodObject.transform.parent = gameObject.transform;
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

    //清除所有nods
    private void DeleteAll()
    {
        if ((nods != null) && (nods.Count != 0))
        {
            for (int i = 0; i < nods.Count; i++)
            {
                DestroyImmediate(nods[i].gameObject);
            }

            nods = new List<PathNods>(NodsNum);
            Debug.Log("已清空");

        }
        else
        {
            Debug.Log("待删除列表为空！");

        }

    }

    //二次确认
    private bool ConfirmToDelete()
    {
        return UnityEditor.EditorUtility.DisplayDialog("确认删除", "是否要清空当前path下所有nods？此操作行为不可撤销。", "确认", "取消");
    }
    private bool ConfirmToNew()
    {
        return UnityEditor.EditorUtility.DisplayDialog("确认新建", "新建行为会覆盖当前已创建的所有nods。此操作行为不可撤销。", "确认", "取消");
    }

    //------------------------------------------------------------------------------
    //以下为创建nods相关的公共方法集，由编辑器类调用

    //根据给定nodsNum批量创建nods，并初始化路径关系
    public void AddNods()
    {
        if (ConfirmToNew())
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
        }

        
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
    }

    //清除所有nods，并初始化nodsNum
    public void Clear()
    {
        if (ConfirmToDelete())
        {
            DeleteAll();
            NodsNum = 2;

        }

    }

    //打开当前path路径
    public void Open()
    {
        int childCount = transform.childCount;
        if ((childCount>=1)&&(nods[childCount - 1].nextNods != null))
        {
            nods[childCount - 1].nextNods = null;
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }

    //闭合当前path路径
    public void Close()
    {
        int childCount = transform.childCount;
        if (childCount >= 1)
        {
            nods[childCount - 1].nextNods = nods[0].gameObject;
            //HandleUtility.Repaint();
            //OnDrawGizmos();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            //立刻刷新既有对象之间的连线，否则会延迟至少2秒左右刷新，因为OnDraw函数不会每帧刷新
        }

    }

    //裁剪当前path路径，将nods删至NodsNum的数量
    public void Delete()
    {
        int childCount = transform.childCount;
        Debug.Log("childCount=" + childCount);

        while (childCount != NodsNum)
        {
            for(int i= transform.childCount; i>NodsNum;i--)
            {
                Transform child = transform.GetChild(i-1);
                Debug.Log("销毁:" + child.gameObject.name);
                DestroyImmediate(child.gameObject);
            }
            childCount = transform.childCount;
        }
        NodsNum = childCount;
        nods = nods.GetRange(0,childCount);
    }

}

//------------------------------------------------------------------------------
//以下为编辑器类的重写，调用公共方法集，实现GUI面板上的按钮事件

#if UNITY_EDITOR
[CustomEditor(typeof(PathList))]
public class PathNodsBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            //DrawDefaultInspector();

            PathList nodsList = (PathList)target;

            if (GUILayout.Button("New"))
            {
                
                nodsList.AddNods();
            }
            if (GUILayout.Button("AddOne"))
            {
                nodsList.AddOne();
            }
            if (GUILayout.Button("DeleteTo"))
            {
                nodsList.Delete();
            }
            if (GUILayout.Button("Clear"))
            {
                nodsList.Clear();
            }
            if (GUILayout.Button("Close"))
            {
                nodsList.Close();
            }
            if (GUILayout.Button("Open"))
            {
                nodsList.Open();
            }

        }

    }
}
#endif

