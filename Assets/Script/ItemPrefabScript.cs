using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefabScript : MonoBehaviour
{
    /// <summary>このアイテムの画像</summary>
    [SerializeField] private Image _thisImage;
    /// <summary>このアイテムの名前</summary>
    [SerializeField] private Text _thisName;
    /// <summary>装備済みアイコン</summary>
    [SerializeField] private TextMeshProUGUI _thisEquipCheck;
    /// <summary>アイテムリストの番号</summary>
    private int _listNumber;
    /// <summary>アイテムの状態</summary>
    public ItemStatusData itemSaveData;
    /// <summary>アイテムの形式</summary>
    private ItemScript.ItemType _thisType;
    /// <summary>使用・装備用ボタン</summary>
    [SerializeField] public Button itemButton;

    /// <summary>
    /// リストの番号を取得
    /// </summary>
    /// <param name="listnum"></param>
    public void GetListNum(int listnum)
    {
        _listNumber = listnum;
    }

    /// <summary>
    /// アイテムのステータス設定
    /// </summary>
    /// <param name="data"></param>
    public void GetThisState(ItemStatusData data)
    {
        _thisImage.sprite = data.sprite;
        _thisType = data.Type;
        _thisName.text = data.Name;
        if (data.EquipFlg == true)
        {
            _thisEquipCheck.gameObject.SetActive(true);
            itemSaveData.EquipFlg = true; 
        }
        else
        {
            _thisEquipCheck.gameObject.SetActive(false);
            itemSaveData.EquipFlg = false;
        }
        _listNumber = data.ListNum;
        itemSaveData = data;
    }

    /// <summary>
    /// 装備解除処理
    /// </summary>
    public void OffEquipItem()
    {
        switch (_thisType)
        {
            case ItemScript.ItemType.EQUIP:
                if(itemSaveData.EquipFlg == true)
                {
                    _thisEquipCheck.gameObject.SetActive(false);
                    itemSaveData.EquipFlg = false;
                    //ステータス変更
                    GameManager.Instance.GetPlayerManager().SubItemState(itemSaveData);
                }
                break;
            case ItemScript.ItemType.CONSUM:
                break;
            default:
                Debug.Log("こないはず");
                break;
        }
    }

    /// <summary>
    /// アイテム使用処理
    /// </summary>
    public void ActionItem()
    {
        switch (_thisType)
        {
            case ItemScript.ItemType.EQUIP:
                GameManager.Instance.GetItemWindow().CheckEquipItem();
                _thisEquipCheck.gameObject.SetActive(true);
                itemSaveData.EquipFlg = true;
                //ステータス反映
                GameManager.Instance.GetPlayerManager().AddItemState(itemSaveData);
                break;
            case ItemScript.ItemType.CONSUM:
                GameManager.Instance.GetItemWindow().OrganizeList(_listNumber);
                GameManager.Instance.GetPlayerManager().AddItemState(itemSaveData);
                Destroy(gameObject);
                break;
            default:
                Debug.Log("こないはず");
                break;
        }

    }

    /// <summary>
    /// アイテム選択時処理
    /// </summary>
    public void OnClick()
    {
        GameManager.Instance.GetItemWindow().OnPopwindow(_listNumber);
    }
}
