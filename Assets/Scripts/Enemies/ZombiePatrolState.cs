using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePatrolState : FSMState
{

    public ZombiePatrolState(FSMSystem fsm) : base(fsm)
    {
        stateID = StateID.Patrol;
      




    }

    public override void Act(GameObject enemy)
    {
        enemy.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        enemy.GetComponent<Zombie>().Patrol();
        if (enemy.GetComponent<Zombie>().OnBlocked())
        {
            Debug.Log("堵住了");
        }
    }

    public override void Reason(GameObject enemy)
    {
        //凡见则追
        if (enemy.GetComponent<Zombie>().SeePlayer)
        {
            fsm.PerformTransition(Transition.SeePlayer);
        }

        //仅闻未见则寻
        if ((!enemy.GetComponent<Zombie>().SeePlayer) && (enemy.GetComponent<Zombie>().hearAnything))
        {
            fsm.PerformTransition(Transition.Hear);
        }

    }
}
