using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayScript : MonoBehaviour {

    [SerializeField]
    StatusDataScript Player;
    GameObject Save;
    [SerializeField]
    int FloorDisplay;
    public void SetFloor(int Floor)
    {
        FloorDisplay = Floor;
    }

    public void SetDisplayScript(StatusDataScript playState)
    {
        Player = playState;
        Save = GameObject.Find("SaveDataObject");
        FloorDisplay = Save.GetComponent<SaveDataScript>().FloorCount;

    }

    // Update is called once per frame
    void Update () {

        this.GetComponent<Text>().text = "HP:"+ Player.GetNowHP().ToString() + "/" + Player.GetMaxHP().ToString() + "\n"
                                       + "特殊：" + Player.GetSpcount().ToString() + "\n"
                                       + "階層："+ FloorDisplay.ToString(); 
	}
}
