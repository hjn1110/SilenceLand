using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveComponent
{
    void MoveTo(Vector3 target);
    void Move();
    void Stop();


}
