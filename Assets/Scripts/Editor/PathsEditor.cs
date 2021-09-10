using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//------------------------------------------------------------------------------
//以下为编辑器类的重写，调用公共方法集，实现GUI面板上的按钮事件

#if UNITY_EDITOR
//inspector重写
[CustomEditor(typeof(PathsManager))]
public class PathsEditor : Editor
{
    

    private bool ConfirmToDelete()
    {
        return UnityEditor.EditorUtility.DisplayDialog("确认删除", "是否要清空所有path？此操作行为不可撤销。", "确认", "取消");
    }

    private bool ConfirmToDeleteOne()
    {
        return UnityEditor.EditorUtility.DisplayDialog("确认删除", "是否要清空最近创建的一个nod？此操作行为不可撤销。", "确认", "取消");
    }

   


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
                if (manager.OnClearAllPaths())
                {
                    if (ConfirmToDelete())
                    {
                        manager.ClearAllPaths();
                    }
                    else
                    {
                        Debug.LogError("待删除列表为空！");
                    }
                }
            }
            if (GUILayout.Button("DeleteOne"))
            {
                if (manager.OnDeleteOne())
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
}


#endif









