using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class StatusDataScript : MonoBehaviour {

    [SerializeField]
    int MaxHP,NowHP,iThisNow,jThisNow,dispDamege;
    [SerializeField]
    Text DamageDisply;
    public int Attack;
    // Use this for initialization
    void Start () {
        NowHP = MaxHP;
        DamageDisply.text = "";
        //DamageDisply = this.transform.GetChild(0).GetComponent<Text>();
        // DamageDisply = this.transform.GetChildCount(0);

    }
    public bool CheckAttack(int iAttack, int jAttack)
    {
        //敵への攻撃が敵の移動後の座標で判定してしまっている
        if (iThisNow == iAttack && jThisNow == jAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator coHitDameDisp()
    {
        DamageDisply.text = dispDamege.ToString();

        yield return new WaitForSeconds(0.2f);

        DamageDisply.text = "";
    }
    public void HitDamage(int Damge)
    {
        NowHP -= Damge;
        dispDamege = Damge;
        StartCoroutine("coHitDameDisp");
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
        iThisNow = this.GetComponent<ActionControllor>().iThisNow;
        jThisNow = this.GetComponent<ActionControllor>().jThisNow;
        if (NowHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
