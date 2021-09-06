using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnMode
{
    Loop,
    Random,
}

public class PathNods : MonoBehaviour
{
    public GameObject nextNods;
    public int pathTag;
    public TurnMode turnMode;

    //private void OnDrawGizmosSelected()
    private void OnDrawGizmos()
    {
        if (nextNods != null)
        {
            Gizmos.DrawLine(gameObject.transform.position, nextNods.transform.position);

        }
    }




        
}
