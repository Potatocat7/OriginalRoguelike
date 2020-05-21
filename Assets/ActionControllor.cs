using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionControllor : MonoBehaviour {

    //
    public int iThisNext, jThisNext;
    public int iThisNow, jThisNow;
    public bool UserActFlg;
    int count;
    GameObject GameCtlObj;
    // Use this for initialization
    public void SetNextStep (int iNext, int jNext)
    {
        iThisNext = iNext;
        jThisNext = jNext;
    }
    void Start () {
        //生成時にthisでオブジェクトの情報を所得してmapの現座標を獲得しておく
        iThisNow = (int)this.transform.position.x;
        jThisNow = (int)this.transform.position.y;
        GameCtlObj = GameObject.Find("GameControllor");
        count = 0;
        UserActFlg = false;
    }
    public void SetUserActFlagOn()
    {
        count = 0;
        UserActFlg = true;
    }
    public void SetUserActFlagOff()
    {
        count = 0;
        UserActFlg = false;
    }

    // Update is called once per frame
    void Update () {
        //アクション動作で攻撃と移動をここで処理
        //if (GameCtlObj.GetComponent<GameControllor>().AcitonFlg == true)
        if(UserActFlg == true)
        {
                //敵とプレイヤーで同じスクリプトを使うので修正が必要

                //10Fかけて次のマスに移動
            this.transform.Translate(iThisNext * 0.1f, jThisNext * 0.1f, 0);
            count += 1;

            if(count == 10)
            {
                UserActFlg = false;
                count = 0;
                iThisNow = iThisNow + iThisNext;
                jThisNow = jThisNow + jThisNext;
            }
        }
		
	}
}
