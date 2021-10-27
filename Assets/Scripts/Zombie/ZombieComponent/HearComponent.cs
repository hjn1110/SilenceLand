using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public interface IHearComponent
{
    bool Hear(out Vector3 target);
    bool Hear();
    Dictionary<Vector3, float> SoundSourceList { get; set; }//存储听到及记得的声源信息，包括坐标和音量记忆

    float Hearing { get; set; }//听力
    float HearingReduceSpeed { get; }//听力记忆衰减速度

    void HearWeaken();

    Vector3 HearTarget { get; }

    void AddSoundSourceCache(Vector3 soundSourcePos, float soundVolume);
    void HearingDelayClear(Vector2 target);
    
}

[CreateAssetMenu]
public class HearSetting : ScriptableObject
{
    public float hearing = 0.03f;
    public float hearingReduceSpeed = 0.01f;
    //public float hearingReduceSpeedLevel2 = 0.02f;
    public float hearingDelayClearTime = 2f;

}


public class HearComponent:IHearComponent
{
    public HearComponent(HearSetting hearSetting)
    {
        SoundSourceList = new Dictionary<Vector3, float>();
        this.hearSetting = hearSetting;

    }

    HearSetting hearSetting;

    //IAIMoveComponent aIMoveComponent;
    //IContainer container;

    //private Timer timer;

    private void Awake()
    {
        //container = new Container();
        //Timer.Cancel(timer);
    }

    void Inject()
    {
        //container.SetInstance<HearComponent>(hearComponent);
    }




    public void Ctor()
    {

    }


    public Vector3 HearTarget
    {
        get
        {
            Vector3 target;
            Hear(out target);
            return target;
        }
    }


    //--------------------------------------------------------------------------
    //IHearComponent依赖

    //字段
    public float Hearing { get; set; }//听力
    public float HearingReduceSpeed { get { return hearSetting.hearingReduceSpeed;  } }//听力记忆衰减速度
    public Dictionary<Vector3, float> SoundSourceList { get; set; }//存储听到及记得的声源信息，包括坐标和音量记忆


    //听觉方法
    //传出听觉对象：SoundSourceList中音量记忆最强的一个；若SoundSourceList为空，则返回false
    public bool Hear(out Vector3 target)
    {
        if (Hear())
        {
            target = TargetFilter(SoundSourceList);
            return true;
        }
        else
        {
            target = Vector3.negativeInfinity;
            return false;
        }
    }
    public bool Hear()
    {
        if ((SoundSourceList != null) && (SoundSourceList.Count != 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //--------------------------------------------------------------------------
    //私有方法

    //音量记忆衰减函数
    public void HearWeaken()
    {
        List<Vector3> soundPos = new List<Vector3>(SoundSourceList.Keys);
        if (Hear())
        {
            for (int i = 0; i < soundPos.Count; i++)
            {
                if (SoundSourceList[soundPos[i]] > 0)
                {
                    SoundSourceList[soundPos[i]] -= HearingReduceSpeed;
                    //Debug.Log("音量记忆衰减:" + soundSourceList[soundPos[i]]);
                }
                else
                {
                    SoundSourceList.Remove(soundPos[i]);
                    //HearingDelayClear(soundPos[i]);
                    //Debug.Log("移除一个声源");
                }
            }
        }
    }



    //对音量记忆list排序，选择音量最大的作为搜寻对象
    private Vector3 TargetFilter(Dictionary<Vector3, float> SoundSourceList)
    {
        float volume = 0;
        Vector3 target = Vector3.zero;
        foreach (Vector3 pos in SoundSourceList.Keys)
        {
            if (volume < SoundSourceList[pos])
            {
                volume = SoundSourceList[pos];
                target = pos;
            }
        }
        return target;
    }




    //--------------------------------------------------------------------------
    //SoundSpreadComponent依赖
    //声音每传导一次，将自己添加进范围覆盖到的enemy的SoundSourceList
    //考虑改写进SoundSpreadComponent中，而非在HearComponent中
    public void AddSoundSourceCache(Vector3 soundSourcePos, float soundVolume)
    {
        if (soundVolume >= Hearing)
        {
            //如果该声音目标未添加，则添加该目标
            if (!SoundSourceList.ContainsKey(soundSourcePos))
            {
                SoundSourceList.Add(soundSourcePos, soundVolume);
            }
            //如果该声音目标已添加，则更新该目标的声音信号
            else
            {
                SoundSourceList[soundSourcePos] = soundVolume;
            }
        }
    }

    //抵达目标后，延时清除该点音量记忆
    public void HearingDelayClear(Vector2 target)
    {
        Debug.Log("准备清除记忆");
        //Timer.Cancel(timer);
        //timer = Timer.Register(hearSetting.hearingDelayClearTime, onComplete: () =>clear(target), isLooped: false,useRealTime:true);
        clear(target);
    }

    private void clear(Vector2 target)
    {
        SoundSourceList.Remove(target);
        Debug.Log("已清除记忆");


    }



}
