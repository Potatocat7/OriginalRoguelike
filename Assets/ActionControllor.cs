using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionControllor : MonoBehaviour {

    //
    bool UserActFlg;
    int count;
    [SerializeField]
    int iThisNext, jThisNext, iThisNow, jThisNow;


    //次の移動するマスの
    public bool SetNextStep(int iNext, int jNext)
    {
        if (MapGenerator.map[iThisNow + iNext, jThisNow + jNext] == 1)
        {
            return false;
        }
        else
        {
            iThisNext = iNext;
            jThisNext = jNext;
            return true;
        }
    }
    // Use this for initialization
    void Start () {
        //生成時にthisでオブジェクトの情報を所得してmapの現座標を獲得しておく
        iThisNow = (int)this.transform.position.x;
        jThisNow = (int)this.transform.position.y;
        count = 0;
        UserActFlg = false;
    }
    public void SetUserActFlagOn()
    {
        count = 0;
        UserActFlg = true;
    }
//    public void SetUserActFlagOff()
//    {
//        count = 0;
//        UserActFlg = false;
//    }
//
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
