using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreater : MonoBehaviour
{
    LineRenderer lineRenderer;

    public GameObject target;


    [SerializeField]
    private Texture2D[] textures;


 
 
 
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    private void Update()
    {
        lineRenderer.SetPosition(1, target.transform.position);

 
    }


}
