using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnMode
{
    FollowTag,
    Random,
}

public class PathNods : MonoBehaviour
{
    public GameObject nextNods;
    public int pathTag;
    public TurnMode turnMode;
    public float delay;
    private TriggerCreater triggerCreater;


    private void OnDrawGizmos()
    {
        if (nextNods != null)
        {
            Gizmos.DrawLine(gameObject.transform.position, nextNods.transform.position);

        }
    }
    private void Start()
    {
        //Debug.Log("创建Trigger");
        gameObject.tag = "PathNod";
        triggerCreater = TriggerCreater.instance;
        triggerCreater.AddTriggerComponent(gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemies>().SetNextTarget(this);
        }
    }
     


}
