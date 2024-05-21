using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    private List<ActionControllor> saveEnemyList = new List<ActionControllor>();
    private List<ActionControllor> EnemyList = new List<ActionControllor>();
    private int EnemyCount;
    private int iRandom, jRandom;
    private int iPmap, jPmap;
    private int iEmap, jEmap;
    [SerializeField]
    private int EnemyAtkCount, EnemyMoveCount, EnemyAtkResetCount, EnemyMoveResetCount;
    private List<ActionControllor> AtkEnemyList = new List<ActionControllor>();
    private List<ActionControllor> AtkResetEnemyList = new List<ActionControllor>();
    private List<ActionControllor> MoveEnemyList = new List<ActionControllor>();
    private List<ActionControllor> MoveResetEnemyList = new List<ActionControllor>();
    private bool attackable;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(List<ActionControllor> enemyList)
    {
        saveEnemyList = enemyList;
        //EnemyList = enemyList;
        attackable = true;
        EnemyListSetUp(saveEnemyList);
    }
    /// <summary>
    /// 攻撃ヒット判定処理
    /// </summary>
    /// <param name="iAttack"></param>
    /// <param name="jAttack"></param>
    /// <param name="attack"></param>
    public void Hitcheck(int iAttack, int jAttack, int attack)
    {
        //複数or0だったときの処理が必要？
        if (MapGenerator.EnemyCount >= 1)
        {
            for (int count = 0; count < EnemyCount; count++)
            {
                if (EnemyList[count].stateData.CheckAttack(iAttack, jAttack) == true)
                {
                    EnemyList[count].stateData.HitDamage(attack);
                }
                else
                {
                }
            }
        }
    }

    /// <summary>
    /// 敵リストのセットアップ
    /// </summary>
    /// <param name="enemylist"></param>
    public void EnemyListSetUp(List<ActionControllor> enemylist)
    {
        for (int count = 0; count < MapGenerator.EnemyCount; count++)
        {
            if (enemylist[count] != null)
            {
                EnemyList.Add(enemylist[count]);
                enemylist[count].StartSetUp();
                EnemyCount += 1;
            }
        }
        EnemyAtkCount = 0;
        EnemyMoveCount = 0;
        EnemyAtkResetCount = 0;
        EnemyMoveResetCount = 0;
    }
    /// <summary>
    /// 敵エネミー向き設定
    /// </summary>
    /// <param name="iStep"></param>
    /// <param name="jStep"></param>
    /// <param name="Enemy"></param>
    void SetEnemyDirection(int iStep, int jStep, ActionControllor Enemy)
    {
        if (iStep == 0 && jStep == 1)
        {//UP
            Enemy.SetDirection(ActionControllor.Direction.UP);
        }
        else if (iStep == -1 && jStep == 1)
        {//UP_LEFT
            Enemy.SetDirection(ActionControllor.Direction.UP_LEFT);
        }
        else if (iStep == 1 && jStep == 1)
        {//UP_RIGHT
            Enemy.SetDirection(ActionControllor.Direction.UP_RIGHT);
        }
        else if (iStep == -1 && jStep == 0)
        {//LEFT
            Enemy.SetDirection(ActionControllor.Direction.LEFT);
        }
        else if (iStep == 1 && jStep == 0)
        {//RIGHT
            Enemy.SetDirection(ActionControllor.Direction.RIGHT);
        }
        else if (iStep == 0 && jStep == -1)
        {//DOWN
            Enemy.SetDirection(ActionControllor.Direction.DOWN);
        }
        else if (iStep == -1 && jStep == -1)
        {//DOWN_LEFT
            Enemy.SetDirection(ActionControllor.Direction.DOWN_LEFT);
        }
        else if (iStep == 1 && jStep == -1)
        {//DOWN_RIGHT
            Enemy.SetDirection(ActionControllor.Direction.DOWN_RIGHT);
        }
        else
        {//動かないときはそのまま

        }

    }
    /// <summary>
    /// 敵移動先チェック
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="MaxCount"></param>
    /// <returns></returns>
    public bool checkNowotherEmoveFlg(ActionControllor Enemy, int MaxCount)
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (count != MaxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].CheckNowStep(Enemy.SetiNextStepArea(), Enemy.SetjNextStepArea()) == true)
                {
                    return true;
                }
                else
                {
                }
            }
        }
        return false;//何事もなく終わったとき
    }

    /// <summary>
    /// 敵移動先に先約がいないかチェック
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="MaxCount"></param>
    /// <returns></returns>
    public bool checkNextotherEmoveFlg(ActionControllor Enemy, int MaxCount)
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (count != MaxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (EnemyList[count].CheckNextStep(Enemy.SetiNextStepArea(), Enemy.SetjNextStepArea()) == true)
                {
                    return true;
                }
                else
                {
                }
            }
        }
        return false;//何事もなく終わったとき
    }
    /// <summary>
    /// 敵移動（ランダム）
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="thisCount"></param>
    void EnemyMoveRandom(ActionControllor Enemy, int thisCount)
    {
        bool checkRandom = true;
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        while (checkRandom == true)
        {
            iRandom = (int)UnityEngine.Random.Range(-1, 2);
            jRandom = (int)UnityEngine.Random.Range(-1, 2);
            Enemy.SetNextStep(iRandom, jRandom);
            bool otherEmoveFlg = false;

            if (thisCount == 0)//[0]のみ現在位置で調べる
            {
                otherEmoveFlg = checkNowotherEmoveFlg(Enemy, EnemyCount);
            }
            else
            {
                otherEmoveFlg = checkNextotherEmoveFlg(Enemy, thisCount);
            }
            if (otherEmoveFlg != true)
            {
                if (Enemy.CheckNextStepWall() == false)
                {
                }
                else
                {
                    SetEnemyDirection(iRandom, jRandom, Enemy);
                    checkRandom = false;
                }
            }
        }

    }
    /// <summary>
    /// 敵移動（プレイヤー視認）
    /// </summary>
    /// <param name="Enemy"></param>
    /// <param name="thisCount"></param>
    void EnemyMoveTargetPlayer(ActionControllor Enemy, int thisCount,(int,int) plaerPos)
    {
        //ここはいずれA*アルゴリズムで動かしたい
        int iEnemyNext, jEnemyNext;
        bool otherEmoveFlg = false;
        if (plaerPos.Item1 - (int)Math.Round(Enemy.transform.position.x) > 0)
        {
            iEnemyNext = 1;
        }
        else if (plaerPos.Item1 - (int)Math.Round(Enemy.transform.position.x) < 0)
        {
            iEnemyNext = -1;
        }
        else
        {//(int)Player.transform.position.x == (int)Enemy.transform.position.x
            iEnemyNext = 0;
        }

        if (plaerPos.Item2 - (int)Math.Round(Enemy.transform.position.y) > 0)
        {
            jEnemyNext = 1;
        }
        else if (plaerPos.Item2 - (int)Math.Round(Enemy.transform.position.y) < 0)
        {
            jEnemyNext = -1;
        }
        else
        {//(int)Player.transform.position.y == (int)Enemy.transform.position.y
            jEnemyNext = 0;
        }
        Enemy.SetNextStep(iEnemyNext, jEnemyNext);

        //for分で該当オブジェクトより手前に設定している敵オブジェクトをしらべる（後のオブジェクトは移動先を設定していないため）
        //※後半のオブジェクトが移動しなかった場合重なる可能性がある（例：後半の敵で攻撃可能がいた場合）
        //ここでの処理を攻撃と移動のリストに分けたうえで移動側のみ調べる。調べる対象は全リスト
        if (thisCount == 0)//[0]のみ現在位置で調べる
        {
            otherEmoveFlg = checkNowotherEmoveFlg(Enemy, EnemyCount);
        }
        else
        {
            otherEmoveFlg = checkNextotherEmoveFlg(Enemy, thisCount);
        }
        if (otherEmoveFlg != true)
        {
            if (Enemy.CheckNextStepWall() == false)
            {
                SetEnemyDirection(iEnemyNext, jEnemyNext, Enemy);
                iEnemyNext = 0;
                jEnemyNext = 0;
                Enemy.SetNextStep(iEnemyNext, jEnemyNext);
            }
            else
            {
                SetEnemyDirection(iEnemyNext, jEnemyNext, Enemy);
            }
        }
        else
        {
            iEnemyNext = 0;
            jEnemyNext = 0;
            Enemy.SetNextStep(iEnemyNext, jEnemyNext);

        }
    }
    public void SetEnemyMove()
    {
        for (int count = 0; count < EnemyCount; count++)
        {
            if (MapGenerator.EnemyCount >= 1)
            {
                //※敵動作については条件で複数パターンあるため現状は仮設定
                //Enemy = GameObject.Find("EnemyPrefab(Clone)");
                //現状プレイヤーの移動前の情報を所得しているので移動先の情報にする必要あり
                //確認用に宣言中　現在Playerの位置情報が移動前の位置情報を所得している※positionが少数点で0.999になると切り捨てになってしまう
                //iEmap = (int)Math.Round(Enemy.transform.position.x);
                iEmap = (int)Math.Round(EnemyList[count].transform.position.x);
                iPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1;
                //jEmap = (int)Math.Round(Enemy.transform.position.y);
                jEmap = (int)Math.Round(EnemyList[count].transform.position.y);
                jPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2;

                if (EnemyList[count].enemyAtk.CheckPlayerThisAround(iPmap, jPmap, iEmap, jEmap) == true)//各敵の周囲(3*3)にプレイヤーがいるかチェックし居たらそちらに方向を切り替えて攻撃動作をセット
                {//周囲を調べてプレイヤーがいた場合方向だけセットしておく
                    //攻撃リストに登録 ※ここは攻撃前
                    //※攻撃方向が指定できていないことがある
                    //SetEnemyDirection(iEnemyNext, jEnemyNext, EnemyList[count]);
                    //攻撃時のリスト追加と同時に攻撃フラグをture
                    EnemyList[count].SetUserAttackFlg();
                    AtkEnemyList.Add(EnemyList[count]);
                    EnemyAtkCount += 1;
                }
                else
                {   //※リストへの割り当てと移動方法への関数呼び出しは別にする
                    MoveEnemyList.Add(EnemyList[count]);
                    EnemyMoveCount += 1;
                }

            }
        }
        for (int count = 0; count < EnemyCount; count++)
        {
            if (EnemyCount >= 1)
            {
                //MoveEnemyListだと移動チェック時に他の敵の位置を見るときに攻撃時の敵を見ることが現状できないため
                if (EnemyList[count].GetUserAttackFlg() == false)
                {
                    if (MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x),
                        (int)Math.Round(EnemyList[count].transform.position.y)] == 0)
                    {//通路だった場合は
                        EnemyMoveRandom(EnemyList[count], count); //現状はランダム移動（後で通路は直進するようにしたい）
                    }
                    else
                    {
                        PlayerManager player = GameManager.Instance.GetPlayerManager();
                        if (MapGenerator.map[(int)Math.Round(player.GetTransform().position.x),
                            (int)Math.Round(player.GetTransform().position.y)] == MapGenerator.map[(int)Math.Round(EnemyList[count].transform.position.x),
                            (int)Math.Round(EnemyList[count].transform.position.y)])
                        {
                            EnemyMoveTargetPlayer(EnemyList[count], count, player.GetNextStepArea());
                        }
                        else
                        {
                            EnemyMoveRandom(EnemyList[count], count);
                        }
                    }
                }
            }
        }
        //攻撃リストの敵の攻撃方向の指定
        for (int count = 0; count < EnemyAtkCount; count++)
        {
            if (EnemyAtkCount >= 1)
            {
                iEmap = (int)Math.Round(AtkEnemyList[count].transform.position.x);
                iPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1;
                jEmap = (int)Math.Round(AtkEnemyList[count].transform.position.y);
                jPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2;
                AtkEnemyList[count].enemyAtk.SetDirectionPlayerThisAround(iPmap, jPmap, iEmap, jEmap);
            }
        }
    }
    public void ResetEnemyList()
    {
        //Listの初期化
        EnemyList.Clear();
        EnemyCount = 0;
        for (int count = 0; count < MapGenerator.EnemyCount; count++)
        {
            if (saveEnemyList[count] != null)
            {
                EnemyList.Add(saveEnemyList[count]);
                EnemyCount += 1;
            }
        }

    }

    public void ResetAttkEnemyList()
    {

        //Listの初期化
        AtkResetEnemyList.Clear();
        EnemyAtkResetCount = 0;
        for (int count = 0; count < EnemyAtkCount; count++)
        {
            if (AtkEnemyList[count] != null)
            {
                AtkResetEnemyList.Add(AtkEnemyList[count]);
                EnemyAtkResetCount += 1;
            }
        }

        AtkEnemyList.Clear();
        for (int count = 0; count < EnemyAtkResetCount; count++)
        {
            AtkEnemyList.Add(AtkResetEnemyList[count]);
        }
        EnemyAtkCount = EnemyAtkResetCount;

    }
    public void ResetMoveEnemyList()
    {

        //Listの初期化
        MoveResetEnemyList.Clear();
        EnemyMoveResetCount = 0;
        for (int count = 0; count < EnemyMoveCount; count++)
        {
            if (MoveEnemyList[count] != null)
            {
                MoveResetEnemyList.Add(MoveEnemyList[count]);
                EnemyMoveResetCount += 1;
            }
        }

        MoveEnemyList.Clear();
        for (int count = 0; count < EnemyMoveResetCount; count++)
        {
            MoveEnemyList.Add(MoveResetEnemyList[count]);
        }
        EnemyMoveCount = EnemyMoveResetCount;

    }

    public List<ActionControllor> GetEnemyList()
    {
        return EnemyList;
    }
    public void SetParamAtkEnemyList(int cnt)
    {
        if (EnemyAtkCount > 0)
        {
            AtkEnemyList[cnt].SetUserAttackFlagOn();
            //if (attackable == true)
            //{
            //    AtkEnemyList[cnt].ActionStart();
            //    AtkEnemyList[cnt].enemyAtk.AttackHit();
            //}
            //else
            //{
            //    AtkEnemyList[cnt].CancelCts();
            //}
            AtkEnemyList[cnt].ActionStart(attackable);
            AtkEnemyList[cnt].enemyAtk.AttackHit();

        }
    }
    public void SetParamMoveEnemyList()
    {
        for (int count = 0; count < EnemyMoveCount; count++)
        {
            //if (attackable == true)
            //{
                MoveEnemyList[count].SetUserActFlagOn();
                MoveEnemyList[count].ActionStart(attackable);
            //}
            //else
            //{
            //    MoveEnemyList[count].CancelCts();
            //}

        }
    }
    public int GetEnemyAtkCount()
    {
        return EnemyAtkCount;
    }
    public void ResetAllEnemyList()
    {
        ResetEnemyList();
        AtkEnemyList.Clear();
        EnemyAtkCount = 0;
        MoveEnemyList.Clear();
        EnemyMoveCount = 0;
    }
    public  void ChangeAttackable(bool flg)
    {
        attackable = flg;
    }
}
