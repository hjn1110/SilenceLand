using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemies
{
    

    //指定当前Enemy类型的参数配置
    protected override EnemySetting setting { get { return globalSetting.zombieSetting; } }

    /*
    IMoveComponent moveComponent;
    public void SetDependance(IMoveComponent moveComponent)
    {
        this.moveComponent = moveComponent;
    }
    */

    //注册状态机
    protected override void initFSM()
    {
        //Debug.Log("Zombie初始化状态机" + gameObject.name);
        fsm = new FSMSystem();

        //Pend的情况下
        FSMState pendState = new ZombiePendState(fsm);
        //SeePlayers后，进入Follow状态
        pendState.ADDTransition(Transition.SeePlayer, StateID.Follow);
        //Hear后，进入Seek状态
        pendState.ADDTransition(Transition.Hear, StateID.Seek);
        //待时：凡见则追，仅闻未见则寻

        //Seek的情况下
        FSMState seekState = new ZombieSeekState(fsm);
        //SeePlayer后，进入Follow状态
        seekState.ADDTransition(Transition.SeePlayer,StateID.Follow);
        //LostHear后，进入Patrol状态
        seekState.ADDTransition(Transition.LostHear, StateID.Patrol);
        //寻时：凡见则追，未闻未见则巡


        //Follow的情况下
        FSMState followState = new ZombieFollowState(fsm);
        //LostPlayer的情况下，进入Patrol状态
        followState.ADDTransition(Transition.LostPlayer,StateID.Patrol);
        //追时：未闻未见则巡

        //Patrol的情况下
        FSMState patrolState = new ZombiePatrolState(fsm);
        //SeePlayer后，进入Follow状态
        patrolState.ADDTransition(Transition.SeePlayer, StateID.Follow);
        //Hear后，进入Seek状态
        patrolState.ADDTransition(Transition.Hear, StateID.Seek);
        //巡时：凡见则追，仅闻未见则寻

        //注册状态
        fsm.AddState(pendState);
        fsm.AddState(seekState);
        fsm.AddState(followState);
        fsm.AddState(patrolState);

        if(defaultState == StateID.Patrol)
        {
            fsm.currentFSMState = patrolState;
        }
        if (defaultState == StateID.Pend)
        {
            fsm.currentFSMState = pendState;
        }
    }


    //挂载刚体
    protected override void AddRigid()
    {
        TriggerCreater.instance.AddRidWithTemplate1(gameObject);
    }






}
