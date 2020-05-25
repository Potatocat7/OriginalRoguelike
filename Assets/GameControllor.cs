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
    int iNext, jNext;
    int iRandom, jRandom;
    GameObject Player;
    GameObject Enemy;
    int timeCount;
    //確認用に宣言
    int iPmap, jPmap;
    int iEmap, jEmap;


    void SetEnemyDirection(int iStep,int jStep)
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
    void EnemyMoveRandom()
    {
        bool checkRandom = true;
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        while (checkRandom == true)
        {
            iRandom = (int)UnityEngine.Random.Range(-1, 2);
            jRandom = (int)UnityEngine.Random.Range(-1, 2);
            if (Enemy.GetComponent<ActionControllor>().CheckNextStep() == false)
            {

            }
            else
            {
                SetEnemyDirection(iRandom, jRandom);
                Enemy.GetComponent<ActionControllor>().SetNextStep(iRandom, jRandom);
                Enemy.GetComponent<ActionControllor>().SetUserActFlagOn();
                checkRandom = false;
            }

        }
    }
    void EnemyMoveTargetPlayer()
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

        if (Enemy.GetComponent<ActionControllor>().CheckNextStep() == false)
        {

        }
        else
        {
            Enemy.GetComponent<ActionControllor>().SetNextStep(iEnemyNext, jEnemyNext);
            SetEnemyDirection(iEnemyNext, jEnemyNext);
            Enemy.GetComponent<ActionControllor>().SetUserActFlagOn();
        }
    }
    void SetEnemyMove()
    {
        if (MapGenerator.EnemyCount >= 1) {
            //※敵動作については条件で複数パターンあるため現状は仮設定
            Enemy = GameObject.Find("EnemyPrefab(Clone)");
            //現状プレイヤーの移動前の情報を所得しているので移動先の情報にする必要あり
            //確認用に宣言中　現在Playerの位置情報が移動前の位置情報を所得している※positionが少数点で0.999になると切り捨てになってしまう
            iEmap = (int)Math.Round(Enemy.transform.position.x);
            iPmap = Player.GetComponent<ActionControllor>().SetiNextStepArea();
            jEmap =(int)Math.Round(Enemy.transform.position.y);
            jPmap = Player.GetComponent<ActionControllor>().SetjNextStepArea();

            if (Enemy.GetComponent< EnemyAttack >().CheckPlayerThisAround(iPmap, jPmap, iEmap, jEmap) == true)//各敵の周囲(3*3)にプレイヤーがいるかチェックし居たらそちらに方向を切り替えて攻撃動作をセット
            {//周囲を調べてプレイヤーがいた場合方向だけセットしておく
                //Enemy.GetComponent<ActionControllor>().SetUserAttackFlagOn(iStep, jStep); 引数があって呼び出せないので引数を使った処理と分割する必要あり
                Enemy.GetComponent<EnemyAttack>().AttackHit();
            }
            else
            {
                if (MapGenerator.map[(int)Math.Round(Enemy.transform.position.x), (int)Math.Round(Enemy.transform.position.y)] == 0)
                {//通路だった場合は
                    EnemyMoveRandom(); //現状はランダム移動（後で通路は直進するようにしたい）
                }
                else
                {
                    if (MapGenerator.map[(int)Math.Round(Player.transform.position.x), (int)Math.Round(Player.transform.position.y)] == MapGenerator.map[(int)Math.Round(Enemy.transform.position.x), (int)Math.Round(Enemy.transform.position.y)])
                    {
                        EnemyMoveTargetPlayer();
                    }
                    else
                    {
                        EnemyMoveRandom();
                    }
                }
            }
        }
    }
    void CheckBlockState()
    {
        Player = GameObject.Find("PlayerPrefab(Clone)");

        //static部分を修正予定
        //次に移動予定のマスが壁でないかのチェック
        if (Player.GetComponent<ActionControllor>().CheckNextStep() == false)
        {
            iNext = 0;
            jNext = 0;
        }
        else
        {
            MapGenerator.iNow = MapGenerator.iNow + iNext;
            MapGenerator.jNow = MapGenerator.jNow + jNext;
            AcitonFlg = true;
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetUserActFlagOn();
            //※敵オブジェクトは0になることもあるため複数対応+無しのときの対応も必要
            SetEnemyMove();
        }


    }

    // Use this for initialization
    void Start () {
        iNext = 0;
        jNext = 1;
        AcitonFlg = false;
        PatkFlg = false; 
        timeCount = 0;
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
                    AcitonFlg = false; //if文でattackフラグをみて解除するかきめると同時にエネミーの攻撃時の移動処理を呼ぶ
                    timeCount = 0;
                }
            }
            if (timeCount == 20)//敵が複数いた場合順々に攻撃してもらう処理が必要？（現在だと敵の攻撃は同時になる）
            {
                PatkFlg = false;
                AcitonFlg = false;
                timeCount = 0; //attckフラグを用意して攻撃時はこちらまで動かす
            }

        }

    }

    public void Push_U()
    { 
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = 1;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_U_L()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = -1;
            jNext = 1;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_LEFT);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_U_R()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 1;
            jNext = 1;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_RIGHT);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_D()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = -1;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_D_L()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = -1;
            jNext = -1;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_LEFT);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_D_R()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 1;
            jNext = -1;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_RIGHT);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_L()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = -1;
            jNext = 0;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.LEFT);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_R()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 1;
            jNext = 0;
            CheckBlockState();
            Player.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.RIGHT);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
        }
    }
    public void Push_ATTCK()
    {
        Player = GameObject.Find("PlayerPrefab(Clone)");
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            iNext = 0;
            jNext = 0;
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            Player.GetComponent<ActionControllor>().SetUserAttackFlagOn();
            Player.GetComponent<PlayerAttack_1>().AttackHit();
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
