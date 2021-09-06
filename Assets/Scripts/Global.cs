using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Silent.GameSystem.SearchObject;




public class Global : MonoBehaviour
{
    public Tilemap tileReal;
    public Grid grid;
    public GameObject player;

    public SearchGameObject sgo;

    #region Singleton
    public static Global instance;
     

    private void Awake()
    {
        instance = this;
        sgo = gameObject.AddComponent<SearchGameObject>();

    }
    #endregion

    private void Start()
    {
        GameObject[] objects;
        bool ifHas=  sgo.Search(1, 1, out objects);
        Debug.Log("指定坐标是否存在对象：" + ifHas);
    }


}
