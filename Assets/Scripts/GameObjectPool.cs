using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using Sirenix.OdinInspector;

public class GameObjectPool : SerializedMonoBehaviour
{
    //公共字段
    private string poolName;
    public int maxCount = 10;
    public int minCount = 0;
    //[SerializeField]
    //public Queue<GameObject> poolQueue;
    //public ConcurrentQueue<GameObject> poolCCQueue;

    public Stack<GameObject> poolStack;
    public GameObject preFab;

    public Transform instanceParentTrans;

    private Transform parentTrans;

    GameObjectPoolManager manager;

    //属性
    //private Transform PoolTrans { get { return transform; } }

    public void InitDeadLine()
    {
        /*
        //初始化最低限度的对象，避免频繁生成时触底
        for (int i = 0; i < minCount; i++)
        {
            StoreInstance(NewInstance());

        }
        */
    }


    //由manager调用
    public virtual void InitPool(string objPoolName,Transform objTransform)
    {
        manager = GameObjectPoolManager.instance;
        instanceParentTrans = manager.instanceContent.transform;

        //poolQueue = new Queue<GameObject>();
        //poolCCQueue = new ConcurrentQueue<GameObject>();
        poolStack = new Stack<GameObject>();
        poolName = objPoolName;
        parentTrans = objTransform;
        
    }
    //创建新实例
    public GameObject NewInstance()
    {
        GameObject gameObject = Instantiate(preFab);
        gameObject.transform.SetParent(parentTrans);
        gameObject.name = preFab.name + Time.time;
        gameObject.SetActive(false);
        //StoreInstance(gameObject);
        return gameObject;

    }
     

    


    public virtual GameObject GetInstance(Vector2 position,float lifetime)
    {
        GameObject gameObject;
        if (lifetime < 0)
        {
            Debug.LogError("lifeTime重置出错！");
            return null;
        }
        //池中有对象，从池中取出对象
        if (poolStack.Count > minCount) 
        {
            
            gameObject = poolStack.Pop();
            if (gameObject == null)
            {
                Debug.LogError("stack中取到了空对象");//触发了，说明是取之前已经为空。但没有空对象入池，说明是入池后、出池前变空的。
            }

        }
        //池中没有对象了，新生成对象
        else
        {
            gameObject = NewInstance();
            Debug.Log("池中没有对象了，新生成对象");
        }
        //初始化新生成的对象
        //设置父对象、坐标，挂载lifetimeCheck组件并初始化lifetime、poolName
        //Debug.Log("设置父对象："+ instanceParentTrans.name);
        if (gameObject == null)
        {
            Debug.LogError("新对象异常：为空");//说明Get方法取到了被摧毁的对象，说明被摧毁的对象仍存在于queue中未成功移除
        }
        gameObject.transform.SetParent(instanceParentTrans);

        gameObject.transform.position = position;
        GameObjectPoolTimeCheck lifetimeCheck = gameObject.GetComponent<GameObjectPoolTimeCheck>();
        if (lifetimeCheck == null)
        {
            lifetimeCheck = gameObject.AddComponent<GameObjectPoolTimeCheck>();
        }
        lifetimeCheck.lifetime = lifetime;
        lifetimeCheck.poolName = poolName;
        //激活
        gameObject.SetActive(true);

        return gameObject;

    }

    //TEST
    private void Update()
    {
        //foreach(GameObject obj in poolQueue)
        
        //foreach(GameObject obj in poolCCQueue)
        
        foreach(GameObject obj in poolStack)
        {
            if(obj == null)
            {
                Debug.LogError("pool中有空对象！");
            }
            
        }
        
    }


    private void StoreInstance(GameObject gameObject)
    {
        /*
        if (gameObject == null)
        {
            Debug.LogError("空对象想要入池！");

        }
        */

        //池子未满，则将对象推入池中
        //if (poolQueue.Count < maxCount)
        //if (poolCCQueue.Count < maxCount)
        if (poolStack.Count < maxCount) 
        {
            //Debug.Log("对象已入池:当前queueCount:"+poolQueue.Count+"maxCount="+maxCount);
            //Debug.Log("对象已入池:当前queueCount:" + poolCCQueue.Count + "maxCount=" + maxCount);
            Debug.Log("对象已入池:当前stackCount:" + poolStack.Count + "maxCount=" + maxCount);
            if (gameObject == null)
            {
                Debug.LogError("空对象入池了");//未检测到,说明没有空对象入池，入池的对象并不是在入池前销毁的
            }
            //禁用、设置父对象、推入池中
            
            gameObject.transform.SetParent(parentTrans);
            //poolQueue.Enqueue(gameObject);
            //poolCCQueue.Enqueue(gameObject);
            gameObject.SetActive(false);



            poolStack.Push(gameObject);
        }
        //池子满了，则将对象直接销毁
        else
        {
            //禁用、销毁对象
            Debug.Log("对象已销毁"+gameObject.name);
            //gameObject.SetActive(false);
            //gameObject.name = "null";
            Destroy(gameObject);//销毁的方法只有这一个，为什么它可以使入池后、出池前的对象变空？是因为此时gameObject是一个入池后出池前的对象引用吗？
        }

    }


    public void ReturnInstance(GameObject gameObject)
    {
        //check
        //initObject
        StoreInstance(gameObject);
    }
     
    public virtual void Destroy()
    {
        //poolQueue.Clear();
        /*
        GameObject obj;
        
        while(poolCCQueue.TryDequeue(out obj))
        {
            //
        }
        */
        poolStack.Clear();
    }
}
