using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public struct nod
{
    public Vector2Int index; 
    public Vector3Int position;     
}

public struct soundUnit
{
    public Vector3Int position;
    public float soundVolume;
}


public class SoundSpread : MonoBehaviour
{

    SoundSpreadVisualization sound { get { return SoundSpreadVisualization.instance; } }
    Global global { get { return Global.instance; } }

    private Tilemap tileHearIndeed { get { return sound.tileHearIndeed; } }
    private Tilemap tileHearBase { get { return sound.tileHearBase; } }
    private Tilemap tileReal { get { return global.tileReal; } }
    private TileBase tilebase0 { get { return sound.tilebase0; } }//用于听觉范围可视化
    private TileBase tilebase1 { get { return sound.tilebase1; } }//用于声音传导可视化
    private Grid grid { get { return global.grid; } }

    private int[,] tileTag;
    private int r = 30;

    List<nod> list0;//存储听觉范围
    List<nod> list1;//存储当前回合需检测的tile 
    List<nod> list3;//存储下个回合应检测的tile
    Dictionary<Vector3Int, float> SoundList;//用于存储完整听觉传导，以及其中每个格子的音量，用坐标可查询音量

    Vector3Int oP;
    int TimeOfSpread;

    
    private void Start()
    {
        //global = Global.instance;
        hearingList = new List<Transform>();
        refresh();
        AddTrigger();
        

    }

    public void MakeASound()
    {
        refresh();
        ContinuelyFlowIn();
    }

    
    private void OnEnable()
    {
        //Init();
        //ContinuelyFlowIn();
    }
    
     

    //刷新
    private void refresh()
    {
        tileHearIndeed.ClearAllTiles();
        tileHearBase.ClearAllTiles();

        list0 = new List<nod>();
        list1 = new List<nod>();
        list3 = new List<nod>();
        SoundList = new Dictionary<Vector3Int, float>();
        oP = grid.WorldToCell(gameObject.transform.position);
        tileTag = new int[2 * r + 1, 2 * r + 1];
        InitTile0();


        Vector2Int index = PosToIndex(oP);
        SetTag(index, 2);


        Vector3Int p = oP;
        nod nod1 = new nod();
        nod1.index = index;
        nod1.position = p;
        list1.Add(nod1);
        InitAround();
        TimeOfSpread = 0;

        //ContinuelyFlowIn();

    }

    /*
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            refresh();
            ContinuelyFlowIn();
            
        }
    }
    */

    //Transform enemyTrans;
    Vector3Int enemyTilePos;


    //用于判断敌人是否离开声场范围，离开该范围的敌人不进入检测
    public void RemoveEnemyInTrigger(Transform trans)
    {
        hearingList.Remove(trans);
    }

    List<Transform> hearingList;

    //用于判断敌人是否位于声场范围，基于位于的前提下进行听觉传导检测
    public void AddEnemyInTrigger(Transform trans)
    {
        hearingList.Add(trans);
        //Debug.Log("敌人位置位于"+pos);
        //Debug.Log("敌人进入声音范围！");
        //enemyTrans = trans;

    }


    void AddTrigger()
    {

        GameObject hearField = TriggerCreater.instance.AddTriggerObject(r, transform, "HearField");
        hearField.AddComponent<SoundField>();

    }

    void GetNpcInField(Transform enemyTrans)
    {
        float volume;
        enemyTilePos = grid.WorldToCell(enemyTrans.position);
        //Debug.Log("tilePos=" + enemyTilePos);
        if (SoundList.ContainsKey(enemyTilePos))
        {
            volume = SoundList[enemyTilePos];
            Debug.Log("监听者听到:"+enemyTrans.gameObject.name+",音量="+ volume);
            enemyTrans.gameObject.GetComponent<Enemies>().Hear(gameObject.transform.position, volume);
        }
        else
        {
            Debug.Log("监听者未听到:"+enemyTrans.gameObject.name);
        }

    }


