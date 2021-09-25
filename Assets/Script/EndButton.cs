using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PushEndbutton()
    {
        GameObject Save;
        Save = GameObject.Find("SaveDataObject");
        Save.GetComponent<SaveDataScript>().ClearData();

        SceneManager.LoadScene("StartScene");
    }
}
