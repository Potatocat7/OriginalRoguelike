using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionControllor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameControllor.AcitonFlg == true)
        {
            this.transform.Translate(GameControllor.iNext, GameControllor.jNext , 0);
            GameControllor.AcitonFlg = false;
        }
		
	}
}
