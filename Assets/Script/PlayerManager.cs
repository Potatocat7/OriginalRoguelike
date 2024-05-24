using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

public class PlayerManager : MonoBehaviour
{
    private ActionControllor Player = null;
    private bool GetPItemFlg;
    private bool actionable;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(ActionControllor player,Action unableToFight)
    {
        Player = player;
        Player.StartSetUp();
        SetDirection(ActionControllor.Direction.DOWN);
        GetPItemFlg = false;
        actionable = true;
        Observable.EveryUpdate()
        .Where(_ => Player.stateData.GetHPnow() <= 0)
        .Subscribe(_ =>
        {
            unableToFight.Invoke();
            actionable = false;
        }).AddTo(this);
    }
    //*Player *//
    public void SetPlayerAction(int inext,int jnext, ActionControllor.Direction direction)
    {
        Player.SetNextStep(inext, jnext);
        Player.SetDirection(direction);
    }
    public void SetDirection(ActionControllor.Direction direction)
    {
        Player.SetDirection(direction);
    }

    public void SetPlayerAction(int inext, int jnext)
    {
        Player.SetNextStep(inext, jnext);
        Player.SetThisNowStep();
    }
    public (int,int) GetNextStepArea()
    {
        return(Player.SetiNextStepArea(), Player.SetjNextStepArea());
    }
    public Transform GetTransform()
    {
        return Player.transform;
    }
    public bool CheckNextStepWall()
    {
        return Player.CheckNextStepWall();
    }
    public void SetNextStep(int inext, int jnext)
    {
        Player.SetNextStep(inext, jnext);
    }
    ///TODO:プレイヤー専用行動はActionControllorから分ける？
    public void SetUserAttackFlagOn()
    {
        Player.SetUserAttackFlagOn();
    }
    public void SetUserActFlagOn()
    {
        Player.SetUserActFlagOn();
    }
    public async UniTask SpActionStart()
    {
        await Player.SpActionStart();
    }
    public async UniTask ActionStart()
    {
        await Player.ActionStart(actionable);
        if (Player.stateData.GetHPnow() <= 0)
        {
            Player.stateData.SetEndingFlg();
        }

    }
    public bool CheckPlayerHP()
    {
        if (Player.stateData.GetHPnow() <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public Status GetStateData()
    {
        return Player.stateData.GetStateData();
    }
    public int GetSPcount()
    {
        return Player.stateData.GetSPcount();
    }
    public void SetSPcount(int cnt)
    {
        Player.stateData.SetSPcount(cnt);
    }

    public void AddItemState(ItemStatusData data)
    {
        Player.stateData.AddState(data);
    }
    public void SubItemState(ItemStatusData data)
    {
        Player.stateData.SubState(data);
    }
    public void SetPItemFlg(bool flg)
    {
        GetPItemFlg = flg;
    }
    public bool GetPowerItemFlg()
    {
        return GetPItemFlg;
    }
    public void StartAttack(bool atkflg ,bool spflg, Action<bool> afterFlg = null)
    {
        Player.Attack.StartAttack(atkflg, spflg,(changeAtkCheckflg) => 
        {
            afterFlg.Invoke(changeAtkCheckflg);
        });
    }


}
