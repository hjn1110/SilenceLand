using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 扩展自：https://blog.csdn.net/u014361280/article/details/108034891
/// 作者：仙魁XAN
/// </summary>
public class BuildAssetBundles
{

    [MenuItem("AssetBundleTools/BuildAllAssetBundles")]
    static void BuildAllAssetBundles()
    {
        //string OutPathDir = "AssetBundles";

        string OutPathDir = string.Empty;

        OutPathDir = PathTool.GetABOutPath();

        if (Directory.Exists(OutPathDir) == false)
        {
            Directory.CreateDirectory(OutPathDir);
        }

        //BuildTarget 选择build出来的AB包要使用的平台
        //BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

#if   UNITY_STANDALONE_OSX
            BuildPipeline.BuildAssetBundles(OutPathDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);

#elif UNITY_STANDALONE_WIN
            BuildPipeline.BuildAssetBundles(OutPathDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
 
#elif UNITY_IPHONE
                
            BuildPipeline.BuildAssetBundles(OutPathDir, BuildAssetBundleOptions.None, BuildTarget.iOS);
#elif UNITY_ANDROID
            BuildPipeline.BuildAssetBundles(OutPathDir, BuildAssetBundleOptions.None, BuildTarget.Android);
 
#endif


    }


}
