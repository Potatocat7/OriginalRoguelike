using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>マップ生成</summary>
    [SerializeField]private MapGenerator mapGeneObj = null;
    /// <summary>ゲーム操作</summary>
    [SerializeField] private GameControllor gameCtrl = null;
    /// <summary>プレイヤーマネージャー</summary>
    [SerializeField] private PlayerManager playerManager = null;
    /// <summary>エネミーマネージャー</summary>
    [SerializeField] private EnemyManager enemyManager = null;
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
        mapGeneObj.MapGeneStart(finish:()=> {
            gameCtrl.GameCtrlStart();
            playerManager.Init();
            enemyManager.Init();
        });
    }
    ///TODO:GameControllorの一部処理をここで行う
    ///主に操作以外の処理

}
