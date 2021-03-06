﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataScript : MonoBehaviour {

    [SerializeField]
    public int PlayerHpNowData;
    [SerializeField]
    public int FloorCount;
    //public int PlayerHpNowData { get; private set; }
    //public int FloorCount { get; private set; }
    [SerializeField]
    bool SaveFlg;

    public void SaveFloorCount()
    {
        FloorCount += 1;
    }
    public void SetFlgOn()
    {
        SaveFlg = true;
    }
    //public void SetFlgOff()
    //{
    //    SaveFlg = false;
    //}
    public bool GetFlg()
    {
        return SaveFlg;
    }
    public void ClearData()
    {
        SaveFlg = false;
        PlayerHpNowData = 0;
        FloorCount = 1;
    }

    public void SavePlayerHpNowData(int NowHP)
    {
        PlayerHpNowData = NowHP;
    }

    // Use this for initialization
    void Awake()
    {
        SaveFlg = false;
        PlayerHpNowData = 0;
        FloorCount = 1;
    }
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
