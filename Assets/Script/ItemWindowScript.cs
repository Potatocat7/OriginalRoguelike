using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWindowScript : MonoBehaviour
{
    [SerializeField] private GameObject _thisWindowPanel;
    [SerializeField] private RectTransform _thisPanelRectTransform;
    [SerializeField] private GameControllor _gameCtrl;//シングルトンにしてよべんじゃね？

    private Vector3 _offPosition = new Vector3(0, 500, 0);
    // Start is called before the first frame update
    void Start()
    {
        if (_gameCtrl.ItemWndowflg==false)
        {
            //_thisWindowPanel.SetActive(false);
            _thisPanelRectTransform.localPosition = _offPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameCtrl.ItemWndowflg == false)
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
