using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Attack : MonoBehaviour
{
    public enum CharactorType
    {
        PLAYER_1 = 0,
        PLAYER_2,
        ENEMY_1,
    }
    /*PLAYER*/
    [SerializeField]
    private int iThisNow, jThisNow;
    [SerializeField]
    private int iThisAtkArea1, jThisAtkArea1, iThisAtkArea2, jThisAtkArea2, iThisAtkArea3, jThisAtkArea3;
    [SerializeField]
    private ActionControllor _actCtrl;
    [SerializeField]
    private StatusDataScript _thisState;
    [SerializeField]
    private CharactorType myType;
    private Action<bool> changeAtkCheck;
    /*ENEMY*/
    private StatusDataScript Player;


    /*Player_1*/
    private void AttackHitPlayer1()
    {
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea1, jThisAtkArea1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea2, jThisAtkArea2, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea3, jThisAtkArea3, _thisState.GetAttack());
    }
    private void SpAttackHitPlayer1()
    {
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow + 1, jThisNow + 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow + 1, jThisNow - 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow + 1, jThisNow, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow, jThisNow + 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow, jThisNow - 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow - 1, jThisNow + 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow - 1, jThisNow - 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow - 1, jThisNow, _thisState.GetAttack());
    }
    private void AttackAreaSetPlayer1()
    {
        switch (_actCtrl.thisNowDirection)
        {
            case ActionControllor.Direction.UP:
                iThisAtkArea1 = iThisNow - 1;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow;
                jThisAtkArea2 = jThisNow + 1;
                iThisAtkArea3 = iThisNow + 1;
                jThisAtkArea3 = jThisNow + 1;
                break;
            case ActionControllor.Direction.UP_LEFT:
                iThisAtkArea1 = iThisNow;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow - 1;
                jThisAtkArea2 = jThisNow + 1;
                iThisAtkArea3 = iThisNow - 1;
                jThisAtkArea3 = jThisNow;
                break;
            case ActionControllor.Direction.UP_RIGHT:
                iThisAtkArea1 = iThisNow;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow + 1;
                jThisAtkArea2 = jThisNow + 1;
                iThisAtkArea3 = iThisNow + 1;
                jThisAtkArea3 = jThisNow;
                break;
            case ActionControllor.Direction.LEFT:
                iThisAtkArea1 = iThisNow - 1;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow - 1;
                jThisAtkArea2 = jThisNow;
                iThisAtkArea3 = iThisNow - 1;
                jThisAtkArea3 = jThisNow - 1;
                break;
            case ActionControllor.Direction.RIGHT:
                iThisAtkArea1 = iThisNow + 1;
                jThisAtkArea1 = jThisNow + 1;
                iThisAtkArea2 = iThisNow + 1;
                jThisAtkArea2 = jThisNow;
                iThisAtkArea3 = iThisNow + 1;
                jThisAtkArea3 = jThisNow - 1;
                break;
            case ActionControllor.Direction.DOWN:
                iThisAtkArea1 = iThisNow - 1;
                jThisAtkArea1 = jThisNow - 1;
                iThisAtkArea2 = iThisNow;
                jThisAtkArea2 = jThisNow - 1;
                iThisAtkArea3 = iThisNow + 1;
                jThisAtkArea3 = jThisNow - 1;
                break;
            case ActionControllor.Direction.DOWN_LEFT:
                iThisAtkArea1 = iThisNow - 1;
                jThisAtkArea1 = jThisNow;
                iThisAtkArea2 = iThisNow - 1;
                jThisAtkArea2 = jThisNow - 1;
                iThisAtkArea3 = iThisNow;
                jThisAtkArea3 = jThisNow - 1;
                break;
            case ActionControllor.Direction.DOWN_RIGHT:
                iThisAtkArea1 = iThisNow + 1;
                jThisAtkArea1 = jThisNow;
                iThisAtkArea2 = iThisNow + 1;
                jThisAtkArea2 = jThisNow - 1;
                iThisAtkArea3 = iThisNow;
                jThisAtkArea3 = jThisNow - 1;
                break;
        }
    }
    private void AttackPlayer1(bool atkCheckflg, bool spAtkFlg)
    {
        iThisNow = _actCtrl.iThisNow;
        jThisNow = _actCtrl.jThisNow;
        AttackAreaSetPlayer1();
        if (atkCheckflg == true)
        {
            if (spAtkFlg == true)
            {
                SpAttackHitPlayer1();
            }
            else
            {
                AttackHitPlayer1();
            }
            changeAtkCheck.Invoke(false);
        }
    }
    /*Player_2*/
    private void AttackHitPlayer2()
    {
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea1, jThisAtkArea1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea2, jThisAtkArea2, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea3, jThisAtkArea3, _thisState.GetAttack());
    }
    private void SpAttackHitPlayer2()
    {
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow + 1, jThisNow + 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow + 1, jThisNow - 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow + 1, jThisNow, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow, jThisNow + 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow, jThisNow - 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow - 1, jThisNow + 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow - 1, jThisNow - 1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisNow - 1, jThisNow, _thisState.GetAttack());
    }
    private void AttackAreaSetPlayer2()
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
    private void AttackPlayer2(bool atkCheckflg, bool spAtkFlg)
    {
        iThisNow = _actCtrl.iThisNow;
        jThisNow = _actCtrl.jThisNow;
        AttackAreaSetPlayer2();
        if (atkCheckflg == true)
        {
            if (spAtkFlg == true)
            {
                SpAttackHitPlayer2();
            }
            else
            {
                AttackHitPlayer2();
            }
            changeAtkCheck.Invoke(false);
        }

    }
    /*ENEMY_1*/
    public void GetPlayerStatusData(StatusDataScript playerData)
    {
        Player = playerData;
    }
    public void GetThisStatusData(StatusDataScript thisData)
    {
        _thisState = thisData;
    }

    public void AttackHit()
    {
        Player.HitDamage(_thisState.Attack);
    }

    public bool CheckPlayerThisAround(int iPstate, int jPstate, int iEstate, int jEstate)
    {
        //iiPstate = iPstate;
        //jjPstate = jPstate;
        //iiEstate = iEstate;
        //jjEstate = jEstate;
        if (iEstate == iPstate && jEstate + 1 == jPstate)
        {//UP
            _actCtrl.SetDirection(ActionControllor.Direction.UP);
            return true;
        }
        else if (iEstate - 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_LEFT
            _actCtrl.SetDirection(ActionControllor.Direction.UP_LEFT);
            return true;
        }
        else if (iEstate + 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_RIGHT
            _actCtrl.SetDirection(ActionControllor.Direction.UP_RIGHT);
            return true;
        }
        else if (iEstate - 1 == iPstate && jEstate == jPstate)
        {//LEFT
            _actCtrl.SetDirection(ActionControllor.Direction.LEFT);
            return true;
        }
        else if (iEstate + 1 == iPstate && jEstate == jPstate)
        {//RIGHT
            _actCtrl.SetDirection(ActionControllor.Direction.RIGHT);
            return true;
        }
        else if (iEstate == iPstate && jEstate - 1 == jPstate)
        {//DOWN
            _actCtrl.SetDirection(ActionControllor.Direction.DOWN);
            return true;
        }
        else if (iEstate - 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_LEFT
            _actCtrl.SetDirection(ActionControllor.Direction.DOWN_LEFT);
            return true;
        }
        else if (iEstate + 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_RIGHT
            _actCtrl.SetDirection(ActionControllor.Direction.DOWN_RIGHT);
            return true;
        }
        else
        {//周囲にいなかったとき
            return false;
        }
    }
    public void SetDirectionPlayerThisAround(int iPstate, int jPstate, int iEstate, int jEstate)
    {
        if (iEstate == iPstate && jEstate + 1 == jPstate)
        {//UP
            _actCtrl.SetDirection(ActionControllor.Direction.UP);
        }
        else if (iEstate - 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_LEFT
            _actCtrl.SetDirection(ActionControllor.Direction.UP_LEFT);
        }
        else if (iEstate + 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_RIGHT
            _actCtrl.SetDirection(ActionControllor.Direction.UP_RIGHT);
        }
        else if (iEstate - 1 == iPstate && jEstate == jPstate)
        {//LEFT
            _actCtrl.SetDirection(ActionControllor.Direction.LEFT);
        }
        else if (iEstate + 1 == iPstate && jEstate == jPstate)
        {//RIGHT
            _actCtrl.SetDirection(ActionControllor.Direction.RIGHT);
        }
        else if (iEstate == iPstate && jEstate - 1 == jPstate)
        {//DOWN
            _actCtrl.SetDirection(ActionControllor.Direction.DOWN);
        }
        else if (iEstate - 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_LEFT
            _actCtrl.SetDirection(ActionControllor.Direction.DOWN_LEFT);
        }
        else if (iEstate + 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_RIGHT
            _actCtrl.SetDirection(ActionControllor.Direction.DOWN_RIGHT);
        }
        else
        {//周囲にいなかったとき
        }
    }

    public void StartAttack(bool atkCheckflg, bool spAtkFlg, Action<bool> changeAtkCheckflg = null)
    {
        switch (myType)
        {
            case CharactorType.PLAYER_1:
                AttackPlayer1(atkCheckflg, spAtkFlg);
                changeAtkCheck = changeAtkCheckflg;
                break;
            case CharactorType.PLAYER_2:
                AttackPlayer2(atkCheckflg, spAtkFlg);
                changeAtkCheck = changeAtkCheckflg;
                break;
            case CharactorType.ENEMY_1:
                break;
            default:
                break;
        }
    }
}
