using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface ITest
{

}

public class G1 : ITest
{

}
public class G2 : ITest
{

}






public class TestSample : MonoBehaviour
{
    void test22(ISmellComponent smellComponent,IPlayer player) 
    {
        smellComponent.SmellPlayer(player);
    }

    private void Awake()
    {
        //ISmellComponent smellComponent = new SmellComponent();
        //SmellComponent smellComponent = new SmellComponent();
        ISmellComponent smellComponent = gameObject.AddComponent<SmellComponent>();

        ISmellComponent ss = gameObject.GetComponent<TestSample2>();

        ZombieEnemy zombieEnemy = new ZombieEnemy();
        zombieEnemy.SetDependance(smellComponent);

        //IPlayer player = new mPlayer();
        mPlayer player = new mPlayer();

        zombieEnemy.SmellPlayer(player);

        test22(ss, player);

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


public class SmellComponent : MonoBehaviour,ISmellComponent
//public class SmellComponent : ISmellComponent

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







