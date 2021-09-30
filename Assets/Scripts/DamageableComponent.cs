using System.Collections;
using System.Collections.Generic;
using UnityEngine;




interface IDamageableComponent
{
    float Hp { get; set; }
    float MaxHp { get; set; }
}




public class DamageableComponent : IDamageableComponent
{

    public float Hp { get; set; }
    public float MaxHp { get; set; }



}

[CreateAssetMenu]
public class DamageSetting : ScriptableObject
{
    public float Hp;
    public float MaxHp;



}