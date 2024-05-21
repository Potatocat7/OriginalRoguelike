using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemWindowScript : MonoBehaviour
{
    [SerializeField] private GameObject _thisWindowPanel;
    [SerializeField] private ItemPrefabScript _itemPrefabObj;
    [SerializeField] private RectTransform _itemPopwindowRectTransform;
    [SerializeField] private RectTransform _thisPanelRectTransform;

    public List<ItemPrefabScript> _gotItemList;// = new List<ItemPrefabScript>();
    public List<ItemStatusData> _saveItemList;// = new List<ItemPrefabScript>();
    private Vector3 _offPosition = new Vector3(0, 500, 0);
    private Vector3 _offPositionPopwin = new Vector3(700, 700, 0); 
    private Vector3 _onPositionPopwin = new Vector3(250, -125, 0);
    private int _listNum;
    private bool ItemWindowflg;

    ////シングルトン化
    //private static ItemWindowScript mInstance;
    //public static ItemWindowScript Instance
    //{
    //    get
    //    {
    //        return mInstance;
    //    }
    //}
    //void Awake()
    //{
    //    if (Instance != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    mInstance = this;
    //    //DontDestroyOnLoad(gameObject);
    //    _saveItemList = SaveDataScript.Instance._saveItemList;
    //    for (int i = 0; i < _saveItemList.Count; i++)
    //    {
    //        AddGotItemPrefab(_saveItemList[i]);
    //        ////装備していたアイテムは装備させる
    //        //if (_saveItemList[i].EquipFlg == true)
    //        //{
    //        //    GameControllor.Instance.AddItemState(_saveItemList[i]);
    //        //}
    //    }
    //    //セーブのリストをリセット
    //    SaveDataScript.Instance._saveItemList= new List<ItemStatusData>();

    //}
    public void Init(Action<ItemStatusData> EquipItem = null)
    {
        ItemWindowflg = false;
        if (ItemWindowflg == false)
        {
            //_thisWindowPanel.SetActive(false);
            ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.GAME);
            _thisPanelRectTransform.localPosition = _offPosition;
        }
        _saveItemList = SaveDataScript.Instance._saveItemList;
        for (int i = 0; i < _saveItemList.Count; i++)
        {
            AddGotItemPrefab(_saveItemList[i]);
            ////装備していたアイテムは装備させる
            //if (_saveItemList[i].EquipFlg == true)
            //{
            //    GameControllor.Instance.AddItemState(_saveItemList[i]);
            //}
        }
        //セーブのリストをリセット
        SaveDataScript.Instance._saveItemList = new List<ItemStatusData>();
        SetupItemState((itemdata)=> {
            EquipItem.Invoke(itemdata);
        });
    }
    public void SetupItemState(Action<ItemStatusData> EquipItem = null)
    {
        for (int i = 0; i < _gotItemList.Count; i++)
        {
            //装備していたアイテムは装備させる
            if (_gotItemList[i].itemSaveData.EquipFlg == true)
            {
                //GameManager.Instance.GetPlayerManager().AddItemState(_gotItemList[i].itemSaveData);
                EquipItem.Invoke(_gotItemList[i].itemSaveData);
            }
        }
    }
    //消費時の処理と装備時の処理を追加
    public void CheckEquipItem()
    {
        for (int i = 0; i < _gotItemList.Count; i++)
        {
            _gotItemList[i].OffEquipItem();
        }
    }
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
    public void ActionButton()
    {
        //アイテムを仕様するための処理
        //_gotItemList[_listNum]

        //やることメモ
        //・指定リストのアイテム
        //・装備ならEを表示
        //・装備でのステータスの追加
        //・装備なら既に装備してるEを非表示
        //・装備していた時の追加ステータスの減算
        //・消費ならステータスの追加
        //・消費ならアイテムプレハブのデストロイ
        //・消費ならデストロイ後リスト順の詰め

        ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMWINDOW);
        _gotItemList[_listNum].ActionItem();
        _itemPopwindowRectTransform.localPosition = _offPositionPopwin;
    }
    public void OffPopwindow()
    {
        ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMWINDOW);
        _itemPopwindowRectTransform.localPosition = _offPositionPopwin;
    }
    public void OnPopwindow(int listNum)
    {
        ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.ITEMPOP);
        _listNum = listNum;
        _itemPopwindowRectTransform.localPosition = _onPositionPopwin;
    }    
    public void AddGotItemPrefab(ItemStatusData Data)
    {
        //prefabにセットするアイコンやら情報でSPATK用のアイテムなら呼ばないようにする
        //ウィンドウ表示中は窓ボタン以外使えないようにする必要あり
        //GameObject prefab = new GameObject();
        Vector3 pos = new Vector3(0, 150f -50.0f * _gotItemList.Count, 0);
        ItemPrefabScript prefab = (ItemPrefabScript)Instantiate(_itemPrefabObj, _thisWindowPanel.transform.position, Quaternion.identity, _thisWindowPanel.transform);
        prefab.gameObject.transform.localPosition += pos;
        Data.ListNum = _gotItemList.Count;
        prefab.GetThisState(Data);
        _gotItemList.Add(prefab);
        ButtonActionManagerScript.Instance.AddItemButtonList(prefab.itemButton);
       //_saveItemList.Add(prefab.GetComponent<ItemPrefabScript>().itemSaveData);
    }
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
