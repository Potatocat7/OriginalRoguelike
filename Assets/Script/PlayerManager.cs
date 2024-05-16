using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerManager : MonoBehaviour
{
    private ActionControllor Player = null;
    private StatusDataScript _playerState = null;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(ActionControllor player, StatusDataScript status)
    {
        Player = player;
        _playerState = status;
        ItemWindowScript.Instance.SetupItemState();
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
        await Player.ActionStart();
    }

    //*_playerState *//
    public int GetPlayerHpNow()
    {
        return _playerState.GetHPnow();
    }
    public Status GetStateData()
    {
        return _playerState.GetStateData();
    }
    public int GetSPcount()
    {
        return _playerState.GetSPcount();
    }
    public void SetSPcount(int cnt)
    {
        _playerState.SetSPcount(cnt);
    }

    public void AddItemState(ItemStatusData data)
    {
        _playerState.AddState(data);
    }
    public void SubItemState(ItemStatusData data)
    {
        _playerState.SubState(data);
    }
}
