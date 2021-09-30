using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IFollowCompenent
{
    void Follow(Transform target);
}

public class FollowComponent:IFollowCompenent
{


    public FollowComponent(IMoveComponent moveComponent)
    {
        this.moveComponent = moveComponent;
    }

    IMoveComponent moveComponent;
   

    public void Follow(Transform target)
    {
        moveComponent.MoveTo(target.position);
    }

}
