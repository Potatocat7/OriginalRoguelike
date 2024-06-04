using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorModel : MonoBehaviour
{
    public Sprite IMAGE;
    public RuntimeAnimatorController ANIMECONTROLLER;
    public int MHP;
    public int ATK;
    public int LV;
    public int EXP;
    public int MEXP;

    public CharactorModel(string folder,int num)
    {
        CharacterEntity charaEntity = Resources.Load<CharacterEntity>(folder+ "/Chara_" + num.ToString());
        IMAGE = charaEntity.IMAGE;
        ANIMECONTROLLER = charaEntity.ANIMECONTROLLER;
        MHP = charaEntity.MHP;
        ATK = charaEntity.ATK;
        LV = charaEntity.LV;
        EXP = charaEntity.EXP;
        MEXP = charaEntity.MEXP;
    }
}
