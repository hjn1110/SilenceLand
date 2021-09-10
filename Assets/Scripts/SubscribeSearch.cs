using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Silent.GameSystem.SearchObject;

/// <summary>
/// 扩展自：https://github.com/Bozar/AxeMan/blob/master/Assets/Scripts/SubscribeSearch.cs
/// 作者：Bozar
/// </summary>
namespace Silent.MapObject.SearchObject
{
    //订阅搜索事件
    public class SubscribeSearch : MonoBehaviour
    {
        private Global global;
        private Grid grid;

        //-----
        //判定方法
        //判定当前坐标是否符合查找条件
        private bool MatchCriteria(int[] position)
        {
            int[] localPostion = new int[2];
            Vector2 pos = transform.position;
            localPostion[0] = grid.WorldToCell(pos).x;
            localPostion[1] = grid.WorldToCell(pos).y;

            return (localPostion[0] == position[0]) && (localPostion[1] == position[1]);
        }
        //判定当前ID是否符合查找条件
        private bool MatchCriteria(int objectID)
        {
            return gameObject.GetInstanceID() == objectID;
        }

        //-----
        //初始化
        private void Start()
        {
            global = Global.instance;
            grid = global.grid;

            SearchGameObject sgo = global.sgo;

            //订阅全局搜索脚本中的搜索方法
            sgo.SearchingPosition += SubscribeSearch_SearchingPosition;
            sgo.SearchingID += SubscribeSearch_SearchingID;

        }

        private void SubscribeSearch_SearchingPosition(object sender,SearchingPositionEventArgs e)
        {
            if (MatchCriteria(e.Postion))
            {
                e.Data.Push(gameObject);
            }
        }
        private void SubscribeSearch_SearchingID(object sender, SearchingIDEventArgs e)
        {
            if (MatchCriteria(e.ObjectID))
            {
                e.Data.Push(gameObject);
            }
        }
    }







}
