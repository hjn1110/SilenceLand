using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieEntity : MonoBehaviour
{
    
    //数据层(ScritableObject)
    public MoveSetting moveSetting;
    public NavMeshAgentSetting agentSetting;
    public SeeSetting seeSetting;
    public HearSetting hearSetting;
    public PatrolSetting patrolSetting;
    public ZombieFSMSetting fsmSetting;


    //行为层(Component)
    //Move组件
    //IMoveComponent moveComponent;
    //AIMove组件
    //IAIMoveComponent aIMoveComponent;
    IMoveComponent AIMoveComponent;
    //See组件
    ISeeComponent seeComponent;
    //Hear组件
    IHearComponent hearComponent;
    //Patrol组件
    IPatrolComponent patrolComponent;
    //Follow组件
    IFollowCompenent followComponent;
    //Seek组件
    ISeekComponent seekComponent;

    //逻辑层(FSM)
    //状态机
    IZombieFSM zombieFSM;


    /// <summary>
    /// 创建组件、依赖注入
    /// </summary>
    private void Start()
    {
        //零级行为层：触发器
        seeComponent = new SeeComponent(seeSetting, transform);
        hearComponent = new HearComponent(hearSetting);

        //一级行为层
        AIMoveComponent = new AIMoveComponent(agentSetting);
        //moveComponent = new MoveComponent(moveSetting);

        //二级行为层：行为组合，依赖并调用一级行为层
        followComponent = new FollowComponent(AIMoveComponent);
        seekComponent = new SeekComponent(AIMoveComponent);
        patrolComponent = new PatrolComponent(patrolSetting,AIMoveComponent, transform);

        //逻辑层：行为组织,二级行为层间的转换机制
        zombieFSM = new ZombieFSM(this, fsmSetting);

    }


    /// <summary>
    /// 行为层实现
    /// </summary>
    public void Follow()
    {
        if (seeComponent.SeeTarget() != null)
        {
            followComponent.Follow(seeComponent.SeeTarget());
        }
    }
    public void Seek()
    {
        if (hearComponent.Hear())
        {
            seekComponent.Seek(hearComponent.HearTarget);
        }
    }
    public void Patrol()
    {
        patrolComponent.Patrol();
    }
    public void ReturnToPatrol()
    {
        patrolComponent.Return();
    }


    /// <summary>
    /// 逻辑层主循环
    /// </summary>
    public void Update()
    {
        //状态机更新
        zombieFSM.Update(gameObject);

        //音量记忆衰减
        hearComponent.HearWeaken();

        //Debug
        //print(gameObject.name+"当前的状态是：" + fsm.currentFSMState);
        //print("seekTarget="+theSeekTarget);

    }


}
