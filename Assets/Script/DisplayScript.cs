using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayScript : MonoBehaviour {

    /// <summary>プレイヤー情報</summary>
    [SerializeField]
    private StatusDataScript Player;
    /// <summary>フロア階層の数値</summary>
    [SerializeField]
    private int FloorDisplay;
    /// <summary>ステータス表示テキスト</summary>
    [SerializeField]
    private Text statusText;

    ///TODO:ディスプレイの処理を別の場所で行う（今はMapGenerator)
    /// <summary>
    /// 表示設定
    /// </summary>
    /// <param name="playState"></param>
    public void SetDisplayScript(StatusDataScript playState)
    {
        Player = playState;
        FloorDisplay = SaveDataScript.Instance.GetSaveData().clearFloor;
    }

    ///TODO：これもアップデートではなく攻撃処理やアイテム取得。フロアカウントで呼べばよい
    void Update () {
        statusText.text = "HP:"+ Player.GetStateData().HP.ToString() + "/" + Player.GetStateData().MHP.ToString() + "\n"
                                       + "特殊：" + Player.GetSPcount().ToString() + "\n"
                                       + "階層："+ FloorDisplay.ToString(); 
	}
}
