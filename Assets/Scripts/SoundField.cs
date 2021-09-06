using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundField : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("准备传递消息："+ collision.transform);
            SendMessageUpwards("AddEnemyInTrigger", collision.transform);
            Debug.Log("注册监听者:"+collision.gameObject.name);
            //向父类中的SoundSpread注册enemyTrans
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("准备传递消息："+ collision.transform);
            SendMessageUpwards("RemoveEnemyInTrigger", collision.transform);
            Debug.Log("移除监听者:" + collision.gameObject.name);
            //向父类中的SoundSpread移除enemyTrans
        }
    }


}
