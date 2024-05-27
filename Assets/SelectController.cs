using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    /// <summary>プレイヤー情報</summary>
    [SerializeField]
    private Image _pOneSelect;
    [SerializeField]
    private Image _pTwoSelect;

    /// <summary>開始ボタン</summary>
    [SerializeField]
    private Button _startButton;
    /// <summary>セーブデータ</summary>
    [SerializeField]
    private SaveCharaSelect _saveCharaSelect;

    void Start()
    {
        _pOneSelect.enabled = true;
        _pTwoSelect.enabled = false;
        _saveCharaSelect = SaveCharaSelect.Instance;
        _saveCharaSelect.CharaNumber = PlayerSelector.PlayerKind.Player_1;
        DontDestroyOnLoad(_saveCharaSelect);
    }

    /// <summary>
    /// スタートボタン
    /// </summary>
    public void PushStartbutton()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// プレイヤー選択１
    /// </summary>
    public void onClickPlayer1()
    {
        _pOneSelect.enabled = true;
        _pTwoSelect.enabled = false;
        _saveCharaSelect.CharaNumber = PlayerSelector.PlayerKind.Player_1;
    }

    /// <summary>
    /// プレイヤー選択２
    /// </summary>
    public void onClickPlayer2()
    {
        _pOneSelect.enabled = false;
        _pTwoSelect.enabled = true;
        _saveCharaSelect.CharaNumber = PlayerSelector.PlayerKind.Player_2;
    }
}
