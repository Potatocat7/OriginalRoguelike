using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//エディターで呼び出せるように
[CreateAssetMenu(fileName = "PlayerEntity", menuName = "Create PlayerEntity")]
public class PlayerEntity : ScriptableObject
{
    public Sprite IMAGE;
    public RuntimeAnimatorController ANIMECONTROLLER;
    public int MHP;
    public int ATK;
    public int LV;
    public int EXP;
    public int MEXP;
}
