using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameControllor : MonoBehaviour {

    /// <summary>行動フラグ</summary>
    private bool AcitonFlg;
    /// <summary>プレイヤー攻撃フラグ</summary>
    private bool PatkFlg;
    /// <summary>ポジション固定フラグ</summary>
    private bool LockFlg;
    /// <summary>攻撃チェック用フラグ</summary>
    private bool AtkCheckflg;
    /// <summary>SP攻撃フラグ</summary>
    private bool SpAtkflg;
    /// <summary>次回移動ポジション</summary>
    private int iNext, jNext;
    /// <summary>アイテム工数確認</summary>
    private int itemCount = 0;
    /// <summary>プレイヤー発見状態フラグ</summary>
    private bool PmoveFlg;
    /// <summary>ゴールフラグ</summary>
    [SerializeField]
    private bool GoalFlg;
    /// <summary>ゴールオブジェクト</summary>
    private GameObject _goalObj;
    /// <summary>所持アイテムリスト</summary>
    private List<ItemScript> ItemList = new List<ItemScript>();
    /// <summary>プレイヤー情報</summary>
    private PlayerManager player;
    /// <summary>エネミー情報</summary>
    private EnemyManager enemy;
    /// <summary></summary>
    private Action changeWindow;

    /// <summary>
    /// ゴールの設定
    /// </summary>
    /// <param name="goal"></param>
    public void SetGoalObj(GameObject goal)
    {
        _goalObj = goal;
    }

    /// <summary>
    /// アイテムの位置情報チェック
    /// </summary>
    /// <param name="iPix"></param>
    /// <param name="jPix"></param>
    /// <returns></returns>
    public bool CheckItemPosition(int iPix,int jPix)
    {
        for (int i=0;i < ItemList.Count;i++)
        {
            ItemStatusData data = ItemList[i].ThisData;
            if (data.iPosition == iPix && data.jPosition == jPix)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 所持アイテム追加
    /// </summary>
    /// <param name="itemObj"></param>
    public void AddCountItemObj(ItemScript itemObj)
    {
        itemCount += 1;
        ItemList.Add(itemObj);
    }

    /// <summary>
    /// MAP生成後初期設定
    /// </summary>
    public void AftorMakeMapStart()
    {
        PmoveFlg = false;
        GoalFlg = false;
        LockFlg = false;
        player.StartAttack(AtkCheckflg, SpAtkflg, (afterflg) =>
        {
            AtkCheckflg = afterflg;
        });

    }

    /// <summary>
    /// 壁の有無チェック
    /// </summary>
    void CheckBlockState()
    {
        //次に移動予定のマスが壁でないかのチェック
        if (player.CheckNextStepWall() == false)
        {
            iNext = 0;
            jNext = 0;
            player.SetNextStep(iNext, jNext); 
        }
        else
        {
            List<ActionControllor> enemylist = enemy.GetEnemyList();
            for (int count = 0; count < enemylist.Count; count++)
            {
                if (enemylist[count].CheckNowStep(player.GetNextStepArea().Item1, player.GetNextStepArea().Item2) == true)
                {
                    PmoveFlg = true;
                }
                else
                {//近くにいないとうごいちゃう
                }
            }
            if (PmoveFlg == true)
            {
                iNext = 0;
                jNext = 0;
                player.SetNextStep(iNext, jNext);
                PmoveFlg = false;
            }
            else
            {
                AcitonFlg = true;
                //※敵オブジェクトは0になることもあるため複数対応+無しのときの対応も必要
                enemy.SetEnemyMove();

            }
        }
        if (player.GetNextStepArea().Item1 == (int)Math.Round(_goalObj.transform.position.x) 
            && player.GetNextStepArea().Item2 == (int)Math.Round(_goalObj.transform.position.y))
        {
            GoalFlg = true;
        }
        if (itemCount >= 1)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (player.GetNextStepArea().Item1 == (int)Math.Round(ItemList[i].transform.position.x) && GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2 == (int)Math.Round(ItemList[i].transform.position.y))
                {
                    if (ItemList[i].ThisData.Type != ItemScript.ItemType.SPECIAL)
                    {
                        GameManager.Instance.GetItemWindow().AddGotItemPrefab(ItemList[i].ThisData);
                    }
                    ItemList[i].GetDestroy();
                    ItemList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="playmanager"></param>
    /// <param name="enemymanager"></param>
    /// <param name="itemWindow"></param>
    public void Init(PlayerManager playmanager, EnemyManager enemymanager,Action itemWindow = null) 
    {
        changeWindow = itemWindow;
        iNext = 0;
        jNext = 1;
        AcitonFlg = false;
        PatkFlg = false; 
        AtkCheckflg = false;
        SpAtkflg = false;
        player = playmanager;
        enemy = enemymanager;
        AftorMakeMapStart();
    }

    /// <summary>
    /// セーブデータを設定
    /// </summary>
    /// <param name="saveFinish"></param>
    void SaveData(Action saveFinish)
    {
        SaveDataScript _saveData = SaveDataScript.Instance;
        _saveData.SaveFloorCount();
        _saveData.SavePlayerNowData(player.GetStateData());
        _saveData.SetFlgOn();
        saveFinish.Invoke();
    }

    /// <summary>
    /// 行動フラグ呼び出しサブ
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    IEnumerator coActionFlgOnSub(int count)
    {
        yield return  new WaitForSeconds(0.3f);

        enemy.SetParamAtkEnemyList(count);
    }

    /// <summary>
    /// 行動フラグメイン
    /// </summary>
    /// <returns></returns>
    IEnumerator coActionFlgOnMain() 
    {
        //プレイヤー
        AcitonFlg = true;
        player.StartAttack(AtkCheckflg, SpAtkflg, (afterflg) =>
        {
            AtkCheckflg = afterflg;
        });
        if (PatkFlg == true)//移動中は入力無効にする
        {
            player.SetUserAttackFlagOn();
            if (SpAtkflg == true) 
            {
                player.SpActionStart();
                player.SetSPcount(-1);
            }
            else
            {
                player.ActionStart();
            }
            yield return new WaitForSeconds(0.3f);
            enemy.ResetAttkEnemyList();//攻撃で敵が消えた時のため
            enemy.ResetMoveEnemyList();//攻撃で敵が消えた時のため
        }
        else
        {
            player.ActionStart();
            if (player.GetPowerItemFlg() == true)
            {
                yield return new WaitForSeconds(0.3f);
                player.SetSPcount( 5 );
                itemCount -= 1;
                player.SetPItemFlg(false);
                ////アイテムが複数なら修正が必要
                //GameObject PItem = GameObject.Find("PowerItemPrefab(Clone)");
                //PItem.GetComponent<PItemScript>().GetDestroy();
            }

        }
        enemy.SetParamMoveEnemyList();

        //敵攻撃
        for (int count = 0; count < enemy.GetEnemyAtkCount(); count++)
        {
            yield return coActionFlgOnSub(count);
        }
        yield return new WaitForSeconds(0.3f);

        if (GoalFlg == true)
        {
            yield return new WaitForSeconds(0.3f);
            SaveData(saveFinish: () => {
                SceneManager.LoadScene("GameScene");
            });
        }

        //攻撃・移動・敵全体各リストを一度リセット
        enemy.ResetAllEnemyList();
        PatkFlg = false;
        AcitonFlg = false;
        if (player.CheckPlayerHP() == false)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
    //*操作ボタン処理*//

    /// <summary>
    /// 上
    /// </summary>
    public void Push_U()
    { 
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = 1;
            //Pの動作セット
            player.SetPlayerAction(iNext, jNext,ActionControllor.Direction.UP);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 左上
    /// </summary>
    public void Push_U_L()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = -1;
            jNext = 1;
            player.SetPlayerAction(iNext, jNext, ActionControllor.Direction.UP_LEFT);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 右上
    /// </summary>
    public void Push_U_R()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 1;
            jNext = 1;
            player.SetPlayerAction(iNext, jNext, ActionControllor.Direction.UP_RIGHT);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 下
    /// </summary>
    public void Push_D()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = -1;
            player.SetPlayerAction(iNext, jNext, ActionControllor.Direction.DOWN);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 左下
    /// </summary>
    public void Push_D_L()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = -1;
            jNext = -1;
            player.SetPlayerAction(iNext, jNext, ActionControllor.Direction.DOWN_LEFT);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 右下
    /// </summary>
    public void Push_D_R()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 1;
            jNext = -1;
            player.SetPlayerAction(iNext, jNext, ActionControllor.Direction.DOWN_RIGHT);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 左
    /// </summary>
    public void Push_L()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = -1;
            jNext = 0;
            player.SetPlayerAction(iNext, jNext, ActionControllor.Direction.LEFT);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 右
    /// </summary>
    public void Push_R()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 1;
            jNext = 0;
            player.SetPlayerAction(iNext, jNext, ActionControllor.Direction.RIGHT);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    public void Push_ATTCK()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = 0;
            player.SetPlayerAction(iNext, jNext);
            enemy.SetEnemyMove();
            AtkCheckflg = true; 
            AcitonFlg = true;
            PatkFlg = true;
            StartCoroutine("coActionFlgOnMain");
            if (SpAtkflg == true)
            {
                if (player.GetSPcount() >= 1)
                {
                    SpAtkflg = true;
                }
                else
                {
                    SpAtkflg = false;
                }
            }
        }
    }

    /// <summary>
    /// 移動固定
    /// </summary>
    public void Push_LOCK()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            if (LockFlg == true) //移動中は入力無効にする
            {
                LockFlg = false;
                SpAtkflg = false;
            }
            else
            {
                LockFlg = true;
                if (player.GetSPcount() >= 1)
                {
                    SpAtkflg = true;
                }
                else
                {
                    SpAtkflg = false;
                }
            }
        }
    }

    /// <summary>
    /// アイテムウィンドウ
    /// </summary>
    public void Push_WINDOW()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            changeWindow.Invoke();
        }
    }


}
