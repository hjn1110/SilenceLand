using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSample2 : MonoBehaviour, ISmellComponent
{
    public void SmellPlayer(IPlayer player)
    {
        Debug.Log("SSS!!");
    }

}
