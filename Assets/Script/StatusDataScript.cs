using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StatusDataScript : MonoBehaviour 
{
    /// <summary>ステータス</summary>
    [SerializeField]
    private Status _charaState;
    /// <summary>HP・位置・ダメージ・SP回数・経験値</summary>
    [SerializeField]
    int MaxHP,NowHP,iThisNow,jThisNow,dispDamege,SPcount,expState;
    /// <summary>ダメージ表示</summary>
    [SerializeField]
    Text DamageDisplay;
    /// <summary>攻撃力</summary>
    public int Attack;
    /// <summary>経験値最大値</summary>
    [SerializeField]
    private int _experienceMax = 5;
    /// <summary>プレイヤーステータス</summary>
    private StatusDataScript _playerState;
    /// <summary>モデル</summary>
    private CharactorModel charactorModel;
    /// <summary>エンディングフラグ</summary>
    private bool endingFlg;
    /// <summary>プレイヤーフォルダ</summary>
    private static string PLAYERFOLDER = "Player";
    /// <summary>エネミーフォルダ</summary>
    private static string ENEMYFOLDER = "Enemy";

    /// <summary>
    /// プレイヤー現在ＨＰ取得
    /// </summary>
    /// <returns></returns>
    public int GetHPnow()
    {
        return _charaState.HP;
    }

    /// <summary>
    /// プレイヤー攻撃力取得
    /// </summary>
    /// <returns></returns>
    public int GetAttack()
    {
        return _charaState.ATK;
    }

    /// <summary>
    /// ステータス増加処理
    /// </summary>
    /// <param name="data"></param>
    public void AddState(ItemStatusData data)
    {
        _charaState.ATK += data.Attack;
        _charaState.MHP += data.Mhp;
        _charaState.HP += data.Hp;
        if (_charaState.HP >= _charaState.MHP)
        {
            _charaState.HP = _charaState.MHP;
        }
        GameManager.Instance.UpdateDisplay();
    }

    /// <summary>
    /// 装備アイテム装備時の増加処理
    /// </summary>
    /// <param name="data"></param>
    public void SubState(ItemStatusData data)
    {
        _charaState.ATK -= data.Attack;
        _charaState.MHP -= data.Mhp;
        _charaState.HP -= data.Hp;
        GameManager.Instance.UpdateDisplay();
    }

    /// <summary>
    /// プレイヤー情報取得
    /// </summary>
    /// <param name="player"></param>
    public void GetPlayerState(StatusDataScript player)
    {
        _playerState = player;
    }

    /// <summary>
    /// 経験値獲得処理
    /// </summary>
    /// <param name="exp"></param>
    public void ExperienceUp(int exp)
    {
        _charaState.EXP += exp;
        if (_charaState.EXP >= _charaState.MEXP)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// レベルアップ処理
    /// </summary>
    void LevelUp()
    {
        _charaState.LV += 1;
        _charaState.ATK += 1;
        _charaState.MHP += 20;
        _charaState.HP += 20;
        _charaState.MEXP = 5 * _charaState.LV;
        _charaState.EXP = 0;
        GameManager.Instance.UpdateDisplay();
    }

    public void Init (Action<Sprite,RuntimeAnimatorController> setAnimator = null) 
    {
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
            }
            else
            {
                int num;
                switch (SaveCharaSelect.Instance.CharaNumber)
                {
                    case PlayerSelector.PlayerKind.Player_1:
                        num = 1;
                        break;
                    case PlayerSelector.PlayerKind.Player_2:
                        num = 2;
                        break;
                    default:
                        num = 1;
                        break;
                }
                charactorModel = new CharactorModel(PLAYERFOLDER, num);
                _charaState.LV = charactorModel.LV;
                _charaState.ATK = charactorModel.ATK;
                Attack = charactorModel.ATK;
                _charaState.MHP = charactorModel.MHP;
                _charaState.HP = charactorModel.MHP;
                _charaState.MEXP = charactorModel.MEXP;
                _charaState.EXP = 0;

                //_charaState.LV = 1;
                //_charaState.ATK = Attack;
                //_charaState.MHP = MaxHP;
                //_charaState.HP = MaxHP;
                //_charaState.MEXP = _experienceMax;
                //_charaState.EXP = 0;
                setAnimator.Invoke(charactorModel.IMAGE, charactorModel.ANIMECONTROLLER);
            }
            endingFlg = false;
            //GameManager.Instance.UpdateDisplay();
        }
        else
        {
            charactorModel = new CharactorModel(ENEMYFOLDER,1);
            _charaState.LV = charactorModel.LV;
            _charaState.ATK = charactorModel.ATK;
            Attack = charactorModel.ATK; 
            _charaState.MHP = charactorModel.MHP;
            _charaState.HP = charactorModel.MHP;
            _charaState.MEXP = charactorModel.MEXP;
            _charaState.EXP = 0;
            setAnimator.Invoke(charactorModel.IMAGE, charactorModel.ANIMECONTROLLER);
       }
        DamageDisplay.text = "";
    }

    /// <summary>
    /// 攻撃範囲判定処理
    /// </summary>
    /// <param name="iAttack"></param>
    /// <param name="jAttack"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 攻撃ヒット表示処理
    /// </summary>
    /// <returns></returns>
    IEnumerator coHitDameDisp()
    {
        DamageDisplay.text = dispDamege.ToString();

        yield return new WaitForSeconds(0.2f);

        DamageDisplay.text = "";
    }

    /// <summary>
    /// 攻撃ヒット処理
    /// </summary>
    /// <param name="Damge"></param>
    /// <param name="dropitem"></param>
    public void HitDamage(int Damge,Action<int,int> dropitem = null)
    {
        _charaState.HP -= Damge;
        GameManager.Instance.UpdateDisplay();
        dispDamege = Damge;
        StartCoroutine("coHitDameDisp");
        CheckDeath((ipos,jpos)=> {
            dropitem.Invoke(ipos,jpos);
        });
    }

    /// <summary>
    /// 現状ステータスの更新
    /// </summary>
    /// <param name="Data"></param>
    public void SetNow(Status Data)
    {
        _charaState = Data;
    }

    /// <summary>
    /// 現在ステータスを返す
    /// </summary>
    /// <returns></returns>
    public Status GetStateData()
    {
        return _charaState;
    }

    /// <summary>
    /// 回復アイテム取得
    /// </summary>
    /// <param name="Heal"></param>
    public void HealItem(int Heal)
    {
        _charaState.HP += Heal;
        if (_charaState.HP >= _charaState.MHP)
        {
            _charaState.HP = _charaState.MHP;
            GameManager.Instance.UpdateDisplay();
        }
    }

    /// <summary>
    /// SPポイント加算
    /// </summary>
    /// <param name="count"></param>
    public void SetSPcount(int count)
    {
        SPcount += count;
    }

    /// <summary>
    /// SPポイントを返す
    /// </summary>
    /// <returns></returns>
    public int GetSPcount()
    {
        return SPcount;
    }

    /// <summary>
    /// エンディングフラグ設定
    /// </summary>
    public void SetEndingFlg()
    {
        endingFlg = true;
    }

    /// <summary>
    /// 現在位置設定
    /// </summary>
    /// <param name="inow"></param>
    /// <param name="jnow"></param>
    public void SetThisPosition(int inow,int jnow)
    {
        iThisNow = inow;
        jThisNow = jnow;
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    /// <param name="drop"></param>
    private void CheckDeath(Action<int,int> drop = null)
    {
        if (_charaState.HP <= 0)
        {
            if (this.tag == "Player")
            {
                if (endingFlg == true)
                {
                    //シーンに行く前にデータの一部をJSONファイルで保存しておく
                    SceneManager.LoadScene("EndScene");
                }
            }
            else
            {   //敵オブジェクトの場合
                //TODO:デリート前にランダムでアイテムを生成
                _playerState.ExperienceUp(expState);
                drop.Invoke(iThisNow, jThisNow);
                Destroy(gameObject);
            }
        }
    }
}
