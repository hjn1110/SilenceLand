using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollowState : FSMState
{

    public ZombieFollowState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Follow;
       




    }

    public override void Act(GameObject enemy)
    {
        enemy.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
        enemy.GetComponent<Zombie>().Follow();

    }

    public override void Reason(GameObject enemy)
    {
        
        //未见则巡
        if ((enemy.GetComponent<Zombie>().LostPlayer))
        {
            //Debug.Log("丢弃声源");
            enemy.GetComponent<Zombie>().BackToPatrol();
            fsm.PerformTransition(Transition.LostPlayer);
        }

    }
}
