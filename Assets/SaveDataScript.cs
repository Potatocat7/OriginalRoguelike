using System.Collections;
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
    public bool GetFlg()
    {
        return SaveFlg;
    }

    public void SavePlayerHpNowData(int NowHP)
    {
        PlayerHpNowData = NowHP;
    }

    // Use this for initialization
    void Start () {
        SaveFlg = false;
        PlayerHpNowData = 0;
        FloorCount = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
