using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWindowScript : MonoBehaviour
{
    [SerializeField] private GameObject _thisWindowPanel;
    [SerializeField] private GameObject _itemPrefabObj;
    [SerializeField] private RectTransform _itemPopwindowRectTransform;
    [SerializeField] private RectTransform _thisPanelRectTransform;

    public List<ItemPrefabScript> _gotItemList;// = new List<ItemPrefabScript>();
    public List<ItemStatusData> _saveItemList;// = new List<ItemPrefabScript>();
    private Vector3 _offPosition = new Vector3(0, 500, 0);
    private Vector3 _offPositionPopwin = new Vector3(700, 700, 0); 
    private Vector3 _onPositionPopwin = new Vector3(250, -125, 0);
    private int _listNum;
    //シングルトン化
    private static ItemWindowScript mInstance;
    public static ItemWindowScript Instance
    {
        get
        {
            return mInstance;
        }
    }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        mInstance = this;
        //DontDestroyOnLoad(gameObject);
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
        SaveDataScript.Instance._saveItemList= new List<ItemStatusData>();

    }
    public void SetupItemState()
    {
        for (int i = 0; i < _gotItemList.Count; i++)
        {
            //装備していたアイテムは装備させる
            if (_gotItemList[i].itemSaveData.EquipFlg == true)
            {
                GameControllor.Instance.AddItemState(_gotItemList[i].itemSaveData);
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
        _gotItemList[_listNum].GetComponent<ItemPrefabScript>().ActionItem();
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
    // Start is called before the first frame update
    void Start()
    {
        if (GameControllor.Instance.ItemWindowflg==false)
        {
            //_thisWindowPanel.SetActive(false);
            ButtonActionManagerScript.Instance.ChangeButtonState(ButtonActionManagerScript.ButtonStateType.GAME);
            _thisPanelRectTransform.localPosition = _offPosition;
        }
    }
    public void AddGotItemPrefab(ItemStatusData Data)
    {
        //prefabにセットするアイコンやら情報でSPATK用のアイテムなら呼ばないようにする
        //ウィンドウ表示中は窓ボタン以外使えないようにする必要あり
        GameObject prefab = new GameObject();
        Vector3 pos = new Vector3(0, 150f -50.0f * _gotItemList.Count, 0);
        prefab = (GameObject)Instantiate(_itemPrefabObj, _thisWindowPanel.transform.position, Quaternion.identity, _thisWindowPanel.transform);
        prefab.transform.localPosition += pos;
        Data.ListNum = _gotItemList.Count;
        prefab.GetComponent<ItemPrefabScript>().GetThisState(Data);
        _gotItemList.Add(prefab.GetComponent<ItemPrefabScript>());
        ButtonActionManagerScript.Instance.AddItemButtonList(prefab.GetComponent<ItemPrefabScript>().itemButton);
       //_saveItemList.Add(prefab.GetComponent<ItemPrefabScript>().itemSaveData);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameControllor.Instance.ItemWindowflg == false)
        {
            //_thisWindowPanel.SetActive(false);
            _thisPanelRectTransform.localPosition = _offPosition;
        }
        else
        {
            //_thisWindowPanel.SetActive(true);
            _thisPanelRectTransform.localPosition = Vector3.zero;
        }
    }
}
