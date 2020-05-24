using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Direction thisNowDirection;
    bool UserActFlg;
    [SerializeField]
    bool UserAttackFlg;
    int count;
    [SerializeField]
    int iThisNext, jThisNext, iThisNow, jThisNow;
    [SerializeField]
    Animator AnimatorState;


    public void SetDirection(Direction thisDirection)
    {
        switch (thisDirection)
        {
            //enumクラス名.文字列でアクセスする
            case Direction.UP:
                AnimatorState.Play("UP");
                thisNowDirection = Direction.UP;
                break;
            case Direction.UP_LEFT:
                AnimatorState.Play("UP_LEFT");
                thisNowDirection = Direction.UP_LEFT;
                break;
            case Direction.UP_RIGHT:
                AnimatorState.Play("UP_RIGHT");
                thisNowDirection = Direction.UP_RIGHT;
                break;
            case Direction.LEFT:
                AnimatorState.Play("LEFT");
                thisNowDirection = Direction.LEFT;
                break;
            case Direction.RIGHT:
                AnimatorState.Play("RIGHT");
                thisNowDirection = Direction.RIGHT;
                break;
            case Direction.DOWN:
                AnimatorState.Play("DOWN");
                thisNowDirection = Direction.DOWN;
                break;
            case Direction.DOWN_LEFT:
                AnimatorState.Play("DOWN_LEFT");
                thisNowDirection = Direction.DOWN_LEFT;
                break;
            case Direction.DOWN_RIGHT:
                AnimatorState.Play("DOWN_RIGHT");
                thisNowDirection = Direction.DOWN_RIGHT;
                break;
        }
    }
    //次の移動するマスの
    public bool SetNextStep(int iNext, int jNext)
    {
        if (MapGenerator.map[iThisNow + iNext, jThisNow + jNext] == 1)
        {
            return false;
        }
        else
        {
            iThisNext = iNext;
            jThisNext = jNext;
            return true;
        }
    }
    // Use this for initialization
    void Start () {
        //生成時にthisでオブジェクトの情報を所得してmapの現座標を獲得しておく
        iThisNow = (int)this.transform.position.x;
        jThisNow = (int)this.transform.position.y;
        count = 0;
        UserActFlg = false;
        UserAttackFlg = false;
        iThisNext = 0;
        jThisNext = 1;
        AnimatorState = this.GetComponent<Animator>();
        thisNowDirection = Direction.DOWN;
    }
    public void SetUserActFlagOn()
    {
        count = 0;
        UserActFlg = true;
    }
    public void SetUserAttackFlagOn(int iNext, int jNext)
    {
        iThisNext = iNext;
        jThisNext = jNext;
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
        //アクション動作で攻撃と移動をここで処理
        //if (GameCtlObj.GetComponent<GameControllor>().AcitonFlg == true)
        if(UserActFlg == true)
        {
            if (UserAttackFlg == true)
            {
                //10Fかけて攻撃動作
                if (count < 5 )
                {
                    this.transform.Translate(iThisNext * 0.1f, jThisNext * 0.1f, 0);
                    count += 1;
                }
                else
                {
                    this.transform.Translate(iThisNext * -0.1f, jThisNext * -0.1f, 0);
                    count += 1;
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
                }
            }
        }		
	}
}
