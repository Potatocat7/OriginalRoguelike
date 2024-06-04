using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class ActionControllor : MonoBehaviour {

    /// <summary>
    /// 向きの列挙型
    /// </summary>
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
    /// <summary>現在向いている方向</summary>
    [SerializeField]
    public Direction thisNowDirection;
    /// <summary>攻撃フラグ</summary>
    [SerializeField]
    private bool UserAttackFlg;
    /// <summary>攻撃エフェクト</summary>
    [SerializeField]
    private AtkEfScript atkEf = null;
    /// <summary>ステータス</summary>
    [SerializeField]
    public StatusDataScript stateData = null;
    /// <summary>敵用攻撃動作</summary>
    [SerializeField]
    public Attack enemyAtk;
    /// <summary>プレイヤー用攻撃動作</summary>
    [SerializeField]
    public Attack Attack;
    /// <summary>次の移動ポジション</summary>
    public int iThisNext { get; private set; }
    public int jThisNext { get; private set; }
    /// <summary>現在のポジション</summary>
    public int iThisNow { get; private set; }
    public int jThisNow { get; private set; }
    /// <summary>攻撃方向</summary>
    public int iAtkDir { get; private set; }
    public int jAtkDir { get; private set; }
    /// <summary>攻撃エフェクト発生フラグ</summary>
    public bool AtkEfFlg;
    /// <summary>アニメーション状態</summary>
    [SerializeField]
    private Animator AnimatorState;
    /// <summary>SP攻撃エフェクト</summary>
    [SerializeField] private SpAtkEF spAtkEF;
    /// <summary>初期状態の画像</summary>
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 
    /// </summary>
    public void Init( )
    {
        //生成時にthisでオブジェクトの情報を所得してmapの現座標を獲得しておく
        iThisNow = (int)Math.Round(this.transform.position.x);
        jThisNow = (int)Math.Round(this.transform.position.y);
        stateData.Init(setAnimator:(image,runtimeAnimatorController)=> 
        {
            spriteRenderer.sprite = image;
            AnimatorState.runtimeAnimatorController = runtimeAnimatorController;
        });
        stateData.SetThisPosition(iThisNow, jThisNow);
        UserAttackFlg = false;
        iThisNext = 0;
        jThisNext = 0;
        thisNowDirection = Direction.DOWN;
        AtkEfFlg = false;
        if (this.tag == "Player")
        {
            atkEf.EffectEnabled(false);
        }
    }
    public void InitSpAtkEF()
    {
        spAtkEF.Init();
    }

    /// <summary>
    /// 方角を設定
    /// </summary>
    /// <param name="thisDirection"></param>
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

    /// <summary>
    /// このオブジェクトの現在位置のチェック
    /// </summary>
    /// <param name="iCheckStep"></param>
    /// <param name="jCheckStep"></param>
    /// <returns></returns>
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

    /// <summary>
    /// このオブジェクトの移動予定位置のチェック
    /// </summary>
    /// <param name="iCheckStep"></param>
    /// <param name="jCheckStep"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 次の移動するマスが壁かどうかのチェック
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 次に移動するポジションを設定
    /// </summary>
    /// <param name="iNext"></param>
    /// <param name="jNext"></param>
    public void SetNextStep(int iNext, int jNext)
    {
        iThisNext = iNext;
        jThisNext = jNext;
    }

    /// <summary>
    /// 現在のポジション設定
    /// </summary>
    public void SetThisNowStep()
    {
        iThisNow = (int)Math.Round(this.transform.position.x);
        jThisNow = (int)Math.Round(this.transform.position.y);
    }

    /// <summary>
    /// 次に移動するポジションを返すi(x)
    /// </summary>
    /// <returns></returns>
    public int SetiNextStepArea()
    {
        return iThisNow + iThisNext;
    }

    /// <summary>
    /// 次に移動するポジションを返すj(y)
    /// </summary>
    /// <returns></returns>
    public int SetjNextStepArea()
    {
        return jThisNow + jThisNext;
    }

    /// <summary>
    /// 攻撃フラグオン
    /// </summary>
    public void SetUserAttackFlg()
    {
        UserAttackFlg = true;
    }

    /// <summary>
    /// 攻撃フラグを返す
    /// </summary>
    /// <returns></returns>
    public bool GetUserAttackFlg()
    {
        return UserAttackFlg ;
    }

    /// <summary>
    /// プレイヤーの攻撃フラグオン
    /// </summary>
    public void SetUserAttackFlagOn()
    {
        //UserActFlg = true;
        UserAttackFlg = true;
    }

    /// <summary>
    /// 移動モーション
    /// </summary>
    /// <param name="nextPosition"></param>
    /// <param name="token"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 移動モーション設定と呼び出し
    /// </summary>
    private async void coActionMove()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        await MoveAsync(new Vector3(iThisNow + iThisNext, jThisNow + jThisNext, -1), token);
        iThisNow = iThisNow + iThisNext;
        jThisNow = jThisNow + jThisNext;
        stateData.SetThisPosition(iThisNow, jThisNow);
        iThisNext = 0;
        jThisNext = 0;
    }

    /// <summary>
    /// 攻撃モーション
    /// </summary>
    /// <param name="attackPosition"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask AttackAsync(Vector3 attackPosition, CancellationToken token)
    {
        // 移動速度
        var moveSpeed = 5.0f;
        while (true)
        {
            // 座標の差分
            var deltaPosition = (attackPosition - this.transform.position);
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

    /// <summary>
    /// 攻撃モーション設定と呼び出し
    /// </summary>
    private async void coActionAttack()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        if (this.tag == "Player")
        {
            atkEf.SetEffecrDirection();
            atkEf.EffectEnabled(true);
        }
        //前へ出る
        await AttackAsync(new Vector3(iAtkDir + this.transform.position.x, jAtkDir + this.transform.position.y, -1), token);
        //戻る
        await AttackAsync(new Vector3(iThisNow, jThisNow, -1), token);
        if (this.tag == "Player")
        {
            atkEf.EffectEnabled(false);
        }
        UserAttackFlg = false;
    }
    
    /// <summary>
    /// SPモーション呼び出し
    /// </summary>
    private async void coSpActionAttack()
    {
        spAtkEF.PlayEffect(true);
        for (int count = 1; count < 11; count++)
        {
            await UniTask.Delay(150);
        }
        spAtkEF.PlayEffect(false);
        UserAttackFlg = false;
    }

    /// <summary>
    /// モーション分岐
    /// </summary>
    /// <param name="actionable"></param>
    /// <returns></returns>
    public async UniTask ActionStart(bool actionable)
    {
        if(actionable == true)
        {
            if (UserAttackFlg == true)
            {
                coActionAttack();
            }
            else
            {
                coActionMove();
            }
        }
    }

    /// <summary>
    /// SP攻撃モーション開始
    /// </summary>
    /// <returns></returns>
    public async UniTask SpActionStart()
    {
        coSpActionAttack();
    }
}
