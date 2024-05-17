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
    public bool ItemWindowflg;
    private int iNext, jNext;
    private int itemCount = 0;
    private bool PmoveFlg;
    [SerializeField]
    private bool GoalFlg;
    [SerializeField]
    private bool GetPItemFlg;

    private GameObject _goalObj;
    private List<ItemScript> ItemList = new List<ItemScript>();

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
    public void OnGetPItemFlg()
    {
        GetPItemFlg = true;
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
        GetPItemFlg = false;
        LockFlg = false;
    }

    void CheckBlockState()
    {
        //次に移動予定のマスが壁でないかのチェック
        if (GameManager.Instance.GetPlayerManager().CheckNextStepWall() == false)
        {
            iNext = 0;
            jNext = 0;
            GameManager.Instance.GetPlayerManager().SetNextStep(iNext, jNext); 
        }
        else
        {
            List<ActionControllor> enemylist = GameManager.Instance.GetEnemyManager().GetEnemyList();
            for (int count = 0; count < enemylist.Count; count++)
            {
                if (enemylist[count].CheckNowStep(GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1, GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2) == true)
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
                GameManager.Instance.GetPlayerManager().SetNextStep(iNext, jNext);
                PmoveFlg = false;
            }
            else
            {
                AcitonFlg = true;
                //※敵オブジェクトは0になることもあるため複数対応+無しのときの対応も必要
                GameManager.Instance.GetEnemyManager().SetEnemyMove();

            }
        }
        if (GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1 == (int)Math.Round(_goalObj.transform.position.x) 
            && GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2 == (int)Math.Round(_goalObj.transform.position.y))
        {
            GoalFlg = true;
        }
        //GameObject PItem = GameObject.Find("PowerItemPrefab(Clone)");
        if (itemCount >= 1)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1 == (int)Math.Round(ItemList[i].transform.position.x) && GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2 == (int)Math.Round(ItemList[i].transform.position.y))
                {
                    if (ItemList[i].ThisData.Type != ItemScript.ItemType.SPECIAL)
                    {
                        ItemWindowScript.Instance.AddGotItemPrefab(ItemList[i].ThisData);
                    }
                    ItemList[i].GetDestroy();
                    ItemList.RemoveAt(i);
                    break;
                }
            }
        }

    }

    // Use this for initialization
    public void GameCtrlStart () {
        GameManager.Instance.GetPlayerManager().SetDirection(ActionControllor.Direction.DOWN);
        iNext = 0;
        jNext = 1;
        AcitonFlg = false;
        PatkFlg = false; 
        AtkCheckflg = false;
        SpAtkflg = false;
        ItemWindowflg = false;
        
        AftorMakeMapStart();
    }

    void SaveData(Action saveFinish)
    {
        SaveDataScript _saveData = SaveDataScript.Instance;
        _saveData.SaveFloorCount();
        _saveData.SavePlayerNowData(GameManager.Instance.GetPlayerManager().GetStateData());
        _saveData.SetFlgOn();
        saveFinish.Invoke();
        //_saveData.SetSaveData(); 
    }

    IEnumerator coActionFlgOnSub(int count)
    {
        yield return  new WaitForSeconds(0.3f);

        GameManager.Instance.GetEnemyManager().SetParamAtkEnemyList(count);
    }
    IEnumerator coActionFlgOnMain()
    {
        //プレイヤー
        AcitonFlg = true;
        if (PatkFlg == true)//移動中は入力無効にする
        {
            GameManager.Instance.GetPlayerManager().SetUserAttackFlagOn();
            if (SpAtkflg == true) 
            {
                GameManager.Instance.GetPlayerManager().SpActionStart();
                GameManager.Instance.GetPlayerManager().SetSPcount(-1);
            }
            else
            {
                GameManager.Instance.GetPlayerManager().ActionStart();
            }
            yield return new WaitForSeconds(0.3f);
            GameManager.Instance.GetEnemyManager().ResetAttkEnemyList();//攻撃で敵が消えた時のため
            GameManager.Instance.GetEnemyManager().ResetMoveEnemyList();//攻撃で敵が消えた時のため
        }
        else
        {
            GameManager.Instance.GetPlayerManager().SetUserActFlagOn();
            GameManager.Instance.GetPlayerManager().ActionStart();
            if (GoalFlg == true)
            {
                yield return new WaitForSeconds(0.3f);
                ///TODO:ゴールに入った瞬間にゲームを読み直すため
                ///アクションでキャラが動いているとエラーがでるので
                ///キャラを動かした後に飛ぶようにするwhile等で
                ///また切り替える前にウィンドウをだす
                SaveData(saveFinish:()=> {
                    SceneManager.LoadScene("GameScene");
                });
            }
            if (GetPItemFlg == true)
            {
                yield return new WaitForSeconds(0.3f);
                GameManager.Instance.GetPlayerManager().SetSPcount( 5 );
                itemCount -= 1;
                GetPItemFlg = false;
                ////アイテムが複数なら修正が必要
                //GameObject PItem = GameObject.Find("PowerItemPrefab(Clone)");
                //PItem.GetComponent<PItemScript>().GetDestroy();
            }

        }
        GameManager.Instance.GetEnemyManager().SetParamMoveEnemyList();

        //敵攻撃
        for (int count = 0; count < GameManager.Instance.GetEnemyManager().GetEnemyAtkCount(); count++)
        {
            yield return coActionFlgOnSub(count);
        }
        yield return new WaitForSeconds(0.3f);

        //攻撃・移動・敵全体各リストを一度リセット
        GameManager.Instance.GetEnemyManager().ResetAllEnemyList();
        PatkFlg = false;
        AcitonFlg = false;
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext,ActionControllor.Direction.UP);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext, ActionControllor.Direction.UP_LEFT);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext, ActionControllor.Direction.UP_RIGHT);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext, ActionControllor.Direction.DOWN);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext, ActionControllor.Direction.DOWN_LEFT);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext, ActionControllor.Direction.DOWN_RIGHT);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext, ActionControllor.Direction.LEFT);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext, ActionControllor.Direction.RIGHT);
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
            GameManager.Instance.GetPlayerManager().SetPlayerAction(iNext, jNext);
            GameManager.Instance.GetEnemyManager().SetEnemyMove();
            AtkCheckflg = true;
            AcitonFlg = true;
            PatkFlg = true;
            StartCoroutine("coActionFlgOnMain");
            if (SpAtkflg == true)
            {
                if (GameManager.Instance.GetPlayerManager().GetSPcount() >= 1)
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
                if (GameManager.Instance.GetPlayerManager().GetSPcount() >= 1)
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
            if (ItemWindowflg == false)
            {
                ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMWINDOW);
                ItemWindowflg = true;
            }
            else
            {
                ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.GAME);
                ItemWindowflg = false;
            }
        }
    }


}
