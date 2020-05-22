using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_1 : MonoBehaviour {

    void AttackHitcheck(int iAttack,int jAttack)
    {
        //複数or0だったときの処理が必要？
        GameObject Enemy = GameObject.Find("EnemyPrefab(Clone)");
        if (Enemy.GetComponent<StatusDataScript>().CheckAttack(iAttack, jAttack) == true)
        {
            Enemy.GetComponent<StatusDataScript>().HitDamage(this.GetComponent<StatusDataScript>().Attack);
        }
        else
        {

        }
    }
    public void AttackAreaSet()
    {
        int iThisNow = (int)this.transform.position.x;
        int jThisNow = (int)this.transform.position.y;

        //仮設置：引数に方向をもってきて攻撃方向を設定してから↑の処理に
        AttackHitcheck(iThisNow + 1, jThisNow + 1);
        AttackHitcheck(iThisNow + 1, jThisNow);
        AttackHitcheck(iThisNow + 1, jThisNow - 1);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
