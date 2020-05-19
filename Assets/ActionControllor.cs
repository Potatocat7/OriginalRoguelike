using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionControllor : MonoBehaviour {
    int count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameControllor.AcitonFlg == true)
        {
            //敵とプレイヤーで同じスクリプトを使うので修正が必要

            //10Fかけて次のマスに移動
            this.transform.Translate(GameControllor.iNext * 0.1f, GameControllor.jNext * 0.1f, 0);
            count += 1;

            if(count == 10)
            {
                GameControllor.AcitonFlg = false;
                count = 0;
            }
        }
		
	}
}
