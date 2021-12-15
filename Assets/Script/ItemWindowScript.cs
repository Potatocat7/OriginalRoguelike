using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWindowScript : MonoBehaviour
{
    [SerializeField] private GameObject _thisWindowPanel;
    [SerializeField] private GameObject _itemPrefabObj;
    [SerializeField] private RectTransform _itemPrefabRectTransform;
    [SerializeField] private RectTransform _thisPanelRectTransform;

    private List<GameObject> _gotItemList = new List<GameObject>();
    private Vector3 _offPosition = new Vector3(0, 500, 0);
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
    }   
    // Start is called before the first frame update
    void Start()
    {
        if (GameControllor.Instance.ItemWindowflg==false)
        {
            //_thisWindowPanel.SetActive(false);
            _thisPanelRectTransform.localPosition = _offPosition;
        }
    }
    public void AddGotItemPrefab()
    {
        //prefabにセットするアイコンやら情報でSPATK用のアイテムなら呼ばないようにする
        //ウィンドウ表示中は窓ボタン以外使えないようにする必要あり
        GameObject prefab = new GameObject();
        Vector3 pos = new Vector3(0, 150f -50.0f * _gotItemList.Count, 0);
        prefab = (GameObject)Instantiate(_itemPrefabObj, _thisWindowPanel.transform.position, Quaternion.identity, _thisWindowPanel.transform);
        prefab.transform.localPosition += pos;
        _gotItemList.Add(prefab);
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
