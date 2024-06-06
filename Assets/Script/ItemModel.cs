using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel : MonoBehaviour
{
    public Sprite IMAGE;
    public ItemScript.ItemType TYPE;
    public string NAME;
    public int ATTACK;
    public int MHP;
    public int ADDHP;

    private int num;
    public ItemModel(ItemScript.ItemType type)
    {
        ///TODO:同種タイプで分岐する情報も必要（CONSUMで２種目を用意するとき）
        switch (type)
        {
            case ItemScript.ItemType.CONSUM:
                num = 1;
                break;
            case ItemScript.ItemType.EQUIP:
                num = 2;
                break;
            case ItemScript.ItemType.SPECIAL:
                num = 3;
                break;
            default:
                num = 1;
                break;
        }


        ItemEntity itemEntity = Resources.Load<ItemEntity>("Item/Item_" + num.ToString());
        IMAGE = itemEntity.IMAGE;
        TYPE = itemEntity.TYPE;
        NAME = itemEntity.NAME;
        ATTACK = itemEntity.ATTACK;
        MHP = itemEntity.MHP;
        ADDHP = itemEntity.ADDHP;
    }

}
