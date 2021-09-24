using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NavMeshAgentSetting : ScriptableObject
{
    public int agentTypeID = 0;
    public float baseOffset = 0.1f;
    public float moveSpeed = 0.5f;
    public float angleSpeed = 10f;
    public float acceleration = 2f;
    public float stoppingDistance = 0.1f;
    public bool autoBraking = true;
    public float radius = 0.2f;
    public float height = 0.2f;
}
