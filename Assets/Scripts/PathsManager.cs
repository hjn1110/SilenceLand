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


    static int numOfSelf;

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

    private bool ConfirmToDeleteOne()
    {
        return UnityEditor.EditorUtility.DisplayDialog("确认删除", "是否要清空最近创建的一个nod？此操作行为不可撤销。", "确认", "取消");
    }
    //工具方法，用于移除当前allNods中为空的对象
    //因为destory所有Paths后，paths中包含的所有nods未从allNods中移除
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
    public void DeleteOne()
    {
        if ((AllPaths != null) && (AllPaths.Count != 0))
        {
            if (ConfirmToDeleteOne())
            {
                DestroyImmediate(AllPaths[AllPaths.Count - 1].gameObject);
                AllPaths.Remove(AllPaths[AllPaths.Count - 1]);
                pathNum--;
                //AllPaths = AllPaths.GetRange(0, pathNum);
                RemoveAllNullInAllNodsList(AllNods);
            }
        }
    }

    

    public void ClearAllPaths()
    {

        if ((AllPaths != null) && (AllPaths.Count != 0))
        {
            if (ConfirmToDelete())
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
    //inspector重写
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
                if (GUILayout.Button("DeleteOne"))
                {
                    manager.DeleteOne();
                }

            }

        }
    }
    //
    [MenuItem("GameObject/2D Object/PathManager")]
    static void CreatManager(MenuCommand menuCommand)
    {
        GameObject pathManager = new GameObject("PathManager");
        //pathManager.transform.parent = gameObject.transform;
        pathManager.AddComponent<PathsManager>();
        GameObjectUtility.SetParentAndAlign(pathManager, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(pathManager, "Create " + pathManager.name);
        Selection.activeObject = pathManager;

    }

#endif








}
