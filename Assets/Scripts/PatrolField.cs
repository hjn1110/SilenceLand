using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PatrolField : MonoBehaviour
{
    Component parentComponent;
    PatrolComponent patrolComponent;

     

    public void Ctor(PatrolComponent patrolComponent)
    {
        this.patrolComponent = patrolComponent;

       
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PathNod"))
        {
             
            gameObject.GetComponentsInParent<Enemies>()[0].NodsInView.AddLast(collision.gameObject.GetComponent<PathNods>());
            //patrolComponent.RefreshNodsCache(collision.gameObject.GetComponent<PathNods>());



        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PathNod"))
        {
            gameObject.GetComponentsInParent<Enemies>()[0].NodsInView.Remove(collision.gameObject.GetComponent<PathNods>());
            gameObject.GetComponentsInParent<Enemies>()[0].theLastNodInView = collision.gameObject.GetComponent<PathNods>();

            //patrolComponent.ClearNodsCache(collision.gameObject.GetComponent<PathNods>());
            //patrolComponent.SetLastNod(collision.gameObject.GetComponent<PathNods>());

        }
    }
}
