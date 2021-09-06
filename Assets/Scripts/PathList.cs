using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//[ExecuteInEditMode]
public class PathList : MonoBehaviour
{
    static int id;
    public int NodsNum = 2;
    private List<PathNods> nods;
    private void Awake()
    {
        id++;
    }

    //[MenuItem("MenuButton/FunA")]
    public void CreatNods()
    {
        nods = new List<PathNods>(NodsNum);
        for(int i = 0; i < NodsNum; i++)
        {
            GameObject nodObject = new GameObject("Nod_"+i);
            nodObject.transform.parent = gameObject.transform;
            PathNods nod = nodObject.AddComponent<PathNods>();
            nods.Add(nod);

            
        }
        for(int i = 0; i < NodsNum; i++)
        {
            if (i != NodsNum - 1)
            {
                nods[i].nextNods = nods[i + 1].gameObject;
            }
        }
        
    }
    
    public void Add()
    {
        int i = nods.Count;
        GameObject nodObject = new GameObject("Nod_" + i);
        nodObject.transform.parent = gameObject.transform;
        PathNods nod = nodObject.AddComponent<PathNods>();
        nods.Add(nod);
        nods[i - 1].nextNods = nod.gameObject;
    }

    public void Clear()
    {
        float childCount = transform.childCount;
         
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

    }

}
