using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class SoundSpreadVisualization : MonoBehaviour
{
    public Tilemap tileHearIndeed;
    public Tilemap tileHearBase;
    //public Tilemap tileReal;
    public TileBase tilebase0;
    public TileBase tilebase1;
    //public Grid grid;


    #region Singleton
    public static SoundSpreadVisualization instance;

    private void Awake()
    {
        instance = this;

    }
    #endregion


}
