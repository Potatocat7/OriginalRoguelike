using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//エディターで呼び出せるように
[CreateAssetMenu(fileName = "ItemEntity", menuName = "Create ItemEntity")]
public class ItemEntity : ScriptableObject
{
    public Sprite IMAGE;
    public ItemScript.ItemType TYPE;
    public string NAME;
    public int ATTACK;
    public int MHP;
    public int ADDHP;
}
