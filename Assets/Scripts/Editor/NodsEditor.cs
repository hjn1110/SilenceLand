using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//------------------------------------------------------------------------------
//以下为编辑器类的重写，调用公共方法集，实现GUI面板上的按钮事件

#if UNITY_EDITOR
[CustomEditor(typeof(PathList))]
public class NodsEditor : Editor
{

    //私有方法，用于弹窗二次确认
    private bool ConfirmToDelete()
    {
        return UnityEditor.EditorUtility.DisplayDialog("确认删除", "是否要清空当前path下所有nods？此操作行为不可撤销。", "确认", "取消");
    }
    private bool ConfirmToNew()
    {
        return UnityEditor.EditorUtility.DisplayDialog("确认新建", "新建行为会覆盖当前已创建的所有nods。此操作行为不可撤销。", "确认", "取消");
    }
    private bool WarnOutOfIndex()
    {
        return UnityEditor.EditorUtility.DisplayDialog("输入错误", "输入的数值超出边界，请重新检查", "确认", "取消");
    }

    //私有方法，用于刷新画面UI
    private void RePaintGUI()
    {
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }

    /// <summary>
    /// 根据编辑器点击事件，调用执行方法
    /// 根据执行方法返回值，确认时机，调用二次确认或报错
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            //DrawDefaultInspector();

            PathList nodsList = (PathList)target;

            if (GUILayout.Button("New"))
            {
                if (nodsList.OnAddNods())
                {
                    if (ConfirmToNew())
                    {
                        nodsList.CreatAll();
                    }
                }
                else
                {
                    nodsList.CreatAll();
                }
            }
            if (GUILayout.Button("AddOne"))
            {
                nodsList.AddOne();
            }
            if (GUILayout.Button("DeleteTo"))
            {
                if (!nodsList.OnDelete())
                {
                    WarnOutOfIndex();
                }
            }
            if (GUILayout.Button("Clear"))
            {
                if (nodsList.OnClear())
                {
                    if (ConfirmToDelete())
                    {
                        nodsList.Clear();

                    }
                }
            }
            if (GUILayout.Button("Close"))
            {
                if (nodsList.Close())
                {
                    RePaintGUI();
                }
            }
            if (GUILayout.Button("Open"))
            {
                if (nodsList.Open())
                {
                    RePaintGUI();
                }

            }
        }
    }
}
#endif






