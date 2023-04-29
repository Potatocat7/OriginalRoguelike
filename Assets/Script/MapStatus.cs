using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStatus : MonoBehaviour
{
    [SerializeField] public GameObject _mapObject;
    [SerializeField] public ItemScript _Item;
    public void SetMapObject(GameObject map)
    {
        _mapObject = map;
    }
    public void SetItem(ItemScript item)
    {
        _mapObject = item.gameObject;
        _Item = item;
    }


}
