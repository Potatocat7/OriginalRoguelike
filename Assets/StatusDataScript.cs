using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        iThisNow = (int)this.transform.position.x;
        jThisNow = (int)this.transform.position.y;
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

        }
    }
}
