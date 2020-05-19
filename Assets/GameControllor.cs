using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllor : MonoBehaviour {

    //staticじゃなくgetcomponent等でもっていけるように修正が必要
    public static bool AcitonFlg;
    public static int iNext, jNext;


    void CheckBlockState()
    {
        //次に移動予定のマスが壁でないかのチェック
        if (MapGenerator.map[MapGenerator.iNow + iNext, MapGenerator.jNow + jNext] == 1)
        {

        }
        else
        {
            MapGenerator.iNow = MapGenerator.iNow + iNext;
            MapGenerator.jNow = MapGenerator.jNow + jNext;
            AcitonFlg = true;
        }
    }

    // Use this for initialization
    void Start () {
        AcitonFlg = false;
    }
	
	// Update is called once per frame
	void Update () {

        
        if(AcitonFlg != true) //移動中は入力無効にする
        {
            //現在は十字キーだが後でUIにボタンを用意してそこから動かすようにする
            // 左に移動
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                iNext = -1;
                jNext = 0;
                CheckBlockState();
            }
            // 右に移動
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                iNext = 1;
                jNext = 0;
                CheckBlockState();
            }
            // 上に移動
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                iNext = 0;
                jNext = 1;
                CheckBlockState();
            }
            // ↓に移動
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                iNext = 0;
                jNext = -1;
                CheckBlockState();
            }
        }
    }
}
