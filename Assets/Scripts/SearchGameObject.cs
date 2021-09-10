using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 扩展自：https://github.com/Bozar/AxeMan/blob/master/Assets/Scripts/SearchObject.cs
/// 作者：Bozar
/// </summary>
namespace Silent.GameSystem.SearchObject
{
    //接口类型声明
    //必须实现Search方法，传参方式有两种
    public interface ISearchObject
    {
        GameObject[] Search(int x,int y);
        GameObject[] Search(int objectID);
    }

    //--------------------------------------------------------------------------
    //事件类型声明，指定发布广播时的数据格式

    //根据id进行Search的事件类型
    public class SearchingIDEventArgs: EventArgs
    {
        //构造函数 objectID是传入的id，Stack<GameObject> 用于存储数据
        public SearchingIDEventArgs(int objectID, Stack<GameObject> data)
        {
            ObjectID = objectID;
            Data = data;
        }
        //获取实现时传入的值
        public Stack<GameObject> Data { get; }
        public int ObjectID { get; }
    }

    //根据tileMap坐标(Grid坐标)进行Search的事件类型
    public class SearchingPositionEventArgs : EventArgs
    {
        //构造函数 int[]是一个只有两个成员的数字，用于表示坐标
        public SearchingPositionEventArgs(int[] position, Stack<GameObject> data)
        {
            //传参数的作用是：将搜索标准和数据容器传递给订阅者
            //声明内部成员的作用：将订阅者返回的数据传回给搜索发布者
            Postion = position;
            Data = data;
        }
        public Stack<GameObject> Data { get; }
        public int[] Postion { get; }
    }

    //--------------------------------------------------------------------------
    //搜索组件声明（具体实现）

    public class SearchGameObject : MonoBehaviour,ISearchObject
    {
        //-----
        //声明事件类型，用于发布广播，以及用于被各个对象订阅
        public event EventHandler<SearchingPositionEventArgs> SearchingPosition;
        public event EventHandler<SearchingIDEventArgs> SearchingID;

        //-----
        //声明Search方法
        //判定是否有结果
        public bool Search(int x,int y,out GameObject[] result)
        {
            result = Search(x,y);
            return result.Length > 0;
        }
        public bool Search(int objectID,out GameObject[] result)
        {
            result = Search(objectID);
            return result.Length > 0;
        }
        //搜索方法
        public GameObject[] Search(int x, int y)
        {
            //将传入的int坐标转换为数组
            int[] pos = new int[] { x, y };
            //创建用于存储返回内容的数据
            Stack<GameObject> data = new Stack<GameObject>();
            //创建事件
            var ea = new SearchingPositionEventArgs(pos, data);
            //发布广播，并将包含了坐标信息和数据容器的事件作为参数传递给订阅者
            OnSearchingPosition(ea);
            //获取回调，获取订阅者push到事件包含的容器中的数据
            return ea.Data.ToArray();
        }
        public GameObject[] Search(int objectID)
        {
            Stack<GameObject> data = new Stack<GameObject>();
            var ea = new SearchingIDEventArgs(objectID, data);

            OnSearchingID(ea);
            return ea.Data.ToArray();
        }

        //-----
        //声明虚方法
        //用于发布广播

        protected virtual void OnSearchingPosition(SearchingPositionEventArgs e)
        {
            SearchingPosition?.Invoke(this, e);
        }

        protected virtual void OnSearchingID(SearchingIDEventArgs e)
        {
            SearchingID?.Invoke(this, e);
        }

        
    }


}



