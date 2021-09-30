using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMoveComponent
{
    void MoveTo(Vector3 target);

}

[CreateAssetMenu]
public class MoveSetting : ScriptableObject
{
    public float patrolVision;
}


public class MoveComponent : IMoveComponent
{
    ISeeComponent seeComponent;
    Vector3 thePatrolTarget;

    public MoveComponent(MoveSetting moveSetting)
    {

    }

    

    public void MoveTo(Vector3 target)
    {

    }

    public void Move()
    {

    }

    public void Stop()
    {

    }

    public void Ctor()
    {

    }
}
