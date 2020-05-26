using System.Collections;
using System.Collections.Generic;
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
    public bool AtkCheckflg;//攻撃判定のフラグ
    bool EndPhase;
    int iNext, jNext;
    int iRandom, jRandom;
    GameObject Player;
    [SerializeField]
    GameObject Enemy;
    List<GameObject> EnemyList = new List<GameObject>();
    List<GameObject> AtkEnemy = new List<GameObject>();
    List<GameObject> MoveEnemy = new List<GameObject>();
    int timeCount;
    int EnemyCount;
    //確認用に宣言
    int iPmap, jPmap;
    int iEmap, jEmap;

    public void AftorMakeMapStart()
    {
        Player = GameObject.Find("PlayerPrefab(Clone)");
        Enemy = GameObject.Find("EnemyPrefab(Clone)");
        //EnemyList = GetComponent<MapGenerator>().EnemyList;
        //EnemyCount = MapGenerator.EnemyCount;
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
    void EnemyMoveRandom(GameObject Enemy)
    {
        bool checkRandom = true;
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        while (checkRandom == true)
        {
            iRandom = (int)UnityEngine.Random.Range(-1, 2);
            jRandom = (int)UnityEngine.Random.Range(-1, 2);
            Enemy.GetComponent<ActionControllor>().SetNextStep(iRandom, jRandom);
            if (Enemy.GetComponent<ActionControllor>().CheckNextStep() == false)
            {
            }
            else
            {
                SetEnemyDirection(iRandom, jRandom, Enemy);
                Enemy.GetComponent<ActionControllor>().SetUserActFlagOn();
                checkRandom = false;
            }

        }
    }
    void EnemyMoveTargetPlayer(GameObject Enemy)
    {
        int iEnemyNext, jEnemyNext;

        if (Player.GetComponent<ActionControllor>().SetiNextStepArea() - (int)Math.Round(Enemy.transform.position.x) > 0)
        {
            iEnemyNext = 1;
        }
        else if(Player.GetComponent<ActionControllor>().SetiNextStepArea() - (int)Math.Round(Enemy.transform.position.x) < 0)
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
        if (Enemy.GetComponent<ActionControllor>().CheckNextStep() == false)
        {
            SetEnemyDirection(iEnemyNext, jEnemyNext, Enemy);
            iEnemyNext = 0;
            jEnemyNext = 0;
            Enemy.GetComponent<ActionControllor>().SetNextStep(iEnemyNext, jEnemyNext);
        }
        else
        {
            SetEnemyDirection(iEnemyNext, jEnemyNext, Enemy);
            Enemy.GetComponent<ActionControllor>().SetUserActFlagOn();
        }
    }
    void SetEnemyMove()
    {
        //for (int count = 0; count < EnemyCount; count++)
        //{
            if (MapGenerator.EnemyCount >= 1) {
                //※敵動作については条件で複数パターンあるため現状は仮設定
                Enemy = GameObject.Find("EnemyPrefab(Clone)");
                //現状プレイヤーの移動前の情報を所得しているので移動先の情報にする必要あり
                //確認用に宣言中　現在Playerの位置情報が移動前の位置情報を所得している※positionが少数点で0.999になると切り捨てになってしまう
                iEmap = (int)Math.Round(Enemy.transform.position.x);
                //iEmap = (int)Math.Round(EnemyList[count].transform.position.x);
                iPmap = Player.GetComponent<ActionControllor>().SetiNextStepArea();
                jEmap = (int)Math.Round(Enemy.transform.position.y);
                //jEmap = (int)Math.Round(EnemyList[count].transform.position.y);
                jPmap = Player.GetComponent<ActionControllor>().SetjNextStepArea();

                if (Enemy.GetComponent<EnemyAttack>().CheckPlayerThisAround(iPmap, jPmap, iEmap, jEmap) == true)//各敵の周囲(3*3)にプレイヤーがいるかチェックし居たらそちらに方向を切り替えて攻撃動作をセット
                //if (EnemyList[count].GetComponent<EnemyAttack>().CheckPlayerThisAround(iPmap, jPmap, iEmap, jEmap) == true)//各敵の周囲(3*3)にプレイヤーがいるかチェックし居たらそちらに方向を切り替えて攻撃動作をセット
                {//周囲を調べてプレイヤーがいた場合方向だけセットしておく
                Enemy.GetComponent<ActionControllor>().SetUserAttackFlagOn();
                    //EnemyList[count].GetComponent<ActionControllor>().SetUserAttackFlagOn();
                    Enemy.GetComponent<EnemyAttack>().AttackHit();
                    //EnemyList[count].GetComponent<EnemyAttack>().AttackHit();
                }
                else
                {
                    if (MapGenerator.map[(int)Math.Round(Enemy.transform.position.x), (int)Math.Round(Enemy.transform.position.y)] == 0)
                    //if (MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x), (int)Math.Round(EnemyList[count].transform.position.y)] == 0)
                    {//通路だった場合は
                        EnemyMoveRandom(Enemy); //現状はランダム移動（後で通路は直進するようにしたい）
                        //EnemyMoveRandom(EnemyList[count]); //現状はランダム移動（後で通路は直進するようにしたい）
                    }
                    else
                    {
                    if (MapGenerator.map[(int)Math.Round(Player.transform.position.x), (int)Math.Round(Player.transform.position.y)] == MapGenerator.map[(int)Math.Round(Enemy.transform.position.x), (int)Math.Round(Enemy.transform.position.y)])
                    //if (MapGenerator.map[(int)Math.Round(Player.transform.position.x), (int)Math.Round(Player.transform.position.y)] == MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x), (int)Math.Round(EnemyList[count].transform.position.y)])
                    {
                        EnemyMoveTargetPlayer(Enemy);
                            //EnemyMoveTargetPlayer(EnemyList[count]);
                        }
                        else
                        {
                            EnemyMoveRandom(Enemy);
                            //EnemyMoveRandom(EnemyList[count]);
                        }
                    }
                }
            }
        //}
    }
    void CheckBlockState()
    {
        Player = GameObject.Find("PlayerPrefab(Clone)");
        Enemy = GameObject.Find("EnemyPrefab(Clone)");

        //次に移動予定のマスが壁でないかのチェック
        if (Player.GetComponent<ActionControllor>().CheckNextStep() == false)
        {
            iNext = 0;
            jNext = 0;
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext); 
        }
        else
        {
            //リスト化した時に修正忘れないように
            if (Enemy.GetComponent<ActionControllor>().CheckNowStep(Player.GetComponent<ActionControllor>().SetiNextStepArea(), Player.GetComponent<ActionControllor>().SetjNextStepArea()) == true) 
            {
                iNext = 0;
                jNext = 0;
                Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            }
            else
            {
                AcitonFlg = true;
                Player.GetComponent<ActionControllor>().SetUserActFlagOn();
                //※敵オブジェクトは0になることもあるため複数対応+無しのときの対応も必要
                SetEnemyMove();
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
        timeCount = 0;
        EndPhase = false;
        AtkCheckflg = false;
    }

    // Update is called once per frame
    void Update () {

        if (AcitonFlg == true)//プレイヤーフェーズとエネミーフェーズを用意が必要
        {

            timeCount += 1;
            if (timeCount == 10)
            {
                if (PatkFlg == true)
                {
                    SetEnemyMove();
                }
                else
                {
                    EndPhase = true;
                    AcitonFlg = false; //if文でattackフラグをみて解除するかきめると同時にエネミーの攻撃時の移動処理を呼ぶ
                    timeCount = 0;
                }
            }
            if (timeCount == 20)//敵が複数いた場合順々に攻撃してもらう処理が必要？（現在だと敵の攻撃は同時になる）
            {
                EndPhase = true;
                PatkFlg = false;
                AcitonFlg = false;
                timeCount = 0; //attckフラグを用意して攻撃時はこちらまで動かす
            }

            if (EndPhase == true)
            {
                EndPhase = false;
            }
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
            CheckBlockState();
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
            CheckBlockState();
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
            CheckBlockState();
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
            CheckBlockState();
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
            CheckBlockState();
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
            CheckBlockState();
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
            CheckBlockState();
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
            CheckBlockState();
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
            Player.GetComponent<ActionControllor>().SetUserAttackFlagOn();
            AtkCheckflg = true;
            //Player.GetComponent<PlayerAttack_1>().AttackHit();
            AcitonFlg = true;
            PatkFlg = true;
        }
    }
    public void Push_LOCK()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
        }
    }
    

}
