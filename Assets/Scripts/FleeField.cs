using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeField : MonoBehaviour
{
    ISeeComponentEditor seeComponentEditor;

    public void Ctor(SeeComponent seeComponent)
    {
        this.seeComponentEditor = seeComponent;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("准备传递消息："+ collision.transform);
            //SendMessageUpwards("RemoveViewTarget", collision.transform);
            //Debug.Log("移除监视" + collision.gameObject.name);
            //向父类中的Enemies移除targetTrans

            seeComponentEditor.RemoveViewTarget(collision.transform);

        }
    }
}
