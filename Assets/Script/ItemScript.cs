﻿using System.Collections;
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
    public enum ItemType
    {
        NONE = 0,
        EQUIP,
        CONSUM,
        SPECIAL
    }
    public ItemStatusData ThisData;
    public void GetPosition(int iPix,int jPix)
    {
        ThisData.iPosition = iPix;
        ThisData.jPosition = jPix;
    }

    public virtual void GetDestroy()
    {
        //GetItemFlg = true;
        Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
