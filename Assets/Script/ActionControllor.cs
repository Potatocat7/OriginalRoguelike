using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

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
    //[SerializeField]
    //private ActionControllor ActContollor;
    [SerializeField]
    public Direction thisNowDirection;
    [SerializeField]
    private bool UserActFlg;
    [SerializeField]
    private bool UserAttackFlg;
    [SerializeField]
    private AtkEfScript atkEf = null;
    [SerializeField]
    public StatusDataScript stateData = null;
    [SerializeField]
    public EnemyAttack enemyAtk;

    private int count;
    public int iThisNext { get; private set; }
    public int jThisNext { get; private set; }
    public int iThisNow { get; private set; }
    public int jThisNow { get; private set; }
    public int iAtkDir { get; private set; }
    public int jAtkDir { get; private set; }

    public bool AtkEfFlg;
    public bool SpAtkEfFlg;
    [SerializeField]
    private Animator AnimatorState;

    //public void SetGameCtrl(GameControllor ctrl)
    //{
    //    Contollor = ctrl;
    //}
    //public GameControllor GetGameCtrl( )
    //{
    //    return Contollor;
    //}
    public void StartSetUp( )
    {
        //Debug.Log(this);
        //生成時にthisでオブジェクトの情報を所得してmapの現座標を獲得しておく
        iThisNow = (int)Math.Round(this.transform.position.x);
        jThisNow = (int)Math.Round(this.transform.position.y);

        count = 0;
        UserActFlg = false;
        UserAttackFlg = false;
        iThisNext = 0;
        jThisNext = 0;
        //AnimatorState = this.GetComponent<Animator>();
        thisNowDirection = Direction.DOWN;
        AtkEfFlg = false;
        SpAtkEfFlg = false;
        if (this.tag == "Player")
        {
            atkEf.EffectEnabled(false);
        }
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
    public void SetUserAttackFlg()
    {
        UserAttackFlg = true;
    }
    public bool GetUserAttackFlg()
    {
        return UserAttackFlg ;
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

    private async UniTask MoveAsync(Vector3 nextPosition, CancellationToken token)
    {
        // 移動速度
        var moveSpeed = 5.0f;
        while (true)
        {
            // 座標の差分
            var deltaPosition = (nextPosition - this.transform.position);

            // 0.1m以内に近づいていたら終了
            if (deltaPosition.magnitude < 0.1f) return;

            // 移動方向
            var direction = deltaPosition.normalized;

            // 移動させる
            this.transform.position += direction * moveSpeed * Time.deltaTime;

            // 1F待つ
            await UniTask.Yield(token);
        }
    }
    //IEnumerator coActionMove()
    private async void coActionMove()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        await MoveAsync(new Vector3(iThisNow + iThisNext, jThisNow + jThisNext, -1), token);
        //for (int count = 1; count < 11; count++)
        //{
        //    this.transform.Translate(iThisNext * 0.1f, jThisNext *  0.1f, 0);
        //    //yield return new WaitForSeconds(0.01f);
        //    await UniTask.Delay(10);
        //}
        UserActFlg = false;
        count = 0;
        iThisNow = iThisNow + iThisNext;
        jThisNow = jThisNow + jThisNext;
        stateData.SetThisPosition(iThisNow, jThisNow);
        iThisNext = 0;
        jThisNext = 0;
        
    }
    //IEnumerator coActionAttack()
    private async void coActionAttack()
    {
        //AtkEfFlg = true;
        if (this.tag == "Player")
        {
            atkEf.SetEffecrDirection();
            atkEf.EffectEnabled(true);
        }
        for (int count = 1; count < 6; count++)
        {
            //if (GameManager.Instance.GetPlayerManager().GetPlayerHpNow() <= 0)
            //{
            //    break;
            //}
            this.transform.Translate(iAtkDir * 0.1f, jAtkDir * 0.1f, 0);
            //yield return new WaitForSeconds(0.025f);
            await UniTask.Delay(25);
        }
        ///エラーがでるのでここでもチェック
        ///TODO:managerでフラグをもって、フラグがたったらActionノフラグを立てる
        //if (GameManager.Instance.GetPlayerManager().GetPlayerHpNow() <= 0)
        //{
        //    return;
        //}
        //AtkEfFlg = false;
        if (this.tag == "Player")
        {
            atkEf.EffectEnabled(false);
        }
        for (int count = 1; count < 6; count++)
        {
            //if (GameManager.Instance.GetPlayerManager().GetPlayerHpNow() <= 0)
            //{
            //    break;
            //}
            this.transform.Translate(iAtkDir * -0.1f, jAtkDir * -0.1f, 0);
            //yield return new WaitForSeconds(0.025f);
            await UniTask.Delay(25);
        }
        UserAttackFlg = false;
        UserActFlg = false;
        count = 0;
    }
    //IEnumerator coSpActionAttack()
    private async void coSpActionAttack()
    {
        SpAtkEfFlg = true;
        for (int count = 1; count < 11; count++)
        {
            //yield return new WaitForSeconds(0.15f);
            await UniTask.Delay(150);
        }
        SpAtkEfFlg = false;
        UserAttackFlg = false;
        UserActFlg = false;
        count = 0;
    }
    public async UniTask ActionStart(bool actionable)
    {
        if(actionable == true)
        {
            if (UserAttackFlg == true)
            {
                coActionAttack();
                //StartCoroutine("coActionAttack");
            }
            else
            {
                coActionMove();
                //StartCoroutine("coActionMove");
            }
        }
    }
    public async UniTask SpActionStart()
    {
        coSpActionAttack();
        //StartCoroutine("coSpActionAttack");
    }
    // Update is called once per frame
    void Update () {
	}
}
