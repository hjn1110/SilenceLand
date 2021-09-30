using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem 
{
    private Dictionary<StateID, FSMState> states = new Dictionary<StateID, FSMState>();
    private StateID currentStateID;
    public FSMState currentFSMState;



    public void Update(GameObject npc)
    {
        currentFSMState.Act(npc);
        currentFSMState.Reason(npc);
    }
    


    public void AddState(FSMState s)
    {
        if (s == null)
        {
            Debug.LogError("FSMState不能为空");
            return;
        }

        if (currentFSMState == null)
        {
            currentFSMState = s;
            currentStateID = s.ID;
        }
        if (states.ContainsKey(s.ID))
        {
            Debug.LogError(s.ID + "已经存在,无法重复添加");
            return;
        }
        states.Add(s.ID, s);
        //Debug.Log(s.ID+"添加完成");
    }
    public void DeleteState(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("无法删除控的状态");
            return;
        }
        if (!states.ContainsKey(id))
        {
            Debug.LogError("无法删除不存在的状态");
        }
        states.Remove(id);

    }

    public void PerformTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("无法执行空的转换条件");
            return;

        }
        StateID id = currentFSMState.GetStateID(transition);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("当前的状态" + id + "无法转换" + transition);
            return;
        }
        if (!states.ContainsKey(id))
        {
            Debug.LogError("状态机中不包含这个状态" + id);
            return;
        }
        FSMState state = states[id];
        currentFSMState.DoAfterLeaving();
        currentFSMState = state;
        currentStateID = state.ID;
        currentFSMState.DoBeforeEntering();



    }
}
