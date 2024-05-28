using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActionManagerScript : MonoBehaviour
{
    /// <summary>
    /// UIの各ボタン
    /// </summary>
    [SerializeField] private Button _stop;
    [SerializeField] private Button _up;
    [SerializeField] private Button _upL;
    [SerializeField] private Button _upR;
    [SerializeField] private Button _attack;
    [SerializeField] private Button _left;
    [SerializeField] private Button _right;
    [SerializeField] private Button _down;
    [SerializeField] private Button _downL;
    [SerializeField] private Button _downR;
    /// <summary>アイテムアイコンリスト</summary>
    private List<Button> _itemIconList= new List<Button>();
    /// <summary>
    /// ボタンを押したときの状態
    /// </summary>
    public enum ButtonStateType
    {
        NONE = 0,
        GAME,
        ITEMWINDOW,
        ITEMPOP
    }  

    //シングルトン化
    private static ButtonActionManagerScript mInstance;
    public static ButtonActionManagerScript Instance
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
    }

    /// <summary>
    /// アイテムリストのボタン追加
    /// </summary>
    /// <param name="button"></param>
    public void AddItemButtonList(Button button)
    {
        _itemIconList.Add(button);
    }

    /// <summary>
    /// アイテムリストのボタン削除
    /// </summary>
    /// <param name="num"></param>
    public void RemoveItemButtonList(int num)
    {
        _itemIconList.RemoveAt(num);
    }

    /// <summary>
    /// ボタンを押したときに状態を変える
    /// </summary>
    /// <param name="state"></param>
    public void ChangeButtonState(ButtonStateType state)
    {
        switch (state)
        {
            case ButtonStateType.GAME:
                _stop.interactable = true;
                _up.interactable = true;
                _upL.interactable = true;
                _upR.interactable = true;
                _attack.interactable = true;
                _left.interactable = true;
                _right.interactable = true;
                _down.interactable = true;
                _downL.interactable = true;
                _downR.interactable = true;
                for (int i = 0; i < _itemIconList.Count; i++)
                {
                    _itemIconList[i].interactable = false;
                }
                break;
            case ButtonStateType.ITEMWINDOW:
                _stop.interactable = false;
                _up.interactable = false;
                _upL.interactable = false;
                _upR.interactable = false;
                _attack.interactable = false;
                _left.interactable = false;
                _right.interactable = false;
                _down.interactable = false;
                _downL.interactable = false;
                _downR.interactable = false;
                for (int i = 0; i < _itemIconList.Count; i++)
                {
                    _itemIconList[i].interactable = true;
                }
                break;
            case ButtonStateType.ITEMPOP:
                _stop.interactable = false;
                _up.interactable = false;
                _upL.interactable = false;
                _upR.interactable = false;
                _attack.interactable = false;
                _left.interactable = false;
                _right.interactable = false;
                _down.interactable = false;
                _downL.interactable = false;
                _downR.interactable = false;
                for (int i = 0; i < _itemIconList.Count; i++)
                {
                    _itemIconList[i].interactable = false;
                }
                break;
            default:
                Debug.Log("こないはず");
                break;
        }
    }
}
