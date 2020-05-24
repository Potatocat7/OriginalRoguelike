using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_1 : MonoBehaviour {

    void AttackHitcheck(int iAttack,int jAttack)
    {
        //複数or0だったときの処理が必要？
        if (MapGenerator.EnemyCount >= 1)
        {
            GameObject Enemy = GameObject.Find("EnemyPrefab(Clone)");
            if (Enemy.GetComponent<StatusDataScript>().CheckAttack(iAttack, jAttack) == true)
            {
                Enemy.GetComponent<StatusDataScript>().HitDamage(this.GetComponent<StatusDataScript>().Attack);
            }
            else
            {

            }
        }
    }
    public void AttackAreaSet()
    {
        int iThisNow = (int)this.transform.position.x;
        int jThisNow = (int)this.transform.position.y;

        switch(this.GetComponent<ActionControllor>().thisNowDirection)
        {
            case ActionControllor.Direction.UP:
                AttackHitcheck(iThisNow - 1, jThisNow + 1);
                AttackHitcheck(iThisNow , jThisNow + 1);
                AttackHitcheck(iThisNow + 1, jThisNow + 1);
                break;
            case ActionControllor.Direction.UP_LEFT:
                AttackHitcheck(iThisNow , jThisNow + 1);
                AttackHitcheck(iThisNow - 1, jThisNow + 1);
                AttackHitcheck(iThisNow - 1, jThisNow );
                break;
            case ActionControllor.Direction.UP_RIGHT:
                AttackHitcheck(iThisNow, jThisNow + 1);
                AttackHitcheck(iThisNow + 1, jThisNow + 1);
                AttackHitcheck(iThisNow + 1, jThisNow);
                break;
            case ActionControllor.Direction.LEFT:
                AttackHitcheck(iThisNow - 1, jThisNow + 1);
                AttackHitcheck(iThisNow - 1, jThisNow);
                AttackHitcheck(iThisNow - 1, jThisNow - 1);
                break;
            case ActionControllor.Direction.RIGHT:
                AttackHitcheck(iThisNow + 1, jThisNow + 1);
                AttackHitcheck(iThisNow + 1, jThisNow);
                AttackHitcheck(iThisNow + 1, jThisNow - 1);
                break;
            case ActionControllor.Direction.DOWN:
                AttackHitcheck(iThisNow - 1, jThisNow - 1);
                AttackHitcheck(iThisNow , jThisNow - 1);
                AttackHitcheck(iThisNow + 1, jThisNow - 1);
                break;
            case ActionControllor.Direction.DOWN_LEFT:
                AttackHitcheck(iThisNow - 1, jThisNow);
                AttackHitcheck(iThisNow - 1, jThisNow - 1);
                AttackHitcheck(iThisNow , jThisNow - 1);
                break;
            case ActionControllor.Direction.DOWN_RIGHT:
                AttackHitcheck(iThisNow + 1, jThisNow);
                AttackHitcheck(iThisNow + 1, jThisNow - 1);
                AttackHitcheck(iThisNow, jThisNow - 1);
                break;
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
