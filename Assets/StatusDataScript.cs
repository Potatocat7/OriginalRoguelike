using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StatusDataScript : MonoBehaviour {

    [SerializeField]
    int MaxHP,NowHP,iThisNow,jThisNow,dispDamege;
    [SerializeField]
    Text DamageDisplay;
    public int Attack;
    // Use this for initialization
    void Start () {
        if (this.tag == "Player")
        {
            GameObject Save;
            Save = GameObject.Find("SaveDataObject");
            if (Save.GetComponent<SaveDataScript>().GetFlg() == true)
            {
                this.GetComponent<StatusDataScript>().SetNowHP(Save.GetComponent<SaveDataScript>().PlayerHpNowData);
                //Mapobj[randomiPix, randomjPix].GetComponent<DisplayScript>().SetFloor(Save.GetComponent<SaveDataScript>().FloorCount);

            }
            else
            {
                NowHP = MaxHP;  
            }
        }
        else
        {
            NowHP = MaxHP;
        }
        DamageDisplay.text = "";
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
        DamageDisplay.text = dispDamege.ToString();

        yield return new WaitForSeconds(0.2f);

        DamageDisplay.text = "";
    }
    public void HitDamage(int Damge)
    {
        NowHP -= Damge;
        dispDamege = Damge;
        StartCoroutine("coHitDameDisp");
    }
    public int GetNowHP()
    {
        return NowHP;
    }
    public void SetNowHP(int HpData)
    {
        NowHP = HpData;
    }
    public int GetMaxHP()
    {
        return MaxHP;
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
            if (this.tag == "Player")
            {
                SceneManager.LoadScene("EndScene");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
