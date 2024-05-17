using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerManager : MonoBehaviour
{
    private ActionControllor Player = null;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(ActionControllor player)
    {
        Player = player;
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

    public int GetPlayerHpNow()
    {
        return Player.stateData.GetHPnow();
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
}
