using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject _player_1_Object;
    [SerializeField]
    private GameObject _player_2_Object;
    //[SerializeField]
    //private GameObject _player_3_Object;
    private GameObject _ansTypeBulletObject;

    public enum PlayerKind
    {
        Player_1 = 0,
        //Player_1,
        Player_2
    }
    public GameObject SelectTypeBullet(PlayerKind ID)
    {
        switch (ID)
        {
            case PlayerKind.Player_1:
                _ansTypeBulletObject = _player_1_Object;
                break;
            case PlayerKind.Player_2:
                _ansTypeBulletObject = _player_2_Object;
                break;
                //case PlayerKind.Player_2:
                //    _ansTypeBulletObject = _player_3_Object;
                //    break;

        }
        return _ansTypeBulletObject;
    }
}
