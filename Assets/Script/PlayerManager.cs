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
    public void Init(ActionControllor player, StatusDataScript status)
    {
        Player = player;
        _playerState = status;
        ItemWindowScript.Instance.SetupItemState();

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
}
