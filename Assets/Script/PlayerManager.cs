using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public ActionControllor Player = null;
    public StatusDataScript _playerState = null;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        ///TODO:プレイヤーの情報設定を行う
    }
    //*Player *//
    public void SetPlayerActionCtrl(ActionControllor player)
    {
        Player = player;
    }


    //*_playerState *//
    public int GetPlayerHpNow()
    {
        return _playerState.GetHPnow();
    }

    public void AddItemState(ItemStatusData data)
    {
        _playerState.AddState(data);
    }
    public void SubItemState(ItemStatusData data)
    {
        _playerState.SubState(data);
    }
    public void SetPlayerState(StatusDataScript pState)
    {
        _playerState = pState;
        ItemWindowScript.Instance.SetupItemState();
    }
}
