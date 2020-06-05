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
    int iNext, jNext;
    int iRandom, jRandom;
    GameObject Player;
    //GameObject AttackEffect;
    GameObject MapGeneObj;
    [SerializeField]
    //GameObject Enemy;
    public List<GameObject> EnemyList = new List<GameObject>();
    [SerializeField]
    List<GameObject> AtkEnemyList = new List<GameObject>();
    [SerializeField]
    List<GameObject> AtkResetEnemyList = new List<GameObject>();
    [SerializeField]
    List<GameObject> MoveEnemyList = new List<GameObject>();
    [SerializeField]
    public int EnemyCount;
    [SerializeField]
    int EnemyAtkCount, EnemyMoveCount, EnemyAtkResetCount;
    //確認用に宣言
    int iPmap, jPmap;
    int iEmap, jEmap;
    int itemCount;
    bool PmoveFlg;
    [SerializeField]
    bool GoalFlg;
    [SerializeField]
    bool GetPItemFlg;

    public void AftorMakeMapStart()
    {
        MapGeneObj = GameObject.Find("MapGenerator");
        Player = GameObject.Find("PlayerPrefab(Clone)");

        for (int count = 0; count < MapGenerator.EnemyCount; count++)
        {
            if (MapGeneObj.GetComponent<MapGenerator>().EnemyList[count] != null)
            {
                EnemyList.Add(MapGeneObj.GetComponent<MapGenerator>().EnemyList[count]);
                EnemyCount += 1;
            }
        }
        EnemyAtkCount = 0;
        EnemyMoveCount = 0;
        EnemyAtkResetCount = 0;
        itemCount = 1;          //アイテムの個数がある場合修正
        PmoveFlg = false;
        GoalFlg = false;
        GetPItemFlg = false;
        LockFlg = false;
    }

    void SetEnemyDirection(int iStep,int jStep , GameObject Enemy)
    {
        if (iStep == 0 && jStep == 1)
        {//UP
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP);
        }
        else if (iStep == -1 && jStep == 1)
        {//UP_LEFT
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_LEFT);
        }
        else if (iStep == 1 && jStep == 1)
        {//UP_RIGHT
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_RIGHT);
        }
        else if (iStep == -1 && jStep == 0)
        {//LEFT
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.LEFT);
        }
        else if (iStep == 1 && jStep == 0)
        {//RIGHT
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.RIGHT);
        }
        else if (iStep == 0 && jStep == -1)
        {//DOWN
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN);
        }
        else if (iStep == -1 && jStep == -1)
        {//DOWN_LEFT
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_LEFT);
        }
        else if (iStep == 1 && jStep == -1)
        {//DOWN_RIGHT
            Enemy.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_RIGHT);
        }
        else
        {//動かないときはそのまま
            
        }

    }
    bool checkNowotherEmoveFlg(GameObject Enemy, int MaxCount)
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (count != MaxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].GetComponent<ActionControllor>().CheckNowStep(Enemy.GetComponent<ActionControllor>().SetiNextStepArea(), Enemy.GetComponent<ActionControllor>().SetjNextStepArea()) == true)
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
    bool checkNextotherEmoveFlg(GameObject Enemy, int MaxCount)
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (count != MaxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].GetComponent<ActionControllor>().CheckNextStep(Enemy.GetComponent<ActionControllor>().SetiNextStepArea(), Enemy.GetComponent<ActionControllor>().SetjNextStepArea()) == true)
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
    void EnemyMoveRandom(GameObject Enemy, int thisCount)
    {
        bool checkRandom = true;
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        while (checkRandom == true)
        {
            iRandom = (int)UnityEngine.Random.Range(-1, 2);
            jRandom = (int)UnityEngine.Random.Range(-1, 2);
            Enemy.GetComponent<ActionControllor>().SetNextStep(iRandom, jRandom);
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
                if (Enemy.GetComponent<ActionControllor>().CheckNextStepWall() == false)
                {
                }
                else
                {
                    SetEnemyDirection(iRandom, jRandom, Enemy);
                    //Enemy.GetComponent<ActionControllor>().SetUserActFlagOn();
                    checkRandom = false;
                }
            }
        }

    }
    void EnemyMoveTargetPlayer(GameObject Enemy,int thisCount)
    {
        //ここはいずれA*アルゴリズムで動かしたい
        int iEnemyNext, jEnemyNext;
        bool otherEmoveFlg = false;

        if (Player.GetComponent<ActionControllor>().SetiNextStepArea() - (int)Math.Round(Enemy.transform.position.x) > 0)
        {
            iEnemyNext = 1;
        }
        else if (Player.GetComponent<ActionControllor>().SetiNextStepArea() - (int)Math.Round(Enemy.transform.position.x) < 0)
        {
            iEnemyNext = -1;
        }
        else
        {//(int)Player.transform.position.x == (int)Enemy.transform.position.x
            iEnemyNext = 0;
        }

        if (Player.GetComponent<ActionControllor>().SetjNextStepArea() - (int)Math.Round(Enemy.transform.position.y) > 0)
        {
            jEnemyNext = 1;
        }
        else if (Player.GetComponent<ActionControllor>().SetjNextStepArea() - (int)Math.Round(Enemy.transform.position.y) < 0)
        {
            jEnemyNext = -1;
        }
        else
        {//(int)Player.transform.position.y == (int)Enemy.transform.position.y
            jEnemyNext = 0;
        }
        Enemy.GetComponent<ActionControllor>().SetNextStep(iEnemyNext, jEnemyNext);

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
            if (Enemy.GetComponent<ActionControllor>().CheckNextStepWall() == false)
            {
                SetEnemyDirection(iEnemyNext, jEnemyNext, Enemy);
                iEnemyNext = 0;
                jEnemyNext = 0;
                Enemy.GetComponent<ActionControllor>().SetNextStep(iEnemyNext, jEnemyNext);
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
            Enemy.GetComponent<ActionControllor>().SetNextStep(iEnemyNext, jEnemyNext);

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
                iPmap = Player.GetComponent<ActionControllor>().SetiNextStepArea();
                //jEmap = (int)Math.Round(Enemy.transform.position.y);
                jEmap = (int)Math.Round(EnemyList[count].transform.position.y);
                jPmap = Player.GetComponent<ActionControllor>().SetjNextStepArea();

                if (EnemyList[count].GetComponent<EnemyAttack>().CheckPlayerThisAround(iPmap, jPmap, iEmap, jEmap) == true)//各敵の周囲(3*3)にプレイヤーがいるかチェックし居たらそちらに方向を切り替えて攻撃動作をセット
                {//周囲を調べてプレイヤーがいた場合方向だけセットしておく
                 //攻撃リストに登録 ※ここは攻撃前
                 //※攻撃方向が指定できていないことがある
                 //SetEnemyDirection(iEnemyNext, jEnemyNext, EnemyList[count]);
                    //攻撃時のリスト追加と同時に攻撃フラグをture
                    EnemyList[count].GetComponent<ActionControllor>().SetUserAttackFlg();
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
                if (EnemyList[count].GetComponent<ActionControllor>().GetUserAttackFlg() == false)
                {
                    if (MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x), (int)Math.Round(EnemyList[count].transform.position.y)] == 0)
                    {//通路だった場合は
                        EnemyMoveRandom(EnemyList[count], count); //現状はランダム移動（後で通路は直進するようにしたい）
                    }
                    else
                    {
                        if (MapGenerator.map[(int)Math.Round(Player.transform.position.x), (int)Math.Round(Player.transform.position.y)] == MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x), (int)Math.Round(EnemyList[count].transform.position.y)])
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
                iPmap = Player.GetComponent<ActionControllor>().SetiNextStepArea();
                jEmap = (int)Math.Round(AtkEnemyList[count].transform.position.y);
                jPmap = Player.GetComponent<ActionControllor>().SetjNextStepArea();
                AtkEnemyList[count].GetComponent<EnemyAttack>().SetDirectionPlayerThisAround(iPmap, jPmap, iEmap, jEmap);
            }
        }
    }
    void CheckBlockState()
    {
        Player = GameObject.Find("PlayerPrefab(Clone)");

        //次に移動予定のマスが壁でないかのチェック
        if (Player.GetComponent<ActionControllor>().CheckNextStepWall() == false)
        {
            iNext = 0;
            jNext = 0;
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext); 
        }
        else
        {
            for (int count = 0; count < EnemyCount; count++)
            {
                if (EnemyList[count].GetComponent<ActionControllor>().CheckNowStep(Player.GetComponent<ActionControllor>().SetiNextStepArea(), Player.GetComponent<ActionControllor>().SetjNextStepArea()) == true)
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
                Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
                PmoveFlg = false;
            }
            else
            {
                AcitonFlg = true;
                //Player.GetComponent<ActionControllor>().SetUserActFlagOn();
                //※敵オブジェクトは0になることもあるため複数対応+無しのときの対応も必要
                SetEnemyMove();

            }
        }
        GameObject Goal = GameObject.Find("GoalPrefab(Clone)");
        if (Player.GetComponent<ActionControllor>().SetiNextStepArea() == (int)Math.Round(Goal.transform.position.x) && Player.GetComponent<ActionControllor>().SetjNextStepArea() == (int)Math.Round(Goal.transform.position.y))
        {
            GoalFlg = true;
        }
        GameObject PItem = GameObject.Find("PowerItemPrefab(Clone)");
        if (itemCount >= 1)
        {
            if (Player.GetComponent<ActionControllor>().SetiNextStepArea() == (int)Math.Round(PItem.transform.position.x) && Player.GetComponent<ActionControllor>().SetjNextStepArea() == (int)Math.Round(PItem.transform.position.y))
            {
                GetPItemFlg = true;
            }
        }

    }

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("PlayerPrefab(Clone)"); //エラー箇所
        Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN);
        iNext = 0;
        jNext = 1;
        AcitonFlg = false;
        PatkFlg = false; 
        AtkCheckflg = false;
        SpAtkflg = false;
    }

    void ResetEnemyList()
    {
        //Listの初期化
        EnemyList.Clear();
        EnemyCount = 0;
        for (int count = 0; count < MapGenerator.EnemyCount; count++)
        {
            if (MapGeneObj.GetComponent<MapGenerator>().EnemyList[count] != null)
            {
                EnemyList.Add(MapGeneObj.GetComponent<MapGenerator>().EnemyList[count]);
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
    void SaveData()
    {
        GameObject Save;
        Save = GameObject.Find("SaveDataObject"); 
        Save.GetComponent<SaveDataScript>().SaveFloorCount();
        Save.GetComponent<SaveDataScript>().SavePlayerHpNowData(Player.GetComponent<StatusDataScript>().GetNowHP());
        Save.GetComponent<SaveDataScript>().SetFlgOn();
    }
    IEnumerator coActionFlgOnSub(int count)
    {
        yield return  new WaitForSeconds(0.3f);

        if (EnemyAtkCount > 0)
        {
            AtkEnemyList[count].GetComponent<ActionControllor>().SetUserAttackFlagOn();
            AtkEnemyList[count].GetComponent<ActionControllor>().ActionStart();
            AtkEnemyList[count].GetComponent<EnemyAttack>().AttackHit();
        }
    }
    IEnumerator coActionFlgOnMain()
    {
        //プレイヤー
        AcitonFlg = true;
        if (PatkFlg == true)
        {
            Player.GetComponent<ActionControllor>().SetUserAttackFlagOn();
            if (SpAtkflg == true) //移動中は入力無効にする
            {
                Player.GetComponent<ActionControllor>().SpActionStart();
                Player.GetComponent<StatusDataScript>().SetSPcount(-1);
            }
            else
            {
                Player.GetComponent<ActionControllor>().ActionStart();
            }
            yield return new WaitForSeconds(0.3f);
            ResetAttkEnemyList();//攻撃で敵が消えた時のため
        }
        else
        {
            Player.GetComponent<ActionControllor>().SetUserActFlagOn();
            Player.GetComponent<ActionControllor>().ActionStart();
            if (GoalFlg == true)
            {
                yield return new WaitForSeconds(0.3f);
                SaveData();
                SceneManager.LoadScene("GameScene");
            }
            if (GetPItemFlg == true)
            {
                yield return new WaitForSeconds(0.3f);
                Player.GetComponent<StatusDataScript>().SetSPcount( 5 );
                itemCount -= 1;
                GetPItemFlg = false;
                //アイテムが複数なら修正が必要
                GameObject PItem = GameObject.Find("PowerItemPrefab(Clone)");
                PItem.GetComponent<PItemScript>().GetDestroy();
            }

        }
        for (int count = 0; count < EnemyMoveCount; count++)
        {
            MoveEnemyList[count].GetComponent<ActionControllor>().SetUserActFlagOn();
            MoveEnemyList[count].GetComponent<ActionControllor>().ActionStart();
            
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
    // Update is called once per frame
    void Update () {

        if (AcitonFlg == true)//プレイヤーフェーズとエネミーフェーズを用意が必要
        {

        }
    }

    public void Push_U()
    { 
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = 1;
            //Pの動作セット
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP);
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_LEFT);
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_RIGHT);
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN);
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_LEFT);
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_RIGHT);
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.LEFT);
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.RIGHT);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
                StartCoroutine("coActionFlgOnMain");
            }
        }
    }
    public void Push_ATTCK()
    {
        Player = GameObject.Find("PlayerPrefab(Clone)");
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = 0;
            Player.GetComponent<ActionControllor>().SetThisNowStep();
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            //Player.GetComponent<ActionControllor>().SetUserAttackFlagOn();
            SetEnemyMove();
            AtkCheckflg = true;
            //Player.GetComponent<PlayerAttack_1>().AttackHit();
            AcitonFlg = true;
            PatkFlg = true;
            StartCoroutine("coActionFlgOnMain");
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
                if (Player.GetComponent<StatusDataScript>().GetSpcount() >= 1)
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
    

}
