using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PatrolField : MonoBehaviour
{
    Component parentComponent;
    IPatrolComponentEditor patrolComponentEditor;

     

    public void Ctor(IPatrolComponentEditor patrolComponent)
    {
        this.patrolComponentEditor = patrolComponent;

       
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PathNod"))
        {
             
            gameObject.GetComponentsInParent<Enemies>()[0].NodsInView.AddLast(collision.gameObject.GetComponent<PathNods>());
            //patrolComponentEditor.RefreshNodsCache(collision.gameObject.GetComponent<PathNods>());



        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PathNod"))
        {
            gameObject.GetComponentsInParent<Enemies>()[0].NodsInView.Remove(collision.gameObject.GetComponent<PathNods>());
            gameObject.GetComponentsInParent<Enemies>()[0].theLastNodInView = collision.gameObject.GetComponent<PathNods>();

            //patrolComponentEditor.ClearNodsCache(collision.gameObject.GetComponent<PathNods>());
            //patrolComponentEditor.SetLastNod(collision.gameObject.GetComponent<PathNods>());

        }
    }
}
