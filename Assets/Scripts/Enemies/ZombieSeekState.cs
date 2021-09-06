using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSeekState : FSMState
{
    //private Transform target;
    

    public ZombieSeekState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Seek;
       




    }

    public override void Act(GameObject enemy)
    {
        enemy.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        enemy.GetComponent<Zombie>().Seek();

    }

    public override void Reason(GameObject enemy)
    {
        //凡见则追
        if (enemy.GetComponent<Zombie>().SeePlayer)
        {
            fsm.PerformTransition(Transition.SeePlayer);
        }
       
        //未闻未见则巡
        if ((!enemy.GetComponent<Zombie>().SeePlayer) && (!enemy.GetComponent<Zombie>().hearAnything))
        {
            //Debug.Log("丢弃声源");
            fsm.PerformTransition(Transition.LostHear);
        }

    }
}
