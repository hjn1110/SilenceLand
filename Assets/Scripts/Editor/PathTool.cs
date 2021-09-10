using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// 扩展自：https://blog.csdn.net/u014361280/article/details/108034891
/// 作者：仙魁XAN
/// </summary>
public class PathTool
{
    /* 路径常量 */
    public const string AB_RESOURCES = "AB_Resources";  // 打包AB包根路径

    /* 路径方法 */

    

    /// <summary>
    /// 得到 AB 资源的输入目录
    /// </summary>
    public static string GetABResourcesPath()
    {
        return Application.dataPath + "/" + AB_RESOURCES;
    }

    /// <summary>
    /// 获得 AB 包输出路径
    ///     1\ 平台(PC/移动端等)路径
    ///     2\ 平台名称
    /// </summary>
    public static string GetABOutPath()
    {
        return GetPlatformPath() + "/" + GetPlatformName();
    }

    /// <summary>
    /// 获得平台路径
    /// </summary>
    private static string GetPlatformPath()
    {

        string strReturenPlatformPath = string.Empty;

#if   UNITY_STANDALONE_OSX
        strReturenPlatformPath = Application.streamingAssetsPath;

#elif UNITY_STANDALONE_WIN
        strReturenPlatformPath = Application.streamingAssetsPath;
 
#elif UNITY_IPHONE
        strReturenPlatformPath = Application.persistentDataPath;
     
#elif UNITY_ANDROID
        strReturenPlatformPath = Application.persistentDataPath;

#endif

        return strReturenPlatformPath;
    }

    /// <summary>
    /// 获得平台名称
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
        string strReturenPlatformName = string.Empty;

#if   UNITY_STANDALONE_OSX
            strReturenPlatformName = "Mac";

#elif UNITY_STANDALONE_WIN
            strReturenPlatformName = "Windows";
 
#elif UNITY_IPHONE
                
            strReturenPlatformName = "IPhone";
#elif UNITY_ANDROID
 
            strReturenPlatformName = "Android";
#endif


        return strReturenPlatformName;
    }

    /// <summary>
    /// 返回 WWW 下载 AB 包加载路径
    /// </summary>
    /// <returns></returns>
    public static string GetWWWAssetBundlePath()
    {
        string strReturnWWWPath = string.Empty;

#if   UNITY_STANDALONE_OSX
            strReturnWWWPath = "file://" + GetABOutPath();

#elif UNITY_STANDALONE_WIN
            strReturnWWWPath = "file://" + GetABOutPath();
 
#elif UNITY_IPHONE
                
            strReturnWWWPath = GetABOutPath() + "/Raw/";
#elif UNITY_ANDROID
            strReturnWWWPath = "jar:file://" + GetABOutPath();

#endif

        return strReturnWWWPath;
    }

}//Class_End