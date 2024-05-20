using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StatusDataScript : MonoBehaviour 
{

    [SerializeField]
    private Status _charaState;
    [SerializeField]
    int MaxHP,NowHP,iThisNow,jThisNow,dispDamege,SPcount,expState;
    [SerializeField]
    Text DamageDisplay;
    public int Attack;
    private int _level = 1;
    [SerializeField]
    private int _experienceNow = 0;
    [SerializeField]
    private int _experienceMax = 5;
    private StatusDataScript _playerState;

    public EnemyModel enemyModel;

    public int GetHPnow()
    {
        return _charaState.HP;
    }
    public int GetAttack()
    {
        return _charaState.ATK;
    }

    public void AddState(ItemStatusData data)
    {
        _charaState.ATK += data.Attack;
        _charaState.MHP += data.Mhp;
        _charaState.HP += data.Hp;
        if (_charaState.HP >= _charaState.MHP)
        {
            _charaState.HP = _charaState.MHP;
        }
    }
    public void SubState(ItemStatusData data)
    {
        _charaState.ATK -= data.Attack;
        _charaState.MHP -= data.Mhp;
        _charaState.HP -= data.Hp;
    }
    public void GetPlayerState(StatusDataScript player)
    {
        _playerState = player;
    }
    public void ExperienceUp(int exp)
    {
        _charaState.EXP += exp;
        if (_charaState.EXP > _charaState.MEXP)
        {
            LevelUp();
        }
    }
    void LevelUp()
    {
        _charaState.LV += 1;
        _charaState.ATK += 1;
        _charaState.MHP += 20;
        _charaState.HP += 20;
        _charaState.MEXP = 5 * _charaState.LV;
        _charaState.EXP = 0;
    }
    // Use this for initialization
    void Start () {
        if (this.tag == "Player")
        {
            //GetComponentをなくしていきたい
            SPcount = 0;
            SaveDataScript Save;
            StatusDataScript StatusData = this.GetComponent<StatusDataScript>();
            Save = GameObject.Find("SaveDataObject").GetComponent<SaveDataScript>();
            if (Save.GetFlg() == true)
            {
                StatusData.SetNow(Save.playerNowData);
                //Mapobj[randomiPix, randomjPix].GetComponent<DisplayScript>().SetFloor(Save.GetComponent<SaveDataScript>().FloorCount);

            }
            else
            {
                _charaState.LV = 1;
                _charaState.ATK = Attack;
                _charaState.MHP = MaxHP;
                _charaState.HP = MaxHP;
                _charaState.MEXP = _experienceMax;
                _charaState.EXP = 0;
            }
        }
        else
        {
            enemyModel = new EnemyModel();
            _charaState.LV = enemyModel.LV;
            _charaState.ATK = enemyModel.ATK;
            _charaState.MHP = enemyModel.MHP;
            _charaState.HP = enemyModel.MHP;
            _charaState.MEXP = enemyModel.MEXP;
            _charaState.EXP = 0;

            //_charaState.LV = 1;
            //_charaState.ATK = Attack;
            //_charaState.MHP = MaxHP;
            //_charaState.HP = MaxHP;
            //_charaState.MEXP = _experienceMax;
            //_charaState.EXP = 0;
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
        _charaState.HP -= Damge;
        dispDamege = Damge;
        StartCoroutine("coHitDameDisp");
    }
    public Status GetNow()
    {
        return _charaState;
    }

    public void SetNow(Status Data)
    {
        _charaState = Data;
    }

    public Status GetStateData()
    {
        return _charaState;
    }
    public void HealItem(int Heal)
    {
        _charaState.HP += Heal;
        if (_charaState.HP >= _charaState.MHP)
        {
            _charaState.HP = _charaState.MHP;
        }
    }
    public void SetSPcount(int count)
    {
        SPcount += count;
    }
    public int GetSPcount()
    {
        return SPcount;
    }
    ///TODO:Updateである必要がない。
    ///コールバック等を使って攻撃ヒット時にチェックを行うようにすればいける
    ///毎度リセット処理しないとエラーになるのは個々のデリートによる敵Listとの差異が原因
    void Update () {
        //UpdateでGetComponentをなくしていきたい
        iThisNow = this.GetComponent<ActionControllor>().iThisNow;
        jThisNow = this.GetComponent<ActionControllor>().jThisNow;
        if (_charaState.HP <= 0)
        {
            if (this.tag == "Player")
            {
                //シーンに行く前にデータの一部をJSONファイルで保存しておく
                //SaveDataScriptがEndSceneまで残っているのでそこからDataを保存させる
                SceneManager.LoadScene("EndScene");
            }
            else
            {   //敵オブジェクトの場合
                //デリート前にランダムでアイテムを生成
                //最初はランダムではなく確定で生成してみる
                //MapGeneratorから関数をひっぱればいける？
                _playerState.ExperienceUp(expState);
                //TODO:このままorGameManager経由にする
                if (GameControllor.Instance.CheckItemPosition(iThisNow, jThisNow))
                {
                    //TODO:このままorGameManager経由にする
                    MapGenerator.Instance.SetDropItemObj(iThisNow, jThisNow);
                }
                Destroy(gameObject);
            }
        }
    }
}
