using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//エディターで呼び出せるように
[CreateAssetMenu(fileName = "EnemyEntity", menuName = "Create EnemyEntity")]
public class EnemyEntity : ScriptableObject
{
    public int MHP;
    public int ATK;
    public int LV;
    public int EXP;
    public int MEXP;
}

