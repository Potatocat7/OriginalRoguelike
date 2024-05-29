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
    /// <summary>UI表示</summary>
    [SerializeField] private DisplayScript displayScript = null;
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
        mapGeneObj.Init(finish:(player,enemyList) =>
        {
            playerManager.Init(player,()=> 
            {
                enemyManager.ChangeAttackable(false);
                GameManager.Instance.UpdateDisplay();
            });
            enemyManager.Init(enemyList,(idropitem,jdropitem)=> 
            {
                if(gameCtrl.CheckItemPosition(idropitem, jdropitem))
                {
                    mapGeneObj.SetDropItemObj(idropitem, jdropitem);
                }
            });
            gameCtrl.Init(playerManager, enemyManager, () =>
            {
                ChangeItemWindow();
            });
            displayScript.SetDisplayScript(player.stateData,() =>
            {
                itemWindow.Init((equipitem) =>
                {
                    playerManager.AddItemState(equipitem);
                });
            });
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

    /// <summary>
    /// アイテムウィンドウを返す
    /// </summary>
    /// <returns></returns>
    public ItemWindowScript GetItemWindow()
    {
        return itemWindow;
    }

    /// <summary>
    /// プレイヤーマネージャーを返す
    /// </summary>
    /// <returns></returns>
    public PlayerManager GetPlayerManager()
    {
        return playerManager;
    }

    /// <summary>
    /// エネミーマネージャーを返す
    /// </summary>
    /// <returns></returns>
    public EnemyManager GetEnemyManager()
    {
        return enemyManager;
    }

    /// <summary>
    /// アイテムウィンドウ切り替え
    /// </summary>
    public void ChangeItemWindow()
    {
        itemWindow.ChangeItemWindow();
    }

    /// <summary>
    /// SPアイテム取得フラグ設定
    /// </summary>
    /// <param name="flg"></param>
    public void SetPItemFlg(bool flg)
    {
        playerManager.SetPItemFlg(flg);
    }

    public void UpdateDisplay()
    {
        displayScript.UpdateDisplay();
    }
}
