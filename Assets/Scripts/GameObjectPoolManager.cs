using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolManager : MonoBehaviour
{

    #region Singleton
    public static GameObjectPoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private Dictionary<string, GameObjectPool> poolDic;

    [HideInInspector]
    public GameObject poolContent;
    [HideInInspector]
    public GameObject instanceContent;

    private void Start()
    {
        initPoolManager();
    }

    private void initPoolManager()
    {
        poolDic = new Dictionary<string, GameObjectPool>();
        poolContent = new GameObject("Pool");
        instanceContent = new GameObject("Instance");
    }

    //创建Pool，Pool必须以GameObjectPool为基类
    public T CreatPool<T>(string poolName) where T : GameObjectPool, new()
    {
        //若已存在，则返回该pool
        if (poolDic.ContainsKey(poolName))
        {
            return (T)poolDic[poolName];
        }
        //若不存在，则创建pool
        GameObject obj = new GameObject(poolName);
        //初始化pool
        //设置父对象、
        obj.transform.SetParent(transform);
        T pool = new T();
        pool.InitPool(poolName, obj.transform);
        poolDic.Add(poolName,pool);
        return pool;
    }

    public GameObject GetInstance(string poolName, Vector2 position, float lifetime)
    {
        //校验pool是否存在，若存在则从其中取出对象
        if (poolDic.ContainsKey(poolName))
        {
            return poolDic[poolName].GetInstance(position, lifetime);
        }
        return null;
    }


    public void ReturnInstance(string poolName, GameObject gameObject)
    {
        //校验pool是否存在，若存在则向其中推入对象
        if (poolDic.ContainsKey(poolName))
        {
            poolDic[poolName].ReturnInstance(gameObject);
        }

    }

    void Destory()
    {
        poolDic.Clear();
        Destroy(gameObject);
    }





}
