using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

public class PlayerManager : MonoBehaviour
{
    /// <summary>プレイヤーのアクションコントローラー</summary>
    private ActionControllor player = null;
    /// <summary>SPアイテム取得フラグ</summary>
    private bool getPItemFlg;
    /// <summary>攻撃可能かのチェックフラグ</summary>
    private bool actionable;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="player"></param>
    /// <param name="unableToFight"></param>
    public void Init(ActionControllor actioncontrollor,Action unableToFight)
    {
        player = actioncontrollor;
        player.Init();
        player.InitSpAtkEF();
        SetDirection(ActionControllor.Direction.DOWN);
        getPItemFlg = false;
        actionable = true;
        Observable.EveryUpdate()
        .Where(_ => player.stateData.GetHPnow() <= 0)
        .Subscribe(_ =>
        {
            unableToFight.Invoke();
            actionable = false;
        }).AddTo(this);
    }

    //*Player *//
    /// <summary>
    /// プレイヤーの向きと位置の設定
    /// </summary>
    /// <param name="inext"></param>
    /// <param name="jnext"></param>
    /// <param name="direction"></param>
    public void SetPlayerAction(int inext,int jnext, ActionControllor.Direction direction)
    {
        player.SetNextStep(inext, jnext);
        player.SetDirection(direction);
    }

    /// <summary>
    /// プレイヤーの向きを設定
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(ActionControllor.Direction direction)
    {
        player.SetDirection(direction);
    }

    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    /// <param name="inext"></param>
    /// <param name="jnext"></param>
    public void SetPlayerAction(int inext, int jnext)
    {
        player.SetNextStep(inext, jnext);
        player.SetThisNowStep();
    }

    /// <summary>
    /// プレイヤーの移動予定位置情報を返す
    /// </summary>
    /// <returns></returns>
    public (int,int) GetNextStepArea()
    {
        return(player.SetiNextStepArea(), player.SetjNextStepArea());
    }

    /// <summary>
    /// プレイヤーの位置を返す
    /// </summary>
    /// <returns></returns>
    public Transform GetTransform()
    {
        return player.transform;
    }

    /// <summary>
    /// 次に移動する場所が壁かどうかのチェック
    /// </summary>
    /// <returns></returns>
    public bool CheckNextStepWall()
    {
        return player.CheckNextStepWall();
    }

    /// <summary>
    /// 次の位置を設定
    /// </summary>
    /// <param name="inext"></param>
    /// <param name="jnext"></param>
    public void SetNextStep(int inext, int jnext)
    {
        player.SetNextStep(inext, jnext);
    }

    /// <summary>
    /// 攻撃フラグオン
    /// </summary>
    public void SetUserAttackFlagOn()
    {
        player.SetUserAttackFlagOn();
    }

    /// <summary>
    /// SP攻撃フラグ開始
    /// </summary>
    /// <returns></returns>
    public async UniTask SpActionStart()
    {
        await player.SpActionStart();
    }

    /// <summary>
    /// 攻撃処理開始
    /// </summary>
    /// <returns></returns>
    public async UniTask ActionStart()
    {
        await player.ActionStart(actionable);
        if (player.stateData.GetHPnow() <= 0)
        {
            player.stateData.SetEndingFlg();
        }
    }

    /// <summary>
    /// プレイヤーのHPチェック
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerHP()
    {
        if (player.stateData.GetHPnow() <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// プレイヤーのステータスを返す
    /// </summary>
    /// <returns></returns>
    public Status GetStateData()
    {
        return player.stateData.GetStateData();
    }

    /// <summary>
    /// 残りのSP攻撃回数を返す
    /// </summary>
    /// <returns></returns>
    public int GetSPcount()
    {
        return player.stateData.GetSPcount();
    }

    /// <summary>
    /// SP攻撃回数の設定
    /// </summary>
    /// <param name="cnt"></param>
    public void SetSPcount(int cnt)
    {
        player.stateData.SetSPcount(cnt);
        GameManager.Instance.UpdateDisplay();
    }

    /// <summary>
    /// 装備アイテムステータスを追加
    /// </summary>
    /// <param name="data"></param>
    public void AddItemState(ItemStatusData data)
    {
        player.stateData.AddState(data);
    }

    /// <summary>
    /// 装備アイテムを外した時の減算
    /// </summary>
    /// <param name="data"></param>
    public void SubItemState(ItemStatusData data)
    {
        player.stateData.SubState(data);
    }

    /// <summary>
    /// SPアイテム取得フラグ変更
    /// </summary>
    /// <param name="flg"></param>
    public void SetPItemFlg(bool flg)
    {
        getPItemFlg = flg;
    }

    /// <summary>
    /// SPアイテムフラグを返す
    /// </summary>
    /// <returns></returns>
    public bool GetPowerItemFlg()
    {
        return getPItemFlg;
    }

    /// <summary>
    /// 攻撃処理開始
    /// </summary>
    /// <param name="atkflg"></param>
    /// <param name="spflg"></param>
    /// <param name="afterFlg"></param>
    public void StartAttack(bool atkflg ,bool spflg, Action<bool> afterFlg = null)
    {
        player.Attack.StartAttack(atkflg, spflg,(changeAtkCheckflg) => 
        {
            afterFlg.Invoke(changeAtkCheckflg);
        });
    }
}
