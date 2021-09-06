using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class EnemySetting : ScriptableObject
{
    public float _hearing = 0.03f;
    public float _vision = 5f;
    public float _fleeVision = 10f;
    public float _maxHp = 10f;
    public float _moveSpeed = 0.5f;
    public float _angleSpeed = 10f;
    public float _hearingReduceSpeed = 0.01f;
}
