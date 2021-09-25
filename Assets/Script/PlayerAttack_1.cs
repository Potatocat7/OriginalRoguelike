using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerAttack_1 : MonoBehaviour {
    [SerializeField]
    private int iThisNow, jThisNow;
    [SerializeField]
    private int iThisAtkArea1, jThisAtkArea1, iThisAtkArea2, jThisAtkArea2, iThisAtkArea3, jThisAtkArea3;
    [SerializeField]
    private GameControllor Contollor;

    void AttackHitcheck(int iAttack,int jAttack)
    {
        //複数or0だったときの処理が必要？
        if (MapGenerator.EnemyCount >= 1)
        {
            for (int count = 0; count < Contollor.EnemyCount; count++)
            {
            //GameObject Enemy = GameObject.Find("EnemyPrefab(Clone)");
            //if (Enemy.GetComponent<StatusDataScript>().CheckAttack(iAttack, jAttack) == true)
                if (Contollor.EnemyList[count].GetComponent<StatusDataScript>().CheckAttack(iAttack, jAttack) == true)
                {
                    Contollor.EnemyList[count].GetComponent<StatusDataScript>().HitDamage(this.GetComponent<StatusDataScript>().Attack);
                    //Enemy.GetComponent<StatusDataScript>().HitDamage(this.GetComponent<StatusDataScript>().Attack);
                }
                else
                {

                }
            }

        }
    }
    void AttackHit()
    {
        AttackHitcheck(iThisAtkArea1, jThisAtkArea1);
        AttackHitcheck(iThisAtkArea2, jThisAtkArea2);
        AttackHitcheck(iThisAtkArea3, jThisAtkArea3);
    }
    void SpAttackHit()
    {
        AttackHitcheck(iThisNow + 1, jThisNow + 1);
        AttackHitcheck(iThisNow + 1, jThisNow - 1);
        AttackHitcheck(iThisNow + 1, jThisNow    );
        AttackHitcheck(iThisNow    , jThisNow + 1);
        AttackHitcheck(iThisNow    , jThisNow - 1);
        AttackHitcheck(iThisNow - 1, jThisNow + 1);
        AttackHitcheck(iThisNow - 1, jThisNow - 1);
        AttackHitcheck(iThisNow - 1, jThisNow    );
    }

    void AttackAreaSet()
    {

        switch (this.GetComponent<ActionControllor>().thisNowDirection)
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
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        //Contollor = GameObject.Find("GameControllor");
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
        iThisNow = this.GetComponent<ActionControllor>().iThisNow;
        jThisNow = this.GetComponent<ActionControllor>().jThisNow;
        AttackAreaSet();
    }
}
