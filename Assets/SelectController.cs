using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    //やること一覧
    //最初にP1を選択状態にする
    //選択状態を次シーンに持ち越し
    //ボタンを押したらシーン切り替え
    //タッチされたら切り替え、選択側の親オブジェクトのImageをfalse
    [SerializeField]
    private Image _pOneSelect;
    [SerializeField]
    private Image _pTwoSelect;

    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private SaveCharaSelect _saveCharaSelect;

    // Start is called before the first frame update
    void Start()
    {
        _pOneSelect.enabled = true;
        _pTwoSelect.enabled = false;
        _saveCharaSelect = SaveCharaSelect.Instance;
        _saveCharaSelect.CharaNumber = PlayerSelector.PlayerKind.Player_1;
        DontDestroyOnLoad(_saveCharaSelect);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PushStartbutton()
    {
        //GameObject Save;
        //Save = GameObject.Find("SaveDataObject");
        //Save.GetComponent<SaveDataScript>().ClearData();
        SceneManager.LoadScene("GameScene");
    }
    public void onClickPlayer1()
    {

        _pOneSelect.enabled = true;
        _pTwoSelect.enabled = false;
        _saveCharaSelect.CharaNumber = PlayerSelector.PlayerKind.Player_1;

    }
    public void onClickPlayer2()
    {

        _pOneSelect.enabled = false;
        _pTwoSelect.enabled = true;
        _saveCharaSelect.CharaNumber = PlayerSelector.PlayerKind.Player_2;

    }
}
