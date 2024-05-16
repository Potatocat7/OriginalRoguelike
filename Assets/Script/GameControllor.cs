using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameControllor : MonoBehaviour {


    /*enum Direction
    {
        UP = 0,
        UP_LEFT,
        UP_RIGHT,
        LEFT,
        RIGHT,
        DOWN,
        DOWN_LEFT,
        DOWN_RIGHT
    }
    Direction PlayerDirection;*/
    public bool AcitonFlg;
    public bool PatkFlg;
    public bool LockFlg;
    public bool AtkCheckflg;//攻撃判定のフラグ
    public bool SpAtkflg;
    public bool ItemWindowflg;
    private int iNext, jNext;
    private int iRandom, jRandom;
    //private ActionControllor Player = null;
    //private StatusDataScript _playerState = null;
    //[SerializeField]
    //private SaveDataScript _saveData = null;
    [SerializeField]
    private MapGenerator MapGeneObj;
    private List<ActionControllor> AtkEnemyList = new List<ActionControllor>();
    private List<ActionControllor> AtkResetEnemyList = new List<ActionControllor>();
    private List<ActionControllor> MoveEnemyList = new List<ActionControllor>();
    private List<ActionControllor> MoveResetEnemyList = new List<ActionControllor>();
    [SerializeField]
    public int EnemyCount;
    [SerializeField]
    private int EnemyAtkCount, EnemyMoveCount, EnemyAtkResetCount, EnemyMoveResetCount;
    //確認用に宣言
    private int iPmap, jPmap;
    private int iEmap, jEmap;
    private int itemCount = 0;
    private bool PmoveFlg;
    [SerializeField]
    private bool GoalFlg;
    [SerializeField]
    private bool GetPItemFlg;

    private GameObject _goalObj;
    private List<ItemScript> ItemList = new List<ItemScript>();
    private List<StatusDataScript> EnemyListState = new List<StatusDataScript>();
    private List<ActionControllor> EnemyList = new List<ActionControllor>();

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
        //DontDestroyOnLoad(gameObject);
    }
    //public int GetPlayerHpNow()
    //{
    //    return _playerState.GetHPnow();
    //}

    //public void AddItemState(ItemStatusData data)
    //{
    //    _playerState.AddState(data);
    //}
    //public void SubItemState(ItemStatusData data)
    //{
    //    _playerState.SubState(data);
    //}
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
            //ListについてるGetComponentもやり方で削除できそう
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

    //public void SetPlayerActionCtrl(ActionControllor player)
    //{
    //    Player = player;
    //}
    //public void SetPlayerState(StatusDataScript pState)
    //{
    //    _playerState = pState;
    //    ItemWindowScript.Instance.SetupItemState();
    //}

    /// <summary>
    /// 攻撃ヒット判定処理
    /// </summary>
    /// <param name="iAttack"></param>
    /// <param name="jAttack"></param>
    /// <param name="attack"></param>
    public void Hitcheck(int iAttack, int jAttack , int attack)
    {
        //複数or0だったときの処理が必要？
        if (MapGenerator.EnemyCount >= 1)
        {
            for (int count = 0; count < EnemyCount; count++)
            {
                if (EnemyListState[count].CheckAttack(iAttack, jAttack) == true)
                {
                    EnemyListState[count].HitDamage(attack);
                }
                else
                {

                }
            }

        }
    }
    /// <summary>
    /// MAP生成後初期設定
    /// </summary>
    public void AftorMakeMapStart()
    {
        for (int count = 0; count < MapGenerator.EnemyCount; count++)
        {
            if (MapGeneObj.EnemyList[count] != null)
            {
                EnemyList.Add(MapGeneObj.EnemyList[count]);
                EnemyListState.Add(MapGeneObj.EnemyList[count].stateData);
                EnemyCount += 1;
            }
        }
        EnemyAtkCount = 0;
        EnemyMoveCount = 0;
        EnemyAtkResetCount = 0;
        EnemyMoveResetCount = 0;
        //itemCount = 0;          //アイテムの個数がある場合修正
        PmoveFlg = false;
        GoalFlg = false;
        GetPItemFlg = false;
        LockFlg = false;
    }

    /// <summary>
    /// 敵エネミー向き設定
    /// </summary>
    /// <param name="iStep"></param>
    /// <param name="jStep"></param>
    /// <param name="Enemy"></param>
    void SetEnemyDirection(int iStep,int jStep , ActionControllor Enemy)
    {
        if (iStep == 0 && jStep == 1)
        {//UP
            //Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP);
            Enemy.SetDirection(ActionControllor.Direction.UP);
        }
        else if (iStep == -1 && jStep == 1)
        {//UP_LEFT
            Enemy.SetDirection(ActionControllor.Direction.UP_LEFT);
        }
        else if (iStep == 1 && jStep == 1)
        {//UP_RIGHT
            Enemy.SetDirection(ActionControllor.Direction.UP_RIGHT);
        }
        else if (iStep == -1 && jStep == 0)
        {//LEFT
            Enemy.SetDirection(ActionControllor.Direction.LEFT);
        }
        else if (iStep == 1 && jStep == 0)
        {//RIGHT
            Enemy.SetDirection(ActionControllor.Direction.RIGHT);
        }
        else if (iStep == 0 && jStep == -1)
        {//DOWN
            Enemy.SetDirection(ActionControllor.Direction.DOWN);
        }
        else if (iStep == -1 && jStep == -1)
        {//DOWN_LEFT
            Enemy.SetDirection(ActionControllor.Direction.DOWN_LEFT);
        }
        else if (iStep == 1 && jStep == -1)
        {//DOWN_RIGHT
            Enemy.SetDirection(ActionControllor.Direction.DOWN_RIGHT);
        }
        else
        {//動かないときはそのまま
            
        }

    }

    /// <summary>
    /// 敵移動先チェック
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="MaxCount"></param>
    /// <returns></returns>
    bool checkNowotherEmoveFlg(ActionControllor Enemy, int MaxCount)
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (count != MaxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].CheckNowStep(Enemy.SetiNextStepArea(), Enemy.SetjNextStepArea()) == true)
                {
                    return true;
                }
                else
                {
                }
            }
        }
        return false;//何事もなく終わったとき
    }

    /// <summary>
    /// 敵移動先に先約がいないかチェック
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="MaxCount"></param>
    /// <returns></returns>
    bool checkNextotherEmoveFlg(ActionControllor Enemy, int MaxCount)
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (count != MaxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].CheckNextStep(Enemy.SetiNextStepArea(), Enemy.SetjNextStepArea()) == true)
                {
                    return true;
                }
                else
                {
                }
            }
        }
        return false;//何事もなく終わったとき
    }

    /// <summary>
    /// 敵移動（ランダム）
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="thisCount"></param>
    void EnemyMoveRandom(ActionControllor Enemy, int thisCount)
    {
        bool checkRandom = true;
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        while (checkRandom == true)
        {
            iRandom = (int)UnityEngine.Random.Range(-1, 2);
            jRandom = (int)UnityEngine.Random.Range(-1, 2);
            Enemy.SetNextStep(iRandom, jRandom);
            bool otherEmoveFlg = false;

            if (thisCount == 0)//[0]のみ現在位置で調べる
            {
                otherEmoveFlg = checkNowotherEmoveFlg(Enemy, EnemyCount);
            }
            else
            {
                otherEmoveFlg = checkNextotherEmoveFlg(Enemy, thisCount);
            }
            if (otherEmoveFlg != true)
            {
                if (Enemy.CheckNextStepWall() == false)
                {
                }
                else
                {
                    SetEnemyDirection(iRandom, jRandom, Enemy);
                    checkRandom = false;
                }
            }
        }

    }

    /// <summary>
    /// 敵移動（プレイヤー視認）
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="thisCount"></param>
    void EnemyMoveTargetPlayer(ActionControllor Enemy, int thisCount)
    {
        //ここはいずれA*アルゴリズムで動かしたい
        int iEnemyNext, jEnemyNext;
        bool otherEmoveFlg = false;
        if (GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1 - (int)Math.Round(Enemy.transform.position.x) > 0)
        {
            iEnemyNext = 1;
        }
        else if (GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1 - (int)Math.Round(Enemy.transform.position.x) < 0)
        {
            iEnemyNext = -1;
        }
        else
        {//(int)Player.transform.position.x == (int)Enemy.transform.position.x
            iEnemyNext = 0;
        }

        if (GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2 - (int)Math.Round(Enemy.transform.position.y) > 0)
        {
            jEnemyNext = 1;
        }
        else if (GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2 - (int)Math.Round(Enemy.transform.position.y) < 0)
        {
            jEnemyNext = -1;
        }
        else
        {//(int)Player.transform.position.y == (int)Enemy.transform.position.y
            jEnemyNext = 0;
        }
        Enemy.SetNextStep(iEnemyNext, jEnemyNext);

        //for分で該当オブジェクトより手前に設定している敵オブジェクトをしらべる（後のオブジェクトは移動先を設定していないため）
        //※後半のオブジェクトが移動しなかった場合重なる可能性がある（例：後半の敵で攻撃可能がいた場合）
        //ここでの処理を攻撃と移動のリストに分けたうえで移動側のみ調べる。調べる対象は全リスト
        if (thisCount == 0)//[0]のみ現在位置で調べる
        {
            otherEmoveFlg = checkNowotherEmoveFlg(Enemy, EnemyCount);
        }
        else
        {
            otherEmoveFlg = checkNextotherEmoveFlg(Enemy, thisCount);
        }
        if (otherEmoveFlg != true)
        {
            if (Enemy.CheckNextStepWall() == false)
            {
                SetEnemyDirection(iEnemyNext, jEnemyNext, Enemy);
                iEnemyNext = 0;
                jEnemyNext = 0;
                Enemy.SetNextStep(iEnemyNext, jEnemyNext);
            }
            else
            {
                SetEnemyDirection(iEnemyNext, jEnemyNext, Enemy);
            }
        }
        else
        {
            iEnemyNext = 0;
            jEnemyNext = 0;
            Enemy.SetNextStep(iEnemyNext, jEnemyNext);

        }
    }
    void SetEnemyMove()
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (MapGenerator.EnemyCount >= 1) {
                //※敵動作については条件で複数パターンあるため現状は仮設定
                //Enemy = GameObject.Find("EnemyPrefab(Clone)");
                //現状プレイヤーの移動前の情報を所得しているので移動先の情報にする必要あり
                //確認用に宣言中　現在Playerの位置情報が移動前の位置情報を所得している※positionが少数点で0.999になると切り捨てになってしまう
                //iEmap = (int)Math.Round(Enemy.transform.position.x);
                iEmap = (int)Math.Round(EnemyList[count].transform.position.x);
                iPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1;
                //jEmap = (int)Math.Round(Enemy.transform.position.y);
                jEmap = (int)Math.Round(EnemyList[count].transform.position.y);
                jPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2;

                //GetComponentを対応できないか
                if (EnemyList[count].enemyAtk.CheckPlayerThisAround(iPmap, jPmap, iEmap, jEmap) == true)//各敵の周囲(3*3)にプレイヤーがいるかチェックし居たらそちらに方向を切り替えて攻撃動作をセット
                {//周囲を調べてプレイヤーがいた場合方向だけセットしておく
                    //攻撃リストに登録 ※ここは攻撃前
                    //※攻撃方向が指定できていないことがある
                    //SetEnemyDirection(iEnemyNext, jEnemyNext, EnemyList[count]);
                    //攻撃時のリスト追加と同時に攻撃フラグをture
                    EnemyList[count].SetUserAttackFlg();
                    AtkEnemyList.Add(EnemyList[count]);
                    EnemyAtkCount += 1;
                }
                else
                {   //※リストへの割り当てと移動方法への関数呼び出しは別にする
                    //EnemyList[count].GetComponent<EnemyAttack>().SetDirectionPlayerThisAround(iPmap, jPmap, iEmap, jEmap);
                    MoveEnemyList.Add(EnemyList[count]);
                    EnemyMoveCount += 1;
                }

            }
        }
        for (int count = 0; count < EnemyCount; count++)
        {
            if (EnemyCount >= 1)
            {
                //MoveEnemyListだと移動チェック時に他の敵の位置を見るときに攻撃時の敵を見ることが現状できないため
                if (EnemyList[count].GetUserAttackFlg() == false)
                {
                    if (MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x),
                        (int)Math.Round(EnemyList[count].transform.position.y)] == 0)
                    {//通路だった場合は
                        EnemyMoveRandom(EnemyList[count], count); //現状はランダム移動（後で通路は直進するようにしたい）
                    }
                    else
                    {
                        if (MapGenerator.map[(int)Math.Round(GameManager.Instance.GetPlayerManager().GetTransform().position.x),
                            (int)Math.Round(GameManager.Instance.GetPlayerManager().GetTransform().position.y)] == MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x),
                            (int)Math.Round(EnemyList[count].transform.position.y)])
                        {
                            EnemyMoveTargetPlayer(EnemyList[count], count);
                        }
                        else
                        {
                            EnemyMoveRandom(EnemyList[count], count);
                        }
                    }
                }
            }
        }
        //攻撃リストの敵の攻撃方向の指定
        for (int count = 0; count < EnemyAtkCount; count++)
        {
            if (EnemyAtkCount >= 1)
            {
                iEmap = (int)Math.Round(AtkEnemyList[count].transform.position.x);
                iPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1;
                jEmap = (int)Math.Round(AtkEnemyList[count].transform.position.y);
                jPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2;
                AtkEnemyList[count].enemyAtk.SetDirectionPlayerThisAround(iPmap, jPmap, iEmap, jEmap);
            }
        }
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
            for (int count = 0; count < EnemyCount; count++)
            {
                if (EnemyList[count].CheckNowStep(GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1, GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2) == true)
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
                SetEnemyMove();

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

    void ResetEnemyList()
    {
        //Listの初期化
        EnemyList.Clear();
        EnemyListState.Clear();
        EnemyCount = 0;
        for (int count = 0; count < MapGenerator.EnemyCount; count++)
        {
            if (MapGeneObj.EnemyList[count] != null)
            {
                EnemyList.Add(MapGeneObj.EnemyList[count]);
                EnemyListState.Add(MapGeneObj.EnemyListStateData[count]);
                EnemyCount += 1;
            }
        }

    }

    void ResetAttkEnemyList()
    {

        //Listの初期化
        AtkResetEnemyList.Clear();
        EnemyAtkResetCount = 0;
        for (int count = 0; count < EnemyAtkCount; count++)
        {
            if (AtkEnemyList[count] != null)
            {
                AtkResetEnemyList.Add(AtkEnemyList[count]);
                EnemyAtkResetCount += 1;
            }
        }

        AtkEnemyList.Clear();
        for (int count = 0; count < EnemyAtkResetCount; count++)
        {
            AtkEnemyList.Add(AtkResetEnemyList[count]);
        }
        EnemyAtkCount = EnemyAtkResetCount;

    }
    void ResetMoveEnemyList()
    {

        //Listの初期化
        MoveResetEnemyList.Clear();
        EnemyMoveResetCount = 0;
        for (int count = 0; count < EnemyMoveCount; count++)
        {
            if (MoveEnemyList[count] != null)
            {
                MoveResetEnemyList.Add(MoveEnemyList[count]);
                EnemyMoveResetCount += 1;
            }
        }

        MoveEnemyList.Clear();
        for (int count = 0; count < EnemyMoveResetCount; count++)
        {
            MoveEnemyList.Add(MoveResetEnemyList[count]);
        }
        EnemyMoveCount = EnemyMoveResetCount;

    }
    void SaveData()
    {
        SaveDataScript _saveData = SaveDataScript.Instance;
        _saveData.SaveFloorCount();
        _saveData.SavePlayerNowData(GameManager.Instance.GetPlayerManager().GetStateData());
        _saveData.SetFlgOn(); 
        //_saveData.SetSaveData(); 
    }

    IEnumerator coActionFlgOnSub(int count)
    {
        yield return  new WaitForSeconds(0.3f);

        if (EnemyAtkCount > 0)
        {
            //ここでもGetComponent。なんとかしたい
            AtkEnemyList[count].SetUserAttackFlagOn();
            AtkEnemyList[count].ActionStart();
            AtkEnemyList[count].enemyAtk.AttackHit();
        }
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
            ResetAttkEnemyList();//攻撃で敵が消えた時のため
            ResetMoveEnemyList();//攻撃で敵が消えた時のため
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
                SaveData();
                SceneManager.LoadScene("GameScene");
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
        for (int count = 0; count < EnemyMoveCount; count++)
        {
            MoveEnemyList[count].SetUserActFlagOn();
            MoveEnemyList[count].ActionStart();

        }

        //敵攻撃
        for (int count = 0; count < EnemyAtkCount; count++)
        {
            yield return coActionFlgOnSub(count);
        }
        yield return new WaitForSeconds(0.3f);

        //攻撃・移動・敵全体各リストを一度リセット
        ResetEnemyList();
        AtkEnemyList.Clear();
        EnemyAtkCount = 0;
        MoveEnemyList.Clear();
        EnemyMoveCount = 0;
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
            SetEnemyMove();
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
