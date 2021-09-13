using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSpreadManager : MonoBehaviour
{
    #region Singleton
    public static SoundSpreadManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    GameObjectPoolManager poolManager;
    
    private void Start()
    {
        
        poolManager = GameObjectPoolManager.instance;
        poolManager.CreatPool<SoundSpreadPool>("SoundPool");
    }

    public void MakeASound(Vector3 pos)
    {
        Vector2 _pos = pos;
        SoundSpread aSound = poolManager.GetInstance("SoundPool", _pos, 5).GetComponent<SoundSpread>();
        aSound.MakeASound();

    }






}
