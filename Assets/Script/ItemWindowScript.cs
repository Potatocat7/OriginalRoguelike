using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemWindowScript : MonoBehaviour
{
    /// <summary>アイテムウィンドウ自体</summary>
    [SerializeField] private GameObject _thisWindowPanel;
    /// <summary>アイテムプレハブ</summary>
    [SerializeField] private ItemPrefabScript _itemPrefabObj;
    /// <summary>使用時のポップウィンドウ</summary>
    [SerializeField] private RectTransform _itemPopwindowRectTransform;
    /// <summary>RectTransform</summary>
    [SerializeField] private RectTransform _thisPanelRectTransform;
    /// <summary>所持アイテムリスト</summary>
    public List<ItemPrefabScript> _gotItemList;// = new List<ItemPrefabScript>();
    /// <summary>所持アイテムのステータス</summary>
    public List<ItemStatusData> _saveItemList;// = new List<ItemPrefabScript>();
    /// <summary>非表示中の位置</summary>　///TODO:待機位置を画面に合わせて変更できるようにしたい
    private Vector3 _offPosition = new Vector3(0, 500, 0);
    /// <summary>ポップウィンドウ非表示</summary>
    private Vector3 _offPositionPopwin = new Vector3(700, 700, 0);
    /// <summary>ポップウィンドウ表示</summary>
    private Vector3 _onPositionPopwin = new Vector3(250, -125, 0);
    /// <summary>リストナンバー</summary>
    private int _listNum;
    /// <summary>アイテムウィンドウ選択中フラグ</summary>
    private bool ItemWindowflg;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="EquipItem"></param>
    public void Init(Action<ItemStatusData> EquipItem = null)
    {
        ItemWindowflg = false;
        if (ItemWindowflg == false)
        {
            ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.GAME);
            _thisPanelRectTransform.localPosition = _offPosition;
        }
        _saveItemList = SaveDataScript.Instance._saveItemList;
        for (int i = 0; i < _saveItemList.Count; i++)
        {
            AddGotItemPrefab(_saveItemList[i]);
        }
        //セーブのリストをリセット
        SaveDataScript.Instance._saveItemList = new List<ItemStatusData>();
        SetupItemState((itemdata)=> {
            EquipItem.Invoke(itemdata);
        });
    }

    /// <summary>
    /// アイテム装備処理
    /// </summary>
    /// <param name="EquipItem"></param>
    public void SetupItemState(Action<ItemStatusData> EquipItem = null)
    {
        for (int i = 0; i < _gotItemList.Count; i++)
        {
            //装備していたアイテムは装備させる
            if (_gotItemList[i].itemSaveData.EquipFlg == true)
            {
                EquipItem.Invoke(_gotItemList[i].itemSaveData);
            }
        }
    }

    /// <summary>
    /// 消費時の処理と装備時の処理を追加
    /// </summary>
    public void CheckEquipItem()
    {
        for (int i = 0; i < _gotItemList.Count; i++)
        {
            _gotItemList[i].OffEquipItem();
        }
    }

    /// <summary>
    /// 消費時のリスト整理
    /// </summary>
    /// <param name="deletenum"></param>
    public void OrganizeList(int deletenum)
    {
        //削除したプレハブをリストから外し数値の入れ替え
        _gotItemList.RemoveAt(deletenum);
        ButtonActionManagerScript.Instance.RemoveItemButtonList(deletenum);
        for (int i = 0; i<_gotItemList.Count;i++)
        {
            _gotItemList[i].GetListNum(i);
            _gotItemList[i].transform.localPosition = new Vector3(0, 150f - 50.0f * i, 0);
        }
    }

    /// <summary>
    /// アイテム使用
    /// </summary>
    public void ActionButton()
    {
        ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMWINDOW);
        _gotItemList[_listNum].ActionItem();
        _itemPopwindowRectTransform.localPosition = _offPositionPopwin;
    }

    /// <summary>
    /// ポップウィンドウ非表示
    /// </summary>
    public void OffPopwindow()
    {
        ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMWINDOW);
        _itemPopwindowRectTransform.localPosition = _offPositionPopwin;
    }

    /// <summary>
    /// ポップウィンドウ表示
    /// </summary>
    public void OnPopwindow(int listNum)
    {
        ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMPOP);
        _listNum = listNum;
        _itemPopwindowRectTransform.localPosition = _onPositionPopwin;
    }
    
    /// <summary>
    /// アイテム取得処理
    /// </summary>
    /// <param name="Data"></param>
    public void AddGotItemPrefab(ItemStatusData Data)
    {
        Vector3 pos = new Vector3(0, 150f -50.0f * _gotItemList.Count, 0);
        ItemPrefabScript prefab = (ItemPrefabScript)Instantiate(_itemPrefabObj, _thisWindowPanel.transform.position, Quaternion.identity, _thisWindowPanel.transform);
        prefab.gameObject.transform.localPosition += pos;
        Data.ListNum = _gotItemList.Count;
        prefab.GetThisState(Data);
        _gotItemList.Add(prefab);
        ButtonActionManagerScript.Instance.AddItemButtonList(prefab.itemButton);
    }

    /// <summary>
    /// アイテムウィンドウ表示/非表示
    /// </summary>
    public void ChangeItemWindow()
    {
        if (ItemWindowflg == false)
        {
            ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMWINDOW);
            _thisPanelRectTransform.localPosition = Vector3.zero;
            ItemWindowflg = true;
        }
        else
        {
            ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.GAME);
            _thisPanelRectTransform.localPosition = _offPosition;
            ItemWindowflg = false;
        }
    }
}
