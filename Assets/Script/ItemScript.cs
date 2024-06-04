using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct ItemStatusData
{
    public ItemScript.ItemType Type;
    public int Attack;
    public int Mhp;
    public int Hp;
    public string Name;
    public int ListNum;
    public bool EquipFlg;
    public int iPosition;
    public int jPosition;
}
[System.Serializable]
public class ItemScript : MonoBehaviour
{
    /// <summary>
    /// アイテムのタイプ
    /// </summary>
    public enum ItemType
    {
        NONE = 0,
        EQUIP,
        CONSUM,
        SPECIAL
    }
    /// <summary>アイテムモデル</summary>
    private ItemModel itemModel;
    /// <summary>このアイテムのステータス</summary>
    public ItemStatusData ThisData;

    /// <summary>
    /// アイテム初期化
    /// </summary>
    /// <param name="iPix"></param>
    /// <param name="jPix"></param>
    public void Init(int iPix,int jPix,int num)
    {
        itemModel = new ItemModel(num);
        ThisData.Attack = itemModel.ATTACK;
        ThisData.Mhp = itemModel.MHP;
        ThisData.Hp = itemModel.ADDHP;
        ThisData.Name = itemModel.NAME;
        //ThisData.ListNum;
        //ThisData.EquipFlg;
        ThisData.iPosition = iPix;
        ThisData.jPosition = jPix;
    }

    /// <summary>
    /// 拾った時の処理
    /// </summary>
    public virtual void GetDestroy()
    {
        Destroy(gameObject);
    }
}
