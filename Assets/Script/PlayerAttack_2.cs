using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerAttack_2 : MonoBehaviour {
    [SerializeField]
    int iThisNow, jThisNow;
    [SerializeField]
    int iThisAtkArea1, jThisAtkArea1, iThisAtkArea2, jThisAtkArea2, iThisAtkArea3, jThisAtkArea3;
    private GameControllor Contollor;
    private ActionControllor _actCtrl;
    private StatusDataScript _thisState;

    //public void SetGameCtrl(GameControllor ctrl)
    //{
    //    Contollor = ctrl;
    //}
    void AttackHit()
    {
        Contollor.Hitcheck(iThisAtkArea1, jThisAtkArea1, _thisState.Attack);
        Contollor.Hitcheck(iThisAtkArea2, jThisAtkArea2, _thisState.Attack);
        Contollor.Hitcheck(iThisAtkArea3, jThisAtkArea3, _thisState.Attack);
    }
    void SpAttackHit()
    {
        Contollor.Hitcheck(iThisNow + 1, jThisNow + 1, _thisState.Attack);
        Contollor.Hitcheck(iThisNow + 1, jThisNow - 1, _thisState.Attack);
        Contollor.Hitcheck(iThisNow + 1, jThisNow, _thisState.Attack);
        Contollor.Hitcheck(iThisNow, jThisNow + 1, _thisState.Attack);
        Contollor.Hitcheck(iThisNow, jThisNow - 1, _thisState.Attack);
        Contollor.Hitcheck(iThisNow - 1, jThisNow + 1, _thisState.Attack);
        Contollor.Hitcheck(iThisNow - 1, jThisNow - 1, _thisState.Attack);
        Contollor.Hitcheck(iThisNow - 1, jThisNow, _thisState.Attack);
    }

    void AttackAreaSet()
    {

        switch (_actCtrl.thisNowDirection)
        {
            case ActionControllor.Direction.UP:
                iThisAtkArea1 = iThisNow;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow;
                jThisAtkArea2 = jThisNow + 2;
                iThisAtkArea3 = iThisNow;
                jThisAtkArea3 = jThisNow + 3;
                break;
            case ActionControllor.Direction.UP_LEFT:
                iThisAtkArea1 = iThisNow - 1;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow - 2;
                jThisAtkArea2 = jThisNow + 2;
                iThisAtkArea3 = iThisNow - 3;
                jThisAtkArea3 = jThisNow + 3;
                break;
            case ActionControllor.Direction.UP_RIGHT:
                iThisAtkArea1 = iThisNow + 1;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow + 2;
                jThisAtkArea2 = jThisNow + 2;
                iThisAtkArea3 = iThisNow + 3;
                jThisAtkArea3 = jThisNow + 3;
                break;
            case ActionControllor.Direction.LEFT:
                iThisAtkArea1 = iThisNow - 1;
                jThisAtkArea1 = jThisNow;
                iThisAtkArea2 = iThisNow - 2;
                jThisAtkArea2 = jThisNow;
                iThisAtkArea3 = iThisNow - 3;
                jThisAtkArea3 = jThisNow;
                break;
            case ActionControllor.Direction.RIGHT:
                iThisAtkArea1 = iThisNow + 1;
                jThisAtkArea1 = jThisNow;
                iThisAtkArea2 = iThisNow + 2;
                jThisAtkArea2 = jThisNow;
                iThisAtkArea3 = iThisNow + 3;
                jThisAtkArea3 = jThisNow;
                break;
            case ActionControllor.Direction.DOWN:
                iThisAtkArea1 = iThisNow;
                jThisAtkArea1 = jThisNow - 1;
                iThisAtkArea2 = iThisNow;
                jThisAtkArea2 = jThisNow - 2;
                iThisAtkArea3 = iThisNow;
                jThisAtkArea3 = jThisNow - 3;
                break;
            case ActionControllor.Direction.DOWN_LEFT:
                iThisAtkArea1 = iThisNow - 1;
                jThisAtkArea1 = jThisNow - 1;
                iThisAtkArea2 = iThisNow - 2;
                jThisAtkArea2 = jThisNow - 2;
                iThisAtkArea3 = iThisNow - 3;
                jThisAtkArea3 = jThisNow - 3;
                break;
            case ActionControllor.Direction.DOWN_RIGHT:
                iThisAtkArea1 = iThisNow + 1;
                jThisAtkArea1 = jThisNow - 1;
                iThisAtkArea2 = iThisNow + 2;
                jThisAtkArea2 = jThisNow - 2;
                iThisAtkArea3 = iThisNow + 3;
                jThisAtkArea3 = jThisNow - 3;
                break;
        }
    }
    // Use this for initialization
    void Start()
    {
        _actCtrl = this.GetComponent<ActionControllor>();
        Contollor = _actCtrl.GetGameCtrl();
        _thisState = this.GetComponent<StatusDataScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Contollor.AtkCheckflg == true)
        {
            if (Contollor.SpAtkflg == true)
            {
                SpAttackHit();
            }
            else
            {
                AttackHit();
            }
            Contollor.AtkCheckflg = false;
        }
        iThisNow = _actCtrl.iThisNow;
        jThisNow = _actCtrl.jThisNow;
        AttackAreaSet();
    }
}
