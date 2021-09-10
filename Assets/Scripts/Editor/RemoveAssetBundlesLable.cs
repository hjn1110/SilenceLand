﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

/// <summary>
/// 扩展自：https://blog.csdn.net/u014361280/article/details/108044890
/// 作者：仙魁XAN
/// </summary>
public class RemoveAssetBundlesLable
{

    [MenuItem("AssetBundleTools/Remove AB Label")]
    public static void RemoveABLabel()
    {
        // 需要移除标记的根目录
        string strNeedRemoveLabelRoot = string.Empty;
        // 目录信息（场景目录信息数组，表示所有根目录下场景目录）
        DirectoryInfo[] directoryDIRArray = null;


        // 定义需要移除AB标签的资源的文件夹根目录
        strNeedRemoveLabelRoot = PathTool.GetABResourcesPath();
        //Debug.Log("strNeedSetLabelRoot = "+strNeedSetLabelRoot);

        DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedRemoveLabelRoot);
        directoryDIRArray = dirTempInfo.GetDirectories();

        // 遍历本场景目录下所有的目录或者文件
        foreach (DirectoryInfo currentDir in directoryDIRArray)
        {
            // 递归调用方法，找到文件，则使用 AssetImporter 类，标记“包名”与 “后缀名”
            JudgeDirOrFileByRecursive(currentDir);
        }

        // 清空无用的 AB 标记
        AssetDatabase.RemoveUnusedAssetBundleNames();
        // 刷新
        AssetDatabase.Refresh();

        // 提示信息，标记包名完成
        Debug.Log("AssetBundle 本次操作移除标记完成");

    }

    /// <summary>
    /// 递归判断判断是否是目录或文件
    /// 是文件，修改 Asset Bundle 标记
    /// 是目录，则继续递归
    /// </summary>
    /// <param name="fileSystemInfo">当前文件信息（文件信息与目录信息可以相互转换）</param>
    private static void JudgeDirOrFileByRecursive(FileSystemInfo fileSystemInfo)
    {
        // 调试信息
        //Debug.Log("currentDir.Name = " + fileSystemInfo.Name);
        //Debug.Log("sceneName = " + sceneName);

        // 参数检查
        if (fileSystemInfo.Exists == false)
        {
            Debug.LogError("文件或者目录名称：" + fileSystemInfo + " 不存在，请检查");
            return;
        }

        // 得到当前目录下一级的文件信息集合
        DirectoryInfo directoryInfoObj = fileSystemInfo as DirectoryInfo;           // 文件信息转为目录信息
        FileSystemInfo[] fileSystemInfoArray = directoryInfoObj.GetFileSystemInfos();

        foreach (FileSystemInfo fileInfo in fileSystemInfoArray)
        {
            FileInfo fileInfoObj = fileInfo as FileInfo;

            // 文件类型
            if (fileInfoObj != null)
            {
                // 修改此文件的 AssetBundle 标签
                RemoveFileABLabel(fileInfoObj);
            }
            // 目录类型
            else
            {

                // 如果是目录，则递归调用
                JudgeDirOrFileByRecursive(fileInfo);
            }
        }
    }

    /// <summary>
    /// 给文件移除 Asset Bundle 标记
    /// </summary>
    /// <param name="fileInfoObj">文件（文件信息）</param>
    static void RemoveFileABLabel(FileInfo fileInfoObj)
    {
        // 调试信息
        //Debug.Log("fileInfoObj.Name = " + fileInfoObj.Name);
        //Debug.Log("scenesName = " + scenesName);

        // 参数定义
        // AssetBundle 包名称
        string strABName = string.Empty;
        // 文件路径（相对路径）
        string strAssetFilePath = string.Empty;

        // 参数检查（*.meta 文件不做处理）
        if (fileInfoObj.Extension == ".meta")
        {
            return;
        }

        // 得到 AB 包名称
        strABName = string.Empty;
        // 获取资源文件的相对路径
        int tmpIndex = fileInfoObj.FullName.IndexOf("Assets");
        strAssetFilePath = fileInfoObj.FullName.Substring(tmpIndex);        // 得到文件相对路径


        // 给资源文件移除 AB 名称
        AssetImporter tmpImportObj = AssetImporter.GetAtPath(strAssetFilePath);
        tmpImportObj.assetBundleName = strABName;


    }
}