    void ContinuelyFlowIn()
    {
        StopAllCoroutines();
        StartCoroutine(Flowing());
    }
    IEnumerator Flowing()
    {
        SoundList.Clear();
        while (TimeOfSpread < r)
        {
            TimeOfSpread++;
            FlowIn();
            yield return null;
        }

        if ((hearingList != null)&&(hearingList.Count>0))
        {
            //Debug.Log("监听者列表非空");
            for (int i = 0; i < hearingList.Count; i++)
            {
                //Debug.Log("检测监听者"+hearingList[i].gameObject.name);
                GetNpcInField(hearingList[i]);
            }
                

        }
        else
        {
            //Debug.Log("监听者列表为空");
        }

    }

    List<Vector3Int> around;

    void InitAround()
    {
        around = new List<Vector3Int>();

        
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (!((i == 0) && (j == 0)))
                {
                    around.Add(new Vector3Int(i,j,0));
                }
            }
        }
        


        //Debug.Log("around:"+around.Count);

    }

    

    void SetTag(Vector2Int index,int tag)
    {
        tileTag[index.x, index.y] = tag;
    }

    int PosToTag(Vector3Int pos)
    {
        Vector2Int index = PosToIndex(pos);
        //Debug.Log("index="+index.x+","+index.y);
        int tag = tileTag[index.x,index.y];
        return tag;
    }

    Vector3Int IndexToPos(Vector2Int index)
    {
        int px = index.x + oP.x - r ;
        int py = index.y + oP.y - r ;
        Vector3Int position = new Vector3Int(px, py,0);
        return position;
    }

    Vector2Int PosToIndex(Vector3Int pos)
    {
        int ix = pos.x - oP.x + r ;
        int iy = pos.y - oP.y + r ;
        Vector2Int index = new Vector2Int(ix, iy);
        return index;
    }

    //传播次数转音量计算函数，需扩展
    float TimeToVolume()
    {
        float volume = ((float)r - (float)TimeOfSpread)/ (float)r;

        return volume;
    }

    void FlowIn()
    {
        int num = 0;
        int scan = 0;



        for (int i = 0; i < list1.Count; i++)
        {
            for (int j = 0; j < around.Count; j++)
            {
                Vector3Int p = new Vector3Int(list1[i].position.x + around[j].x, list1[i].position.y + around[j].y, 0);
                scan++;
                if ((p.x >= oP.x - r) && (p.x - r <= oP.x + r) && (p.y >= oP.y - r) && (p.y - r <= oP.y + r)) 
                {
                     
                    if (PosToTag(p)==1)
                    {
                        //Debug.Log("待查点确在视野中,且未被标记");

                        //Debug.Log("待查点未被标记过");
                        
                        if (tileReal.GetColliderType(list1[i].position + around[j]) == Tile.ColliderType.None)
                        {
                            //Debug.Log("填色！");

                            tileHearIndeed.SetTile(p, tilebase1);
                            //List4.Add(p);

                            SoundList.Add(p, TimeToVolume());
                            nod nod1 = new nod();
                            nod1.position = p;
                            Vector2Int index = PosToIndex(p);
                            nod1.index = index;

                            SetTag(index, 2);
                            list3.Add(nod1);
                            num++;
                        }


                    }
     
                }
                
            }
            
        }
        list1.Clear();
        if (list3.Count != 0)
        {
            for (int n = 0; n < list3.Count; n++)
            {
                list1.Add(list3[n]);
            }
        }
        list3.Clear();
        //Debug.Log("本次扫描" + scan + "个方块;"+"本次渲染" +num+"个方块");
        /*
        if (num == 0)
        {
            return (false);
        }
        else
        {
            return (true);
        }
        */


    }



    void InitTile0()
    {

        for (int i = -r; i <= r; i++)
        {
            for (int j = -r; j <= r; j++)
            {
                if (Math.Pow(i , 2) + Math.Pow(j, 2) <= r * r)
                {
                    Vector3Int p = new Vector3Int(i+oP.x , j+oP.y , 0);
                    if (tileReal.GetColliderType(p) == Tile.ColliderType.None)
                    {
                        nod nod1 = new nod();
                        nod1.position = p;
                        Vector2Int index = PosToIndex(p);
                        nod1.index = index;
                        SetTag(index, 1);
                        list0.Add(nod1);
                        tileHearBase.SetTile(p, tilebase0);
                    }
                }

            }

        }
    }



}
