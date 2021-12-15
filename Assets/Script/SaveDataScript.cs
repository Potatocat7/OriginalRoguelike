﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]

public struct Status
{
    public int HP;
    public int MHP;
    public int ATK;
    public int LV;
    public int EXP;
    public int MEXP;

}

public class SaveDataScript : MonoBehaviour {

    [SerializeField]
    public Status playerNowData;
    [SerializeField]
    public int PlayerHpNowData;
    //[SerializeField]
    //public int FloorCount;
    //public int PlayerHpNowData { get; private set; }
    //public int FloorCount { get; private set; }
    [SerializeField]
    bool SaveFlg;
    private static SaveDataScript mInstance;
    private ScoreStatus _playerSaveData;

    public static SaveDataScript Instance
    {
        get
        {
            return mInstance;
        }
    }
    public ScoreStatus GetSaveData()
    {
        return _playerSaveData;
    }
    //public void SetSaveData()
    //{
    //    _playerSaveData.clearFloor = FloorCount;
    //}

    public void SaveFloorCount()
    {
        _playerSaveData.clearFloor += 1;
    }
    public void SetFlgOn()
    {
        SaveFlg = true;
    }
    public void SetFlgOff()
    {
        SaveFlg = false;
    }
    public bool GetFlg()
    {
        return SaveFlg;
    }
    public void ClearData()
    {
        SaveFlg = false;
        //PlayerHpNowData = 0;
        _playerSaveData.clearFloor = 1;
    }

    public void SavePlayerNowData(Status Data)
    {
        playerNowData = Data;
    }

    // Use this for initialization
    void Awake()
    {
        //PlayerHpNowData = 0;
        _playerSaveData.clearFloor = 1;
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}