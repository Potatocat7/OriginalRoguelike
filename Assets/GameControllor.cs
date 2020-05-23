using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int iNext, jNext;
    public int iRandom, jRandom;
    GameObject Player;
    GameObject Enemy;
    int timeCount;

    void SetEnemyMove()
    {
        //※敵動作については条件で複数パターンあるため現状は仮設定
        Enemy = GameObject.Find("EnemyPrefab(Clone)");
        bool checkRandom = true;
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        while (checkRandom == true)
        {
            iRandom = (int)Random.Range(-1, 2);
            jRandom = (int)Random.Range(-1, 2);
            if (Enemy.GetComponent<ActionControllor>().SetNextStep(iRandom, jRandom) == false)
            {

            }
            else
            {
                Enemy.GetComponent<ActionControllor>().SetUserActFlagOn();
                checkRandom = false;
            }

        }
    }
    void CheckBlockState()
    {
        Player = GameObject.Find("PlayerPrefab(Clone)");

        //static部分を修正予定
        //次に移動予定のマスが壁でないかのチェック
        if (Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext) == false)
        {

        }
        else
        {
            MapGenerator.iNow = MapGenerator.iNow + iNext;
            MapGenerator.jNow = MapGenerator.jNow + jNext;
            AcitonFlg = true;
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
        timeCount = 0;
    }

    // Update is called once per frame
    void Update () {

        if (AcitonFlg == true)//プレイヤーフェーズとエネミーフェーズを用意が必要
        {

            timeCount += 1;
            if (timeCount == 10)
            {
                AcitonFlg = false; //if文でattackフラグをみて解除するかきめると同時にエネミーの攻撃時の移動処理を呼ぶ
                timeCount = 0;
            }
            /*if (timeCount == 20)
            {
                timeCount = 0; //attckフラグを用意して攻撃時はこちらまで動かす
            }*/

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
        }
    }
    public void Push_ATTCK()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
            Player = GameObject.Find("PlayerPrefab(Clone)");
            Player.GetComponent<ActionControllor>().SetUserAttackFlagOn(iNext, jNext);
            Player.GetComponent<PlayerAttack_1>().AttackAreaSet();
            SetEnemyMove();
        }
    }
    public void Push_LOCK()
    {
        if (AcitonFlg != true) //移動中は入力無効にする
        {
        }
    }

}
