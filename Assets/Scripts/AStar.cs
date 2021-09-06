using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    //地图节点类型
    public enum NodeType
    {
        moveable,   //可移动
        bar,        //障碍物
        boundary,   //边界
        aStarPath   //A星路径
    }

    //A星状态
    public enum AStarState
    {
        free,
        isInOpenList,
        isInCloseList
    }

    //pos<-->node 字典
    private Dictionary<Vector2, Node> PosNodeDict;

    //地图节点
    public class Node
    {
        public Vector2 pos;
        public NodeType nodeType;
        public AStarState aStarState;
        public Node[] neighbourNodes;
        public Node parentNode = null;
        public float F = 0;
        public float G = 0;
        public float H = 0;
    }

    //地图初始化
    public Node[,] InitMap(int mapHeight, int mapLength)
    {
        Node[,] nodes = new Node[mapHeight, mapLength];
        PosNodeDict = new Dictionary<Vector2, Node>();

        
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapLength; j++)
            {
                nodes[i, j] = new Node();
                //边界判断
                if (i == 0)
                {
                    nodes[i, j].nodeType = NodeType.boundary;
                    nodes[i, j].pos = new Vector2(j, i);
                }
                else if (j == 0)
                {
                    nodes[i, j].nodeType = NodeType.boundary;
                    nodes[i, j].pos = new Vector2(j, i);
                }
                else if (i == mapHeight - 1)
                {
                    nodes[i, j].nodeType = NodeType.boundary;
                    nodes[i, j].pos = new Vector2(j, i);
                }
                else if (j == mapLength - 1)
                {
                    nodes[i, j].nodeType = NodeType.boundary;
                    nodes[i, j].pos = new Vector2(j, i);
                }
                else
                {
                    nodes[i, j].nodeType = NodeType.moveable;
                    nodes[i, j].pos = new Vector2(j, i);
                }

                //节点加入 坐标<-->节点 字典
                nodes[i, j].aStarState = AStarState.free;
                PosNodeDict.Add(new Vector2(j, i), nodes[i, j]);
            }
        }
        

        //初始化相邻坐标节点数组
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapLength; j++)
            {
                nodes[i, j].neighbourNodes = new Node[8];
                Vector2 curNeighbVec2;
                //上
                curNeighbVec2 = new Vector2(j, i + 1);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[0] = PosNodeDict[curNeighbVec2];
                }
                //下
                curNeighbVec2 = new Vector2(j, i - 1);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[1] = PosNodeDict[curNeighbVec2];
                }
                //左
                curNeighbVec2 = new Vector2(j - 1, i);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[2] = PosNodeDict[curNeighbVec2];
                }
                //右
                curNeighbVec2 = new Vector2(j + 1, i);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[3] = PosNodeDict[curNeighbVec2];
                }
                //左上
                curNeighbVec2 = new Vector2(j - 1, i + 1);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[4] = PosNodeDict[curNeighbVec2];
                }
                //右上
                curNeighbVec2 = new Vector2(j + 1, i + 1);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[5] = PosNodeDict[curNeighbVec2];
                }
                //左下
                curNeighbVec2 = new Vector2(j - 1, i - 1);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[6] = PosNodeDict[curNeighbVec2];
                }
                //右下
                curNeighbVec2 = new Vector2(j + 1, i - 1);
                if (PosNodeDict.ContainsKey(curNeighbVec2))
                {
                    nodes[i, j].neighbourNodes[7] = PosNodeDict[curNeighbVec2];
                }
            }
        }
        return nodes;
    }

    //设置障碍bar
    public void AddBar(Vector2 barPos)
    {
        if (PosNodeDict.ContainsKey(barPos))
        {
            print("bar added");
            PosNodeDict[barPos].nodeType = NodeType.bar;
        }
    }

    //地图具象化
    /*
    public void InstantiateMap(Node[,] nodes)
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                GameObject curCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                curCube.transform.position = new Vector3(j, i, 0);
                if (nodes[i, j].nodeType == NodeType.boundary)
                {
                    print("set boundary");
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                }
                else if (nodes[i, j].nodeType == NodeType.bar)
                {
                    print("set bar");
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                }
                else if (nodes[i, j].nodeType == NodeType.aStarPath)
                {
                    print("set path");
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                }
            }
        }
    }
    */

    //地图具象化（并对Openlist，Closelist内节点上色）
    /*
    public void InstantiateMapBeta(Node[,] nodes)
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                GameObject curCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                curCube.transform.position = new Vector3(j, i, 0);
                if (nodes[i, j].nodeType == NodeType.boundary)
                {
                    print("set boundary");
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                }
                else if (nodes[i, j].nodeType == NodeType.bar)
                {
                    print("set bar");
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                }
                else if (nodes[i, j].nodeType == NodeType.aStarPath)
                {
                    print("set path");
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                }
                else if (OpenList.Contains(nodes[i, j]))
                {
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else if (CloseList.Contains(nodes[i, j]))
                {
                    curCube.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                }
            }
        }
    }
    */

    //寻路相关
    private List<Node> OpenList;
    private List<Node> CloseList;
    private Vector2 globalEndPos;
    //寻路入口
    public bool PathSearch(Node[,] nodes, Vector2 startPos, Vector2 endPos)
    {
        //校验参数
        if (!PosNodeDict.ContainsKey(startPos) || !(PosNodeDict.ContainsKey(endPos)))
        {
            print("invalid para");
            return false;
        }
        if (PosNodeDict[startPos].nodeType != NodeType.moveable || PosNodeDict[endPos].nodeType != NodeType.moveable)
        {
            print("invalid para");
            return false;
        }

        OpenList = new List<Node>();
        CloseList = new List<Node>();
        globalEndPos = endPos;
        //算法开始
        //起点为A
        //这里有点不直观，startPos的xy要倒过来引用nodes。感谢tuyou67评论指出。
        Node A = nodes[(int)startPos.y, (int)startPos.x];
        A.G = 0;
        A.H = Mathf.Abs(globalEndPos.x - A.pos.x) + Mathf.Abs(globalEndPos.y - A.pos.y); //Vector2.Distance(A.pos, globalEndPos);
        A.F = A.G + A.H;
        A.parentNode = null;
        CloseList.Add(A);
        A.aStarState = AStarState.isInCloseList;
        do
        {
            //遍历OpenList，寻找F值最小的节点，设为A
            if (OpenList.Count > 0)
            {
                A = OpenList[0];
            }
            for (int i = 0; i < OpenList.Count; i++)
            {
                if (OpenList[i].F < A.F)
                {
                    A = OpenList[i];
                }
            }

            Node path = AStarSearch(A);

            if (path != null)
            {
                print("path found");
                do
                {
                    path.nodeType = NodeType.aStarPath;
                    if (path.parentNode == null)
                    {
                        path = null;
                    }
                    else
                    {
                        path = path.parentNode;
                    }
                } while (path != null);
                return true;
            }
            OpenList.Remove(A);
            CloseList.Add(A);
            A.aStarState = AStarState.isInCloseList;
            //OpenList是否还有节点
        } while (OpenList.Count > 0);
        //无到达目的地的路径
        print("path not found");
        return false;
    }

    public Node AStarSearch(Node A)
    {
        //遍历A的周边节点，当前处理节点为B
        Node B;
        for (int i = 0; i < A.neighbourNodes.Length; i++)
        {
            if (A.neighbourNodes[i] == null)
            {
                continue;
            }

            B = A.neighbourNodes[i];
            //是否是可移动节点
            if (B.nodeType != NodeType.moveable)
            {
                continue;
            }
            //是否在开放列表中
            if (B.aStarState == AStarState.isInOpenList)
            {
                //A到B的G值+A.G>B.G
                float curG = Vector2.Distance(A.pos, B.pos);
                if (B.G > curG + A.G)
                {
                    //更新B的父节点为A，并相应更新B.G,B.H
                    B.parentNode = A;
                    B.G = curG + A.G;
                    B.F = B.G + B.H;
                }
                continue;
            }
            else if (B.aStarState == AStarState.free)
            {
                //更新B的父节点为A，并相应更新B.G; 计算B.F,B.H; B加入OpenList
                B.parentNode = A;
                B.G = Vector2.Distance(A.pos, B.pos) + A.G;
                B.H = Mathf.Abs(globalEndPos.x - B.pos.x) + Mathf.Abs(globalEndPos.y - B.pos.y); //Vector2.Distance(B.pos, globalEndPos);
                B.F = B.G + B.H;
                OpenList.Add(B);
                B.aStarState = AStarState.isInOpenList;
                //B.F==0
                if (B.H < Mathf.Epsilon)
                {
                    //B的所有父节点既是路径
                    return B;
                }
                else
                {
                    //继续遍历
                    continue;
                }
            }
            else
            {
                continue;
            }
        }
        return null;
    }

    Grid grid;
    Global global;

    //程序入口
    // Use this for initialization
    void Start()
    {
        global = Global.instance;
        grid = global.grid;

        Node[,] nodes = InitMap(200, 200);

        AddBar(new Vector2(2, 2));
        AddBar(new Vector2(2, 3));
        AddBar(new Vector2(3, 2));
        AddBar(new Vector2(3, 3));
        AddBar(new Vector2(4, 4));
        AddBar(new Vector2(3, 4));
        AddBar(new Vector2(4, 3));
        AddBar(new Vector2(4, 5));
        AddBar(new Vector2(5, 4));
        AddBar(new Vector2(4, 6));
        AddBar(new Vector2(4, 7));
        AddBar(new Vector2(1, 7));
        AddBar(new Vector2(2, 7));
        AddBar(new Vector2(3, 7));

        PathSearch(nodes, new Vector2(1, 3), new Vector2(2, 12));

        //InstantiateMapBeta(nodes);
    }

    GameObject player { get { return global.player; } }

     
}

