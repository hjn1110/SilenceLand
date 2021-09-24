﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveComponent : MonoBehaviour, IAIMoveComponent
{
    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void Move()
    {
        agent.isStopped = false;



    }

    public NavMeshAgent agent { get; private set; }
    public NavMeshAgentSetting agentSetting;
     

    public void Ctor(NavMeshAgent agent)
    {
        this.agent = agent;

        agent.agentTypeID = agentSetting.agentTypeID;
        agent.baseOffset = agentSetting.baseOffset;
        agent.speed = agentSetting.moveSpeed;
        agent.acceleration = agentSetting.acceleration;
        agent.stoppingDistance = agentSetting.stoppingDistance;
        agent.autoBraking = agentSetting.autoBraking;
        agent.radius = agentSetting.radius;
        agent.height = agentSetting.height;

        //下面两行是使用重写的转向方法，弥补了navMesh自身的转向方法的失效
        Rotate_Smoothly rotateSmooth = agent.gameObject.AddComponent<Rotate_Smoothly>();
        rotateSmooth.rotateSpeed = agentSetting.angleSpeed;
    }

    
    

    public void Stop()
    {
        agent.isStopped = true;
    }
     
}
