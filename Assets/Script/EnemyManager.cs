using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    /// <summary>敵個数 </summary>
    private int enemyCount;
    /// <summary>乱数用 </summary>
    private int iRandom, jRandom;
    /// <summary>プレイヤー位置 </summary>
    private int iPmap, jPmap;
    /// <summary>敵位置 </summary>
    private int iEmap, jEmap;
    /// <summary>行動別敵個数+リセット処理用個数 </summary>
    private int enemyAtkCount, enemyMoveCount, enemyAtkResetCount, enemyMoveResetCount;
    /// <summary>敵個数初期状態保存用 </summary>
    private List<ActionControllor> saveEnemyList = new List<ActionControllor>();
    /// <summary>敵リスト </summary>
    private List<ActionControllor> enemyList = new List<ActionControllor>();
    /// <summary>行動別敵リスト+リセット処理用リスト </summary>
    private List<ActionControllor> atkEnemyList = new List<ActionControllor>();
    private List<ActionControllor> atkResetEnemyList = new List<ActionControllor>();
    private List<ActionControllor> moveEnemyList = new List<ActionControllor>();
    private List<ActionControllor> moveResetEnemyList = new List<ActionControllor>();
    /// <summary>攻撃可能かフラグ </summary>
    private bool attackable;
    /// <summary>ドロップアイテム配置用コールバック </summary>
    Action<int, int> dropItemPosition;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="droppos"></param>
    public void Init(List<ActionControllor> enemyList,Action<int,int> droppos = null)
    {
        dropItemPosition = droppos;
        saveEnemyList = enemyList;
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
            for (int count = 0; count < enemyCount; count++)
            {
                if (enemyList[count].stateData.CheckAttack(iAttack, jAttack) == true)
                {
                    enemyList[count].stateData.HitDamage(attack,(idrop,jdrop)=> 
                    {
                        dropItemPosition.Invoke(idrop, jdrop);
                    });
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
                enemyList.Add(enemylist[count]);
                enemylist[count].Init();
                enemyCount += 1;
            }
        }
        enemyAtkCount = 0;
        enemyMoveCount = 0;
        enemyAtkResetCount = 0;
        enemyMoveResetCount = 0;
    }

    /// <summary>
    /// 敵エネミー向き設定
    /// </summary>
    /// <param name="iStep"></param>
    /// <param name="jStep"></param>
    /// <param name="enemy"></param>
    void SetEnemyDirection(int iStep, int jStep, ActionControllor enemy)
    {
        if (iStep == 0 && jStep == 1)
        {//UP
            enemy.SetDirection(ActionControllor.Direction.UP);
        }
        else if (iStep == -1 && jStep == 1)
        {//UP_LEFT
            enemy.SetDirection(ActionControllor.Direction.UP_LEFT);
        }
        else if (iStep == 1 && jStep == 1)
        {//UP_RIGHT
            enemy.SetDirection(ActionControllor.Direction.UP_RIGHT);
        }
        else if (iStep == -1 && jStep == 0)
        {//LEFT
            enemy.SetDirection(ActionControllor.Direction.LEFT);
        }
        else if (iStep == 1 && jStep == 0)
        {//RIGHT
            enemy.SetDirection(ActionControllor.Direction.RIGHT);
        }
        else if (iStep == 0 && jStep == -1)
        {//DOWN
            enemy.SetDirection(ActionControllor.Direction.DOWN);
        }
        else if (iStep == -1 && jStep == -1)
        {//DOWN_LEFT
            enemy.SetDirection(ActionControllor.Direction.DOWN_LEFT);
        }
        else if (iStep == 1 && jStep == -1)
        {//DOWN_RIGHT
            enemy.SetDirection(ActionControllor.Direction.DOWN_RIGHT);
        }
        else
        {//動かないときはそのまま

        }

    }

    /// <summary>
    /// 敵移動先チェック
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="MaxCount"></param>
    /// <returns></returns>
    public bool checkNowotherEmoveFlg(ActionControllor enemy, int maxCount)
    {
        for (int count = 0; count < enemyCount; count++)
        {
            if (count != maxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (enemyList[count].CheckNowStep(enemy.SetiNextStepArea(), enemy.SetjNextStepArea()) == true)
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
    /// <param name="enemy"></param>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    public bool checkNextotherEmoveFlg(ActionControllor enemy, int maxCount)
    {
        for (int count = 0; count < enemyCount; count++)
        {
            if (count != maxCount) //自分の位置については無視
            {
                //全的オブジェクトの現在位置を調べて移動先にいないかのチェック。いたらtrueを返す
                if (enemyList[count].CheckNextStep(enemy.SetiNextStepArea(), enemy.SetjNextStepArea()) == true)
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
    /// <param name="enemy"></param>
    /// <param name="thisCount"></param>
    void EnemyMoveRandom(ActionControllor enemy, int thisCount)
    {
        bool checkRandom = true;
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        while (checkRandom == true)
        {
            iRandom = (int)UnityEngine.Random.Range(-1, 2);
            jRandom = (int)UnityEngine.Random.Range(-1, 2);
            enemy.SetNextStep(iRandom, jRandom);
            bool otherEmoveFlg = false;

            if (thisCount == 0)//[0]のみ現在位置で調べる
            {
                otherEmoveFlg = checkNowotherEmoveFlg(enemy, enemyCount);
            }
            else
            {
                otherEmoveFlg = checkNextotherEmoveFlg(enemy, thisCount);
            }
            if (otherEmoveFlg != true)
            {
                if (enemy.CheckNextStepWall() == false)
                {
                }
                else
                {
                    SetEnemyDirection(iRandom, jRandom, enemy);
                    checkRandom = false;
                }
            }
        }

    }

    /// <summary>
    /// 敵移動（プレイヤー視認）
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="thisCount"></param>
    void EnemyMoveTargetPlayer(ActionControllor enemy, int thisCount,(int,int) plaerPos)
    {
        //ここはいずれA*アルゴリズムで動かしたい
        int iEnemyNext, jEnemyNext;
        bool otherEmoveFlg = false;
        if (plaerPos.Item1 - (int)Math.Round(enemy.transform.position.x) > 0)
        {
            iEnemyNext = 1;
        }
        else if (plaerPos.Item1 - (int)Math.Round(enemy.transform.position.x) < 0)
        {
            iEnemyNext = -1;
        }
        else
        {
            iEnemyNext = 0;
        }

        if (plaerPos.Item2 - (int)Math.Round(enemy.transform.position.y) > 0)
        {
            jEnemyNext = 1;
        }
        else if (plaerPos.Item2 - (int)Math.Round(enemy.transform.position.y) < 0)
        {
            jEnemyNext = -1;
        }
        else
        {
            jEnemyNext = 0;
        }
        enemy.SetNextStep(iEnemyNext, jEnemyNext);

        //for分で該当オブジェクトより手前に設定している敵オブジェクトをしらべる（後のオブジェクトは移動先を設定していないため）
        //ここでの処理を攻撃と移動のリストに分けたうえで移動側のみ調べる。調べる対象は全リスト
        if (thisCount == 0)//[0]のみ現在位置で調べる
        {
            otherEmoveFlg = checkNowotherEmoveFlg(enemy, enemyCount);
        }
        else
        {
            otherEmoveFlg = checkNextotherEmoveFlg(enemy, thisCount);
        }
        if (otherEmoveFlg != true)
        {
            if (enemy.CheckNextStepWall() == false)
            {
                SetEnemyDirection(iEnemyNext, jEnemyNext, enemy);
                iEnemyNext = 0;
                jEnemyNext = 0;
                enemy.SetNextStep(iEnemyNext, jEnemyNext);
            }
            else
            {
                SetEnemyDirection(iEnemyNext, jEnemyNext, enemy);
            }
        }
        else
        {
            iEnemyNext = 0;
            jEnemyNext = 0;
            enemy.SetNextStep(iEnemyNext, jEnemyNext);

        }
    }

    /// <summary>
    /// 敵移動先設定
    /// </summary>
    public void SetEnemyMove()
    {
        for (int count = 0; count < enemyCount; count++)
        {
            if (MapGenerator.EnemyCount >= 1)
            {
                iEmap = (int)Math.Round(enemyList[count].transform.position.x);
                iPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1;
                jEmap = (int)Math.Round(enemyList[count].transform.position.y);
                jPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2;

                if (enemyList[count].enemyAtk.CheckPlayerThisAround(iPmap, jPmap, iEmap, jEmap) == true)//各敵の周囲(3*3)にプレイヤーがいるかチェックし居たらそちらに方向を切り替えて攻撃動作をセット
                {
                    enemyList[count].SetUserAttackFlg();
                    atkEnemyList.Add(enemyList[count]);
                    enemyAtkCount += 1;
                }
                else
                {   //※リストへの割り当てと移動方法への関数呼び出しは別にする
                    moveEnemyList.Add(enemyList[count]);
                    enemyMoveCount += 1;
                }

            }
        }
        for (int count = 0; count < enemyCount; count++)
        {
            if (enemyCount >= 1)
            {
                if (enemyList[count].GetUserAttackFlg() == false)
                {
                    if (MapGenerator.map[(int)Math.Round(enemyList[count].transform.position.x),
                        (int)Math.Round(enemyList[count].transform.position.y)] == 0)
                    {//通路だった場合は
                        EnemyMoveRandom(enemyList[count], count); //現状はランダム移動（後で通路は直進するようにしたい）
                    }
                    else
                    {
                        PlayerManager player = GameManager.Instance.GetPlayerManager();
                        if (MapGenerator.map[(int)Math.Round(player.GetTransform().position.x),
                            (int)Math.Round(player.GetTransform().position.y)] == MapGenerator.map[(int)Math.Round(enemyList[count].transform.position.x),
                            (int)Math.Round(enemyList[count].transform.position.y)])
                        {
                            EnemyMoveTargetPlayer(enemyList[count], count, player.GetNextStepArea());
                        }
                        else
                        {
                            EnemyMoveRandom(enemyList[count], count);
                        }
                    }
                }
            }
        }
        //攻撃リストの敵の攻撃方向の指定
        for (int count = 0; count < enemyAtkCount; count++)
        {
            if (enemyAtkCount >= 1)
            {
                iEmap = (int)Math.Round(atkEnemyList[count].transform.position.x);
                iPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item1;
                jEmap = (int)Math.Round(atkEnemyList[count].transform.position.y);
                jPmap = GameManager.Instance.GetPlayerManager().GetNextStepArea().Item2;
                atkEnemyList[count].enemyAtk.SetDirectionPlayerThisAround(iPmap, jPmap, iEmap, jEmap);
            }
        }
    }

    /// <summary>
    /// 敵のリセット処理（敵が倒された場合の穴埋め）
    /// </summary>
    public void ResetEnemyList()
    {
        //Listの初期化
        enemyList.Clear();
        enemyCount = 0;
        for (int count = 0; count < MapGenerator.EnemyCount; count++)
        {
            if (saveEnemyList[count] != null)
            {
                enemyList.Add(saveEnemyList[count]);
                enemyCount += 1;
            }
        }

    }

    /// <summary>
    /// 攻撃した敵のリセット処理
    /// </summary>
    public void ResetAttkEnemyList()
    {
        //Listの初期化
        atkResetEnemyList.Clear();
        enemyAtkResetCount = 0;
        for (int count = 0; count < enemyAtkCount; count++)
        {
            if (atkEnemyList[count] != null)
            {
                atkResetEnemyList.Add(atkEnemyList[count]);
                enemyAtkResetCount += 1;
            }
        }
        atkEnemyList.Clear();
        for (int count = 0; count < enemyAtkResetCount; count++)
        {
            atkEnemyList.Add(atkResetEnemyList[count]);
        }
        enemyAtkCount = enemyAtkResetCount;
    }

    /// <summary>
    /// 移動した敵のリセット処理
    /// </summary>
    public void ResetMoveEnemyList()
    {
        //Listの初期化
        moveResetEnemyList.Clear();
        enemyMoveResetCount = 0;
        for (int count = 0; count < enemyMoveCount; count++)
        {
            if (moveEnemyList[count] != null)
            {
                moveResetEnemyList.Add(moveEnemyList[count]);
                enemyMoveResetCount += 1;
            }
        }
        moveEnemyList.Clear();
        for (int count = 0; count < enemyMoveResetCount; count++)
        {
            moveEnemyList.Add(moveResetEnemyList[count]);
        }
        enemyMoveCount = enemyMoveResetCount;
    }

    /// <summary>
    /// 敵のリストを返す
    /// </summary>
    /// <returns></returns>
    public List<ActionControllor> GetEnemyList()
    {
        return enemyList;
    }

    /// <summary>
    /// 攻撃する敵のパラメーター設定
    /// </summary>
    /// <param name="cnt"></param>
    public void SetParamAtkEnemyList(int cnt)
    {
        if (enemyAtkCount > 0)
        {
            atkEnemyList[cnt].SetUserAttackFlagOn();
            atkEnemyList[cnt].ActionStart(attackable);
            atkEnemyList[cnt].enemyAtk.AttackHit();
        }
    }

    /// <summary>
    /// 移動する敵のパラメーター設定
    /// </summary>
    public void SetParamMoveEnemyList()
    {
        for (int count = 0; count < enemyMoveCount; count++)
        {
            moveEnemyList[count].SetUserActFlagOn();
            moveEnemyList[count].ActionStart(attackable);
        }
    }

    /// <summary>
    /// 攻撃する敵の個数を返す
    /// </summary>
    /// <returns></returns>
    public int GetEnemyAtkCount()
    {
        return enemyAtkCount;
    }

    /// <summary>
    /// 敵全体のリセット
    /// </summary>
    public void ResetAllEnemyList()
    {
        ResetEnemyList();
        atkEnemyList.Clear();
        enemyAtkCount = 0;
        moveEnemyList.Clear();
        enemyMoveCount = 0;
    }

    /// <summary>
    /// 攻撃可能かのフラグを設定
    /// </summary>
    /// <param name="flg"></param>
    public  void ChangeAttackable(bool flg)
    {
        attackable = flg;
    }
}
