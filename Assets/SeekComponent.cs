using System.Collections;
using System.Collections.Generic;
using UnityEngine;




interface ISeekComponent
{
    void Seek(Vector3 target);
}
public class SeekComponent : ISeekComponent
{

    public SeekComponent(IMoveComponent moveComponent)
    {
        this.moveComponent = moveComponent;
    }

    IMoveComponent moveComponent;

    public void Seek(Vector3 target)
    {
        moveComponent.MoveTo(target);
    }

}
