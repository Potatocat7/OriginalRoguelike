using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStatus : MonoBehaviour
{
    /// <summary>マップ</summary>
    [SerializeField] public GameObject _mapObject;
    /// <summary>アクションコントローラー</summary>
    [SerializeField] public ActionControllor _actCtrl;
    /// <summary>アイテム</summary>
    [SerializeField] public ItemScript _Item;

    /// <summary>
    /// マップの設定
    /// </summary>
    /// <param name="map"></param>
    public void SetMapObject(GameObject map)
    {
        _mapObject = map;
    }

    /// <summary>
    /// アクションコントローラーの設定
    /// </summary>
    /// <param name="act"></param>
    public void SetActCtrl(ActionControllor act)
    {
        _actCtrl = act;
    }

    /// <summary>
    /// アイテムの設定
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(ItemScript item)
    {
        _mapObject = item.gameObject;
        _Item = item;
    }


}
