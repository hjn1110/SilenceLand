using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PathsManager : MonoBehaviour
{
    //此类实例必须为单例，待加以限制
    //[SerializeField]
    [ReadOnly]
    public int pathNum=0;

    [SerializeField]
    [ReadOnly]
    public List<PathNods> AllNods;//用于存储全局所有nods
    [SerializeField]
    [ReadOnly]
    public List<PathList> AllPaths;//用于存储全局所有path

    
    #region Singleton
    public static PathsManager instance;


    private void Awake()
    {
        instance = this;

    }
    #endregion
    


     

    //PATH
    //------------------------------------------------------------------------------
    //以下为创建paths相关的私有方法集，由公共方法组合调用

    private bool ConfirmToDelete()
    {
        return  UnityEditor.EditorUtility.DisplayDialog("确认删除","是否要清空所有path？此操作行为不可撤销。","确认","取消");
    }

    //以下为创建paths相关的公共方法集，由编辑器类调用

    public void AddPath()
    {
        if (pathNum == 0)
        {
            AllPaths = new List<PathList>();
        }
        GameObject pathObject = new GameObject("Path_" + pathNum);
        pathObject.transform.parent = gameObject.transform;
        PathList path = pathObject.AddComponent<PathList>();
        path.manager = this;
        AllPaths.Add(path);
        pathNum++;

    }
    public void ClearAllPaths()
    {
        if (ConfirmToDelete())
        {
            if ((AllPaths != null) && (AllPaths.Count != 0))
            {
                for (int i = 0; i < AllPaths.Count; i++)
                {
                    DestroyImmediate(AllPaths[i].gameObject);
                }
                AllPaths = new List<PathList>();
                pathNum = 0;
                Debug.Log("已清空");
            }
            else
            {
                Debug.LogError("待删除列表为空！");
            }
            
        }
        

    }

    //------------------------------------------------------------------------------
    //以下为编辑器类的重写，调用公共方法集，实现GUI面板上的按钮事件

#if UNITY_EDITOR
    [CustomEditor(typeof(PathsManager))]
    public class PathBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            {

                PathsManager manager = (PathsManager)target;

                if (GUILayout.Button("AddOne"))
                {
                    manager.AddPath();
                }
                if (GUILayout.Button("Clear"))
                {
                    manager.ClearAllPaths();
                }

            }

        }
    }
#endif








}
