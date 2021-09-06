using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{
    public ParticleSystem breath;
    public void breathPlay()
    {
        breath.Play();
    }
}
