using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameControllor : MonoBehaviour {


    public bool AcitonFlg;
    public bool PatkFlg;
    public bool LockFlg;
    public bool AtkCheckflg;//攻撃判定のフラグ
    public bool SpAtkflg;
    //public bool ItemWindowflg;
    private int iNext, jNext;
    private int itemCount = 0;
    private bool PmoveFlg;
    [SerializeField]
    private bool GoalFlg;
    //[SerializeField]
    //private bool GetPItemFlg;

    private GameObject _goalObj;
    private List<ItemScript> ItemList = new List<ItemScript>();

    private PlayerManager player;
    private EnemyManager enemy;

    private Action changeWindow;

    //シングルトン化
    private static GameControllor mInstance;
    public static GameControllor Instance
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
    public void SetGoalObj(GameObject goal)
    {
        _goalObj = goal;
    }
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
        //itemCount = 0;          //アイテムの個数がある場合修正
        PmoveFlg = false;
        GoalFlg = false;
        LockFlg = false;
    }

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
        //GameObject PItem = GameObject.Find("PowerItemPrefab(Clone)");
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

    // Use this for initialization
    public void GameCtrlStart (PlayerManager playmanager, EnemyManager enemymanager,Action itemWindow = null) 
    {
        //GameManager.Instance.GetPlayerManager().SetDirection(ActionControllor.Direction.DOWN);
        changeWindow = itemWindow;
        iNext = 0;
        jNext = 1;
        AcitonFlg = false;
        PatkFlg = false; 
        AtkCheckflg = false;
        SpAtkflg = false;
        //ItemWindowflg = false;
        player = playmanager;
        enemy = enemymanager;
        AftorMakeMapStart();
    }

    void SaveData(Action saveFinish)
    {
        SaveDataScript _saveData = SaveDataScript.Instance;
        _saveData.SaveFloorCount();
        _saveData.SavePlayerNowData(player.GetStateData());
        _saveData.SetFlgOn();
        saveFinish.Invoke();
        //_saveData.SetSaveData(); 
    }

    IEnumerator coActionFlgOnSub(int count)
    {
        yield return  new WaitForSeconds(0.3f);

        enemy.SetParamAtkEnemyList(count);
    }
    IEnumerator coActionFlgOnMain()
    {
        //プレイヤー
        AcitonFlg = true;
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
            player.SetUserActFlagOn();
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
    void Update () {

        if (AcitonFlg == true)//プレイヤーフェーズとエネミーフェーズを用意が必要
        {

        }
    }
    //*操作ボタン処理*//

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
                //Eを攻撃/移動でリストにセット
                //攻撃の方向をセット
                //移動の移動先と方向をセット
                CheckBlockState();
                //コルーチン呼び出しで
                //P/移動がまとめて移動
                //攻撃が順に攻撃
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }
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
    public void Push_WINDOW()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            changeWindow.Invoke();
            //GameManager.Instance.ChangeItemWindow();
            //if (ItemWindowflg == false)
            //{
            //    ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMWINDOW);
            //    ItemWindowflg = true;
            //}
            //else
            //{
            //    ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.GAME);
            //    ItemWindowflg = false;
            //}
        }
    }


}
