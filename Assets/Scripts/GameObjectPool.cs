using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    //公共字段
    private string poolName;
    public int maxCount = 30;
    public int minCount = 3;
    public Queue<GameObject> poolQueue;
    public GameObject preFab;

    public Transform instanceParentTrans;

    private Transform parentTrans;

    GameObjectPoolManager manager;

    //属性
    //private Transform PoolTrans { get { return transform; } }

    


    //由manager调用
    public virtual void InitPool(string objPoolName,Transform objTransform)
    {
        manager = GameObjectPoolManager.instance;
        instanceParentTrans = manager.instanceContent.transform;

        poolQueue = new Queue<GameObject>();
        poolName = objPoolName;
        parentTrans = objTransform;
        //初始化最低限度的对象，避免频繁生成时触底
        for(int i = 0; i < minCount; i++)
        {
            NewInstance();
        }
    }
    //创建新实例
    public GameObject NewInstance()
    {
        GameObject gameObject = Instantiate(preFab);
        gameObject.transform.SetParent(parentTrans);
        gameObject.name = preFab.name + Time.time;
        StoreInstance(gameObject);
        return gameObject;

    }
     

    


    public GameObject GetInstance(Vector2 position,float lifetime)
    {
        GameObject gameObject;
        if (lifetime < 0)
        {
            return null;
        }
        //池中有对象，从池中取出对象
        if (poolQueue.Count > minCount)
        {
            gameObject = poolQueue.Dequeue();
        }
        //池中没有对象了，新生成对象
        else
        {
            gameObject = NewInstance();
        }
        //初始化新生成的对象
        //设置父对象、坐标，挂载lifetimeCheck组件并初始化lifetime、poolName
        gameObject.transform.SetParent(instanceParentTrans);
        gameObject.transform.position = position;
        GameObjectPoolTimeCheck lifetimeCheck = gameObject.AddComponent<GameObjectPoolTimeCheck>();
        lifetimeCheck.lifetime = lifetime;
        lifetimeCheck.poolName = poolName;
        //激活
        gameObject.SetActive(true);

        return null;
    }
    private void StoreInstance(GameObject gameObject)
    {
        //池子未满，则将对象推入池中
        if (poolQueue.Count < maxCount)
        {
            //禁用、设置父对象、推入池中
            gameObject.SetActive(false);
            gameObject.transform.SetParent(parentTrans);
            poolQueue.Enqueue(gameObject);
        }
        //池子满了，则将对象直接销毁
        else
        {
            //禁用、销毁对象
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }


    public void ReturnInstance(GameObject gameObject)
    {
        //check
        //initObject
        StoreInstance(gameObject);
    }
     
    public void Destroy()
    {

    }
}
