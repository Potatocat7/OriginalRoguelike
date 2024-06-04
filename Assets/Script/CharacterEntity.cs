using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//エディターで呼び出せるように
[CreateAssetMenu(fileName = "CharacterEntity", menuName = "Create CharacterEntity")]

public class CharacterEntity : ScriptableObject
{
    public Sprite IMAGE;
    public RuntimeAnimatorController ANIMECONTROLLER;
    public int MHP;
    public int ATK;
    public int LV;
    public int EXP;
    public int MEXP;
}
