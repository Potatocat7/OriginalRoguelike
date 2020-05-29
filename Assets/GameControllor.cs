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
    bool PmoveFlg;
    [SerializeField]
    bool GoalFlg;

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
        PmoveFlg = false;
        GoalFlg = false;
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

            for (int count = 0; count < thisCount; count++)
            {
                if (count != thisCount)//自分の位置については無視
                {
                    if (EnemyList[count].GetComponent<ActionControllor>().CheckNextStep(Enemy.GetComponent<ActionControllor>().SetiNextStepArea(), Enemy.GetComponent<ActionControllor>().SetjNextStepArea()) == true)
                    {
                        otherEmoveFlg = true;
                    }
                    else
                    {
                    }
                }
            }
            for (int count = 0; count < EnemyCount; count++)
            {
                if (count != thisCount)//自分の位置については無視
                {
                    if (EnemyList[count].GetComponent<ActionControllor>().CheckNowStep(Enemy.GetComponent<ActionControllor>().SetiNextStepArea(), Enemy.GetComponent<ActionControllor>().SetjNextStepArea()) == true)
                    {
                        otherEmoveFlg = true;
                    }
                    else
                    {
                    }
                }
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
        MoveEnemyList.Add(Enemy);
        EnemyMoveCount += 1;

    }
    void EnemyMoveTargetPlayer(GameObject Enemy,int thisCount)
    {
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
        for (int count = 0; count < thisCount; count++)
        {
            if (count != thisCount) //自分の位置については無視//こっちは自分の番号まで来ないからいらない？
            {
                //全的オブジェクトの移動位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].GetComponent<ActionControllor>().CheckNextStep(Enemy.GetComponent<ActionControllor>().SetiNextStepArea(), Enemy.GetComponent<ActionControllor>().SetjNextStepArea()) == true)
                {
                    otherEmoveFlg = true;
                }
                else
                {
                }
            }
        }
        //for分で全敵オブジェクトをチェックする（うまくいかない）
        for (int count = 0; count < EnemyCount; count++)
        {
            if (count != thisCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].GetComponent<ActionControllor>().CheckNowStep(Enemy.GetComponent<ActionControllor>().SetiNextStepArea(), Enemy.GetComponent<ActionControllor>().SetjNextStepArea()) == true)
                {
                    otherEmoveFlg = true;
                }
                else
                {
                }
            }
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
        MoveEnemyList.Add(Enemy);
        EnemyMoveCount += 1;
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
                    AtkEnemyList.Add(EnemyList[count]);
                    EnemyAtkCount += 1;
                }
               else
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
                            EnemyMoveRandom(EnemyList[count],count);
                        }
                    }
                }
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
                Player.GetComponent<ActionControllor>().SetUserActFlagOn();
                //※敵オブジェクトは0になることもあるため複数対応+無しのときの対応も必要
                SetEnemyMove();

            }
        }
        GameObject Goal = GameObject.Find("GoalPrefab(Clone)");
        if (Player.GetComponent<ActionControllor>().SetiNextStepArea() == (int)Math.Round(Goal.transform.position.x) && Player.GetComponent<ActionControllor>().SetjNextStepArea() == (int)Math.Round(Goal.transform.position.y))
        {
            GoalFlg = true;
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

        if (EnemyMoveCount > 0)
        {
            AtkEnemyList[count].GetComponent<ActionControllor>().SetUserAttackFlagOn();
            EnemyList[count].GetComponent<EnemyAttack>().AttackHit();
        }
    }
    IEnumerator coActionFlgOnMain()
    {
        //プレイヤー
        AcitonFlg = true;
        if (PatkFlg == true)
        {
            Player.GetComponent<ActionControllor>().SetUserAttackFlagOn();
            yield return new WaitForSeconds(0.3f);
            ResetAttkEnemyList();//攻撃で敵が消えた時のため
        }
        else
        {
            Player.GetComponent<ActionControllor>().SetUserActFlagOn();
            if (GoalFlg == true)
            {
                yield return new WaitForSeconds(0.3f);
                SaveData();
                SceneManager.LoadScene("GameScene");
            }
        }
        for (int count = 0; count < EnemyMoveCount; count++)
        {
            MoveEnemyList[count].GetComponent<ActionControllor>().SetUserActFlagOn();
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
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP);
            if (LockFlg != true) //移動中は入力無効にする
            {
                CheckBlockState();
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
            }
            else
            {
                LockFlg = true;
            }
        }
    }
    

}
