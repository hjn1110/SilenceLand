using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 基本属性
///-精准度
///-是否蓄力
///-攻击范围：点
///- 有短指示线
///- 射程近、中
/// - 杀伤力小
/// - 上弹速度快
/// - 弹夹大
/// - 声音中
/// </summary>
///

public enum AimingWay
{

}

public class Gun : Weapon
{
    //攻击力

    //精准度
    public virtual float Accuracy { get; set; }

    //是否蓄力
    public virtual bool Accumulate { get; set; }

    //伤害半径
    public virtual float DamageRadius { get; set; }

    //具备瞄准镜
    public virtual bool HasAimscope { get; set; }

    //瞄准距离(范围)
    public virtual float AimDistanceMin { get; set; }
    public virtual float AimDistanceMax { get; set; }

    //射击距离
    public virtual float DamageDistanceMax { get; set; }

    //填弹速度
    public float FillingSpeed { get; set; }

    //弹夹容量
    public float MagazineCapacity { get; set; }

    








}
