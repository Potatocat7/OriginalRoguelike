using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public class SaveDataScript : MonoBehaviour 
{
    /// <summary>プレイヤー現状ステータス </summary>
    [SerializeField]
    public Status playerNowData;
    /// <summary>プレイヤーHP</summary>
    [SerializeField]
    public int PlayerHpNowData;
    /// <summary>所持アイテム</summary>
    public List<ItemStatusData> _saveItemList;// = new List<ItemPrefabScript>();
    /// <summary>セーブフラグ</summary>
    [SerializeField]
    bool SaveFlg;
    /// <summary>プレイヤーのスコア</summary>
    private ScoreStatus _playerSaveData;
    
    /// シングルトン化
    private static SaveDataScript mInstance;

    public static SaveDataScript Instance
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
        else
        {
            ClearData();
        }
        mInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// スコアの取得
    /// </summary>
    /// <returns></returns>
    public ScoreStatus GetSaveData()
    {
        return _playerSaveData;
    }

    /// <summary>
    /// セーブデータの破棄
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// フロア階数の取得
    /// </summary>
    public void SaveFloorCount()
    {
        _playerSaveData.clearFloor += 1;
    }

    /// <summary>
    /// フラグON
    /// </summary>
    public void SetFlgOn()
    {
        SaveFlg = true;
    }

    /// <summary>
    /// フラグOFF
    /// </summary>
    public void SetFlgOff()
    {
        SaveFlg = false;
    }

    /// <summary>
    /// セーブフラグ取得
    /// </summary>
    /// <returns></returns>
    public bool GetFlg()
    {
        return SaveFlg;
    }

    /// <summary>
    /// データクリア
    /// </summary>
    public void ClearData()
    {
        SaveFlg = false;
        _playerSaveData.clearFloor = 1;
    }

    /// <summary>
    /// プレイヤーの現状セーブ
    /// </summary>
    /// <param name="Data"></param>
    public void SavePlayerNowData(Status Data)
    {
        playerNowData = Data;

        for (int i= 0;i< GameManager.Instance.GetItemWindow()._gotItemList.Count;i++)
        {
            _saveItemList.Add(GameManager.Instance.GetItemWindow()._gotItemList[i].itemSaveData);
            //移動時に一度アイテムは外したステータスに
            if (GameManager.Instance.GetItemWindow()._gotItemList[i].itemSaveData.EquipFlg == true)
            {
                GameManager.Instance.GetPlayerManager().SubItemState(GameManager.Instance.GetItemWindow()._gotItemList[i].itemSaveData);
            }
        }
    }
}
