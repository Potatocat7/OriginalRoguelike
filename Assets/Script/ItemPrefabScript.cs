﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefabScript : MonoBehaviour
{
    [SerializeField] private Sprite _imageEquip;
    [SerializeField] private Sprite _imageConsum;

    [SerializeField] private GameObject _thisImageObj;
    [SerializeField] private Image _thisImage;
    [SerializeField] private GameObject _thisNameObj;
    [SerializeField] private Text _thisName;
    [SerializeField] private int _thisData_HP;
    [SerializeField] private int _thisData_Attack;
    [SerializeField] private GameObject _thisEquipCheckObj;
    [SerializeField] private Text _thisEquipCheck;
    private int _listNumber;
    private ItemScript.ItemType _thisType;
    [SerializeField] private Button _ItemButton;

    public void GetListNum(int listnum)
    {
        _listNumber = listnum;
    }
    public void GetThisState(ItemStatusData data)
    {
        _thisImage = _thisImageObj.GetComponent<Image>();
        _thisName = _thisNameObj.GetComponent<Text>();
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
        _thisEquipCheckObj.SetActive(false);
        _listNumber = data.ListNum;

    }
    public void ActionItem()
    {
        switch (_thisType)
        {
            case ItemScript.ItemType.EQUIP:
                _thisEquipCheckObj.SetActive(true);
                break;
            case ItemScript.ItemType.CONSUM:
                ItemWindowScript.Instance.OrganizeList(_listNumber);
                Destroy(gameObject);
                break;
            default:
                Debug.Log("こないはず");
                break;
        }

    }
    public void OnClick()
    {
        ItemWindowScript.Instance.OnPopwindow(_listNumber);
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
