using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IAIMoveComponent
{
    void MoveTo(Vector3 target);
    void Move();
    void Stop();
    void Ctor(NavMeshAgent agent);


}
