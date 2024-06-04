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
    public ItemModel(int num)
    {
        ItemEntity itemEntity = Resources.Load<ItemEntity>("Item/Item_" + num.ToString());
        IMAGE = itemEntity.IMAGE;
        TYPE = itemEntity.TYPE;
        NAME = itemEntity.NAME;
        ATTACK = itemEntity.ATTACK;
        MHP = itemEntity.MHP;
        ADDHP = itemEntity.ADDHP;
    }

}
