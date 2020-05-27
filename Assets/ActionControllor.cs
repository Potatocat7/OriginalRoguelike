using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ActionControllor : MonoBehaviour {

    public enum Direction
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
    [SerializeField]
    public Direction thisNowDirection;
    [SerializeField]
    bool UserActFlg;
    [SerializeField]
    bool UserAttackFlg;
    int count;
    [SerializeField]
    int aaa, bbb;
    [SerializeField]
    int ccc, ddd;
    public int iThisNext { get; private set; }
    public int jThisNext { get; private set; }
    public int iThisNow { get; private set; }
    public int jThisNow { get; private set; }
    public int iAtkDir { get; private set; }
    public int jAtkDir { get; private set; }

    public bool AtkEfFlg;
    [SerializeField]
    Animator AnimatorState;

    public void StartSetUp( )
    {
        //Debug.Log(this);
        //生成時にthisでオブジェクトの情報を所得してmapの現座標を獲得しておく
        iThisNow = (int)Math.Round(this.transform.position.x);
        jThisNow = (int)Math.Round(this.transform.position.y);
        aaa = (int)Math.Round(this.transform.position.x);
        bbb = (int)Math.Round(this.transform.position.y);

        count = 0;
        UserActFlg = false;
        UserAttackFlg = false;
        iThisNext = 0;
        jThisNext = 0;
        AnimatorState = this.GetComponent<Animator>();
        thisNowDirection = Direction.DOWN;
        AtkEfFlg = false;
    }
    public void SetDirection(Direction thisDirection)
    {
        switch (thisDirection)
        {
            //enumクラス名.文字列でアクセスする
            case Direction.UP:
                AnimatorState.Play("UP");
                thisNowDirection = Direction.UP;
                iAtkDir = 0;
                jAtkDir = 1;
                break;
            case Direction.UP_LEFT:
                AnimatorState.Play("UP_LEFT");
                thisNowDirection = Direction.UP_LEFT;
                iAtkDir = -1;
                jAtkDir = 1;
                break;
            case Direction.UP_RIGHT:
                AnimatorState.Play("UP_RIGHT");
                thisNowDirection = Direction.UP_RIGHT;
                iAtkDir = 1;
                jAtkDir = 1;
                break;
            case Direction.LEFT:
                AnimatorState.Play("LEFT");
                thisNowDirection = Direction.LEFT;
                iAtkDir = -1;
                jAtkDir = 0;
                break;
            case Direction.RIGHT:
                AnimatorState.Play("RIGHT");
                thisNowDirection = Direction.RIGHT;
                iAtkDir = 1;
                jAtkDir = 0;
                break;
            case Direction.DOWN:
                AnimatorState.Play("DOWN");
                thisNowDirection = Direction.DOWN;
                iAtkDir = 0;
                jAtkDir = -1;
                break;
            case Direction.DOWN_LEFT:
                AnimatorState.Play("DOWN_LEFT");
                thisNowDirection = Direction.DOWN_LEFT;
                iAtkDir = -1;
                jAtkDir = -1;
                break;
            case Direction.DOWN_RIGHT:
                AnimatorState.Play("DOWN_RIGHT");
                thisNowDirection = Direction.DOWN_RIGHT;
                iAtkDir = 1;
                jAtkDir = -1;
                break;
        }
    }
    //このオブジェクトの現在位置のチェック
    public bool CheckNowStep(int iCheckStep, int jCheckStep)
    {
        if (iThisNow == iCheckStep && jThisNow == jCheckStep)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //このオブジェクトの移動予定位置のチェック
    public bool CheckNextStep(int iCheckStep, int jCheckStep)
    {
        if (iThisNow + iThisNext == iCheckStep && jThisNow + jThisNext == jCheckStep)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //次の移動するマスが壁かどうかのチェック
    public bool CheckNextStepWall()
    {
        if (MapGenerator.map[iThisNow + iThisNext, jThisNow + jThisNext] == 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void SetNextStep(int iNext, int jNext)
    {
        iThisNext = iNext;
        jThisNext = jNext;
    }
    public void SetThisNowStep()
    {
        iThisNow = (int)Math.Round(this.transform.position.x);
        jThisNow = (int)Math.Round(this.transform.position.y);
    }

    // Use this for initialization
    void Start () {
    }
    public void SetUserActFlagOn()
    {
        count = 0;
        UserActFlg = true;
    }
    public int SetiNextStepArea()
    {
        return iThisNow + iThisNext;
    }
    public int SetjNextStepArea()
    {
        return jThisNow + jThisNext;
    }
    public void SetUserAttackFlagOn()
    {
        count = 0;
        UserActFlg = true;
        UserAttackFlg = true;
    }
    //    public void SetUserActFlagOff()
    //    {
    //        count = 0;
    //        UserActFlg = false;
    //    }
    //


    // Update is called once per frame
    void Update () {
        aaa = iThisNow;
        bbb = jThisNow;
        ccc = iThisNext;
        ddd = jThisNext;
        //アクション動作で攻撃と移動をここで処理
        if (UserActFlg == true)
        {
            if (UserAttackFlg == true)
            {
                //10Fかけて攻撃動作
                if (count < 5 )
                {
                    this.transform.Translate(iAtkDir * 0.1f, jAtkDir * 0.1f, 0);
                    count += 1;
                    AtkEfFlg = true;
                }
                else
                {
                    this.transform.Translate(iAtkDir * -0.1f, jAtkDir * -0.1f, 0);
                    count += 1;
                    AtkEfFlg = false;
                }
                if (count == 10)
                {
                    UserAttackFlg = false;
                    UserActFlg = false;
                    count = 0;
                }

            }
            else
            {
                //10Fかけて次のマスに移動
                this.transform.Translate(iThisNext * 0.1f, jThisNext * 0.1f, 0);
                count += 1;

                if (count == 10)
                {
                    UserActFlg = false;
                    count = 0;
                    iThisNow = iThisNow + iThisNext;
                    jThisNow = jThisNow + jThisNext;
                    iThisNext = 0 ;
                    jThisNext = 0 ;
                }
            }
        }		
	}
}
