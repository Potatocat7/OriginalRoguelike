using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefabScript : MonoBehaviour
{
    [SerializeField] private Sprite _imageEquip;
    [SerializeField] private Sprite _imageConsum;

    //[SerializeField] private GameObject _thisImageObj;
    [SerializeField] private Image _thisImage;
    //[SerializeField] private GameObject _thisNameObj;
    [SerializeField] private Text _thisName;
    [SerializeField] private int _thisData_HP;
    [SerializeField] private int _thisData_Attack;
    //[SerializeField] private GameObject _thisEquipCheckObj;
    [SerializeField] private TextMeshProUGUI _thisEquipCheck;
    private int _listNumber;
    public ItemStatusData itemSaveData;
    private ItemScript.ItemType _thisType;
    [SerializeField] public Button itemButton;

    public void GetListNum(int listnum)
    {
        _listNumber = listnum;
    }
    public void GetThisState(ItemStatusData data)//GetComponentどうにかしたい
    {
        //_thisImage = _thisImageObj.GetComponent<Image>();
        //_thisName = _thisNameObj.GetComponent<Text>();
        //_thisEquipCheck = _thisEquipCheckObj.GetComponent<Text>();
        switch (data.Type)
        {
            case ItemScript.ItemType.EQUIP:
                _thisImage.sprite = _imageEquip;
                break;
            case ItemScript.ItemType.CONSUM:
                _thisImage.sprite = _imageConsum;
                break;
            default:
                Debug.Log("こないはず");
                break;
        }
        _thisType = data.Type;
        _thisName.text = data.Name;
        _thisData_HP = data.Hp;
        _thisData_Attack = data.Attack;
        if (data.EquipFlg == true)
        {
            //_thisEquipCheckObj.SetActive(true);
            _thisEquipCheck.gameObject.SetActive(true);
            itemSaveData.EquipFlg = true; 
        }
        else
        {
            //_thisEquipCheckObj.SetActive(false);
            _thisEquipCheck.gameObject.SetActive(false);
            itemSaveData.EquipFlg = false;
        }
        _listNumber = data.ListNum;
        itemSaveData = data;
    }
    public void OffEquipItem()
    {
        switch (_thisType)
        {
            case ItemScript.ItemType.EQUIP:
                if(itemSaveData.EquipFlg == true)
                {
                    //_thisEquipCheckObj.SetActive(false);
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
    public void ActionItem()
    {
        switch (_thisType)
        {
            case ItemScript.ItemType.EQUIP:
                GameManager.Instance.GetItemWindow().CheckEquipItem();
                //_thisEquipCheckObj.SetActive(true);
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
    public void OnClick()
    {
        GameManager.Instance.GetItemWindow().OnPopwindow(_listNumber);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
