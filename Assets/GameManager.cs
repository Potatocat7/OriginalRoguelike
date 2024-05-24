using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>マップ生成</summary>
    [SerializeField] private MapGenerator mapGeneObj = null;
    /// <summary>ゲーム操作</summary>
    [SerializeField] private GameControllor gameCtrl = null;
    /// <summary>プレイヤーマネージャー</summary>
    [SerializeField] private PlayerManager playerManager = null;
    /// <summary>エネミーマネージャー</summary>
    [SerializeField] private EnemyManager enemyManager = null;
    /// <summary>アイテムウィンドウ</summary>
    [SerializeField] private ItemWindowScript itemWindow = null;



    //シングルトン化
    private static GameManager mInstance;
    public static GameManager Instance
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
    void Start()
    {
        mapGeneObj.MapGeneStart(finish:(player,enemyList) =>
        {
            playerManager.Init(player,()=> 
            {
                enemyManager.ChangeAttackable(false);
            });
            enemyManager.Init(enemyList,(idropitem,jdropitem)=> 
            {
                if(gameCtrl.CheckItemPosition(idropitem, jdropitem))
                {
                    mapGeneObj.SetDropItemObj(idropitem, jdropitem);
                }
            });
            gameCtrl.GameCtrlStart(playerManager, enemyManager, () =>
            {
                ChangeItemWindow();
            });
            itemWindow.Init((equipitem) =>
            {
                playerManager.AddItemState(equipitem);
            });
            //ItemWindowflg = false;
        },
        setItem: (makeitem) => 
        {
            gameCtrl.AddCountItemObj(makeitem);
        },
        setGoal: (goal) =>
        {
            gameCtrl.SetGoalObj(goal);
        });
    }
    
    public ItemWindowScript GetItemWindow()
    {
        return itemWindow;
    }

    public PlayerManager GetPlayerManager()
    {
        return playerManager;
    }
    public EnemyManager GetEnemyManager()
    {
        return enemyManager;
    }

    public void ChangeItemWindow()
    {
        itemWindow.ChangeItemWindow();
    }
    //public bool GetItemWindowflg()
    //{
    //    return ItemWindowflg;
    //}
    public void SetPItemFlg(bool flg)
    {
        playerManager.SetPItemFlg(flg);
    }

    ///TODO:GameControllorの一部処理をここで行う
    ///主に操作以外の処理

}
