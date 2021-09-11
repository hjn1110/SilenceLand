using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolTimeCheck : MonoBehaviour
{
    //[HideInInspector]
    public float lifetime = 0;
    [HideInInspector] public string poolName;

    private GameObjectPoolManager manager;

    private WaitForSeconds waitTime;

    void Awake()
    {
        if (lifetime > 0)
        {
            waitTime = new WaitForSeconds(lifetime);
        }
    }
    private void Start()
    {
        manager = GameObjectPoolManager.instance;
    }

    //每次激活后重新计时
    void OnEnable()
    {
        //确认无第三方引用时，执行以下逻辑。确认方法待补充
        if (lifetime > 0)
        {
            StopAllCoroutines();
            StartCoroutine(CountDown(lifetime));
        }
    }

    IEnumerator CountDown(float lifetime)
    {
        //yield return waitTime;
        yield return new WaitForSeconds(lifetime);
        //将对象加入对象池

        if (gameObject == null)
        {
            Debug.LogError("Check入池！对象为空");
        }
        else
        {
            Debug.Log("Check入池成功"+gameObject.name);
        }

        manager.ReturnInstance(poolName, gameObject);
    }

}
