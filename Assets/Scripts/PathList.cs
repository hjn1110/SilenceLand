using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PathList : MonoBehaviour
{
    static int id;
    public int NodsNum = 2;
    private List<PathNods> nods;
    private void Awake()
    {
        id++;
    }

    public void AddNods()
    {
        DeleteAll();
        int num = NodsNum;
        nods = new List<PathNods>(num);
        for(int i = 0; i < num; i++)
        {
            Add(i);


        }
        for(int i = 0; i < num; i++)
        {
            if (i != num - 1)
            {
                nods[i].nextNods = nods[i + 1].gameObject;
            }
        }
        
    }

    Vector2 Place(int i)
    {
        float x = 0;
        float y= 0;
        if (i  == 0)
        {
            x = 0;y = 1;
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


    GameObject Add(int i)
    {
        GameObject nodObject = new GameObject("Nod_" + i);
        nodObject.transform.parent = gameObject.transform;
        PathNods nod = nodObject.AddComponent<PathNods>();
        nods.Add(nod);
        nod.transform.position=Place(i);
        return nod.gameObject;
    }


    public void AddOne()
    {
        Open();
        NodsNum = nods.Count+1;
        int i = nods.Count;
        GameObject nod = Add(i);
        if (i - 1 >= 0)
        {
            nods[i - 1].nextNods = nod.gameObject;

        }
    }
    public void Clear()
    {
        DeleteAll();
        NodsNum = 2;

    }
    public void DeleteAll()
    {
        float childCount = transform.childCount;
        Debug.Log("childCount="+childCount);

        while (childCount != 0)
        {
            foreach (Transform child in transform)
            {
                Debug.Log("销毁:" + child.name);
                DestroyImmediate(child.gameObject);
            }
            childCount = transform.childCount;
        }
        nods = new List<PathNods>(NodsNum);
       

    }

    public void Open()
    {
        int childCount = transform.childCount;
        if ((childCount>=1)&&(nods[childCount - 1].nextNods != null))
        {
            nods[childCount - 1].nextNods = null;
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();

        }


    }

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
#if UNITY_EDITOR
[CustomEditor(typeof(PathList))]
public class PathBuilderEditor : Editor
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

