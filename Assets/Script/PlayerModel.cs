using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public Sprite IMAGE;
    public RuntimeAnimatorController ANIMECONTROLLER;
    public int MHP;
    public int ATK;
    public int LV;
    public int EXP;
    public int MEXP;
    public PlayerModel(int num)
    {
        ///TODO:モデルを選択状況から切り替え
        ///TODO:いずれは敵Entityと統一
        PlayerEntity playerEntity = Resources.Load<PlayerEntity>("Player/Player_"+ num.ToString());
        IMAGE = playerEntity.IMAGE;
        ANIMECONTROLLER = playerEntity.ANIMECONTROLLER;
        MHP = playerEntity.MHP;
        ATK = playerEntity.ATK;
        LV = playerEntity.LV;
        EXP = playerEntity.EXP;
        MEXP = playerEntity.MEXP;
    }

}
