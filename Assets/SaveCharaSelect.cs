using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCharaSelect : MonoBehaviour
{
    //シングルトン化
    private static SaveCharaSelect mInstance;
    public static SaveCharaSelect Instance
    {
        get
        {
            return mInstance;
        }
    }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        mInstance = this;
    }
    public PlayerSelector.PlayerKind CharaNumber;
}
