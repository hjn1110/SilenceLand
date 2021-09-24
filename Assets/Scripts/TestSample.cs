using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSample : MonoBehaviour
{

    private void Awake()
    {
        //ISmellComponent smellComponent = new SmellComponent();
        SmellComponent smellComponent = new SmellComponent();

        ZombieEnemy zombieEnemy = new ZombieEnemy();
        zombieEnemy.SetDependance(smellComponent);

        //IPlayer player = new mPlayer();
        mPlayer player = new mPlayer();

        zombieEnemy.SmellPlayer(player);

    }



}


public interface ISmellComponent
{
    void SmellPlayer(IPlayer player);
}

public interface IPlayer
{

}

public class mPlayer : IPlayer
{
    public void Ctor()
    {
        UnityEngine.Debug.Log("初始化");
    }
}


public class SmellComponent:ISmellComponent
{
    public void SmellPlayer(IPlayer player)
    {
        UnityEngine.Debug.Log("找到玩家");
    }

}

public class ZombieEnemy:ISmellComponent
{
    ISmellComponent smellComponent;
    public ZombieEnemy() { }

    public void SetDependance(ISmellComponent smellComponent)
    {
        this.smellComponent = smellComponent;
    }


    public void SmellPlayer(IPlayer player)
    {
        smellComponent.SmellPlayer(player);
    }
}







