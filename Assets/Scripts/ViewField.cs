using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("碰撞发生，碰撞者：" + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            //Debug.Log("准备传递消息："+ collision.transform);
            SendMessageUpwards("AddViewTarget", collision.transform);
            Debug.Log("注册监视"+collision.gameObject.name);
            //向父类中的Enemies注册targetTrans
        }
    }
 
}
