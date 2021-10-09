﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Silent.MapObject.SearchObject;

/*
public class ZombieGo : MonoBehaviour
{
    IContainer container;

    private void Awake()
    {
        container = new Container();

    }


    private void Inject()
    {
        ZombieEntity zombieEntity = new ZombieEntity();
        container.SetInstance<ZombieEntity>(zombieEntity);
    }


    

    protected FSMSystem fsm;
    //指定默认状态
    public StateID defaultState = StateID.Pend;
    public StateID currentState;


    //注册状态机
    private void initFSM()
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
        seekState.ADDTransition(Transition.SeePlayer, StateID.Follow);
        //LostHear后，进入Patrol状态
        seekState.ADDTransition(Transition.LostHear, StateID.Patrol);
        //寻时：凡见则追，未闻未见则巡


        //Follow的情况下
        FSMState followState = new ZombieFollowState(fsm);
        //LostPlayer的情况下，进入Patrol状态
        followState.ADDTransition(Transition.LostPlayer, StateID.Patrol);
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

        if (defaultState == StateID.Patrol)
        {
            fsm.currentFSMState = patrolState;
        }
        if (defaultState == StateID.Pend)
        {
            fsm.currentFSMState = pendState;
        }
    }



    private void Update()
    {
        if (Time.frameCount % 10 == 0)//10帧检查一次状态。FPS=30时约1秒检查3次，60时则6次
        {
            //状态机更新
            fsm.Update(gameObject);
            currentState = fsm.currentFSMState.ID;

            //print(gameObject.name+"当前的状态是：" + fsm.currentFSMState);
            //print("seekTarget="+theSeekTarget);
            //音量记忆衰减
            HearingReduce();
            //根据音量记忆排序，确定搜寻对象
            seekTarget();

            selectTargetFormMinDis();
        }
    }






}
*/