using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Attack : MonoBehaviour
{
    /// <summary>
    /// キャラクター情報
    /// </summary>
    public enum CharactorType
    {
        PLAYER_1 = 0,
        PLAYER_2,
        ENEMY_1,
    }
    /*PLAYER*/
    /// <summary>現在位置</summary>
    [SerializeField]
    private int iThisNow, jThisNow;
    /// <summary>攻撃範囲</summary>
    [SerializeField]
    private int iThisAtkArea1, jThisAtkArea1, iThisAtkArea2, jThisAtkArea2, iThisAtkArea3, jThisAtkArea3;
    /// <summary>アクションコントローラー</summary>
    [SerializeField]
    private ActionControllor _actCtrl;
    /// <summary>ステータス</summary>
    [SerializeField]
    private StatusDataScript _thisState;
    /// <summary>キャラクタータイプ</summary>
    [SerializeField]
    private CharactorType myType;
    /// <summary>攻撃状態のコールバック</summary>
    private Action<bool> changeAtkCheck;
    /*ENEMY*/
    /// <summary>プレイヤーのステータス</summary>
    private StatusDataScript Player;


    /*Player_1*/
    /// <summary>
    /// Player_1攻撃ヒット判定
    /// </summary>
    private void AttackHitPlayer1()
    {
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea1, jThisAtkArea1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea2, jThisAtkArea2, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea3, jThisAtkArea3, _thisState.GetAttack());
    }

    /// <summary>
    /// Player_1SP攻撃ヒット判定
    /// </summary>
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

    /// <summary>
    /// Player_1攻撃範囲設定
    /// </summary>
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

    /// <summary>
    /// Player_1攻撃処理
    /// </summary>
    /// <param name="atkCheckflg"></param>
    /// <param name="spAtkFlg"></param>
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
    /// <summary>
    ///Player_2攻撃ヒット判定
    /// </summary>
    private void AttackHitPlayer2()
    {
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea1, jThisAtkArea1, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea2, jThisAtkArea2, _thisState.GetAttack());
        GameManager.Instance.GetEnemyManager().Hitcheck(iThisAtkArea3, jThisAtkArea3, _thisState.GetAttack());
    }

    /// <summary>
    /// Player_2SP攻撃判定
    /// </summary>
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

    /// <summary>
    /// Player_2攻撃範囲設定
    /// </summary>
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

    /// <summary>
    /// Player_2攻撃処理
    /// </summary>
    /// <param name="atkCheckflg"></param>
    /// <param name="spAtkFlg"></param>
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

    /// <summary>
    /// ENEMY_1プレイヤーステータス取得
    /// </summary>
    /// <param name="playerData"></param>
    public void GetPlayerStatusData(StatusDataScript playerData)
    {
        Player = playerData;
    }

    /// <summary>
    /// ENEMY_1ステータス取得
    /// </summary>
    /// <param name="thisData"></param>
    public void GetThisStatusData(StatusDataScript thisData)
    {
        _thisState = thisData;
    }

    /// <summary>
    /// ENEMY_1攻撃ヒット判定
    /// </summary>
    public void AttackHit()
    {
        Player.HitDamage(_thisState.Attack);
    }

    /// <summary>
    /// ENEMY_1プレイヤーの周囲チェック
    /// </summary>
    /// <param name="iPstate"></param>
    /// <param name="jPstate"></param>
    /// <param name="iEstate"></param>
    /// <param name="jEstate"></param>
    /// <returns></returns>
    public bool CheckPlayerThisAround(int iPstate, int jPstate, int iEstate, int jEstate)
    {
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

    /// <summary>
    /// ENEMY_1プレイヤーの周囲位置設定
    /// </summary>
    /// <param name="iPstate"></param>
    /// <param name="jPstate"></param>
    /// <param name="iEstate"></param>
    /// <param name="jEstate"></param>
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

    /// <summary>
    /// ENEMY_1攻撃処理
    /// </summary>
    /// <param name="atkCheckflg"></param>
    /// <param name="spAtkFlg"></param>
    /// <param name="changeAtkCheckflg"></param>
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
