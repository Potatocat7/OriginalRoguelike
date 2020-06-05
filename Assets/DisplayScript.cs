using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayScript : MonoBehaviour {

    [SerializeField]
    GameObject Player;
    GameObject Save;
    [SerializeField]
    int FloorDisplay;
    public void SetFloor(int Floor)
    {
        FloorDisplay = Floor;
    }

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("PlayerPrefab(Clone)");
        Save = GameObject.Find("SaveDataObject");
        
        FloorDisplay = Save.GetComponent<SaveDataScript>().FloorCount;
    }

    // Update is called once per frame
    void Update () {

        this.GetComponent<Text>().text = "HP:"+ Player.GetComponent<StatusDataScript>().GetNowHP().ToString() + "/" + Player.GetComponent<StatusDataScript>().GetMaxHP().ToString() + "\n" + "特殊：" + Player.GetComponent<StatusDataScript>().GetSpcount().ToString() + "\n"+"階層："+ FloorDisplay.ToString(); 
	}
}
