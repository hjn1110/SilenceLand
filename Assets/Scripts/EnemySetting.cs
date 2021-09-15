using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class EnemySetting : ScriptableObject
{
    public float hearing = 0.03f;
    public int vision = 5;
    public int fleeVision = 10;
    public int patrolVision = 10;
    public float maxHp = 10f;
    public float moveSpeed = 0.5f;
    public float angleSpeed = 10f;
    public float hearingReduceSpeed = 0.01f;
    public float hearingReduceSpeedLevel2 = 0.02f;
    public float hearingDelayClearTime = 2f;
}
