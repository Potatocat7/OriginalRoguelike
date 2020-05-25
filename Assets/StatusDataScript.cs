using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class StatusDataScript : MonoBehaviour {

    [SerializeField]
    int MaxHP,NowHP,iThisNow,jThisNow;

    public int Attack;
    // Use this for initialization
    void Start () {
        NowHP = MaxHP;

    }
    public bool CheckAttack(int iAttack, int jAttack)
    {
        //敵への攻撃が敵の移動後の座標で判定してしまっている
        iThisNow = this.GetComponent< ActionControllor>().iThisNow;
        jThisNow = this.GetComponent< ActionControllor>().jThisNow;
        if (iThisNow == iAttack && jThisNow == jAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HitDamage(int Damge)
    {
        NowHP -= Damge;
    }
    public void HealItem(int Heal)
    {
        NowHP += Heal;
        if (NowHP >= MaxHP)
        {
            NowHP = MaxHP;
        }
    }

    // Update is called once per frame
    void Update () {
        if(NowHP <= 0)
        {
            Destroy(gameObject);
            MapGenerator.EnemyCount -= 1;
        }
    }
}
