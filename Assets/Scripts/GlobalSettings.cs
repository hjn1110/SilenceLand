using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public EnemySetting zombieSetting;
    public PlayerSetting playerSetting;

    #region Singleton
    public static GlobalSettings instance;

    private void Awake()
    {
        instance = this;

    }
    #endregion
}
