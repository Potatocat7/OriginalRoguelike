using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    public int MHP;
    public int ATK;
    public int LV;
    public int EXP;
    public int MEXP;

    //public Sprite Image;
    //public string Name;
    //public int Number;
    //public int Mass;
    ///// <summary>果物の大きさ（scale） </summary>
    //public float LimitScale;
    ///// <summary>落下中フラグ </summary>
    //public bool Falling { get; set; }
    ///// <summary>スケール変更の倍率 </summary>
    //private const float ScaleMagnification = 0.5f;

    public EnemyModel()
    {
        EnemyEntity enemyEntity = Resources.Load<EnemyEntity>("Enemy/Enemy_1");
        MHP = enemyEntity.MHP;
        ATK = enemyEntity.ATK;
        LV = enemyEntity.LV;
        EXP = enemyEntity.EXP;
        MEXP = enemyEntity.MEXP;
    }
}
