using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayScript : MonoBehaviour {

    [SerializeField]
    private StatusDataScript Player;
    [SerializeField]
    private SaveDataScript Save;
    [SerializeField]
    private int FloorDisplay;
    [SerializeField]
    private Text statusText;

    public void SetFloor(int Floor)
    {
        FloorDisplay = Floor;
    }

    public void SetDisplayScript(StatusDataScript playState)
    {
        Player = playState;
        //Save = GameObject.Find("SaveDataObject");
        //FloorDisplay = Save.GetComponent<SaveDataScript>().GetSaveData().clearFloor;
        Debug.Log("a");
        FloorDisplay = Save.GetSaveData().clearFloor;

    }

    // Update is called once per frame
    void Update () {
        //宣言すればGetComponentが不要になりそう（あとUpdateでやらないことを考えてみる）
        statusText.text = "HP:"+ Player.GetNow().HP.ToString() + "/" + Player.GetNow().MHP.ToString() + "\n"
                                       + "特殊：" + Player.GetSpcount().ToString() + "\n"
                                       + "階層："+ FloorDisplay.ToString(); 
	}
}
