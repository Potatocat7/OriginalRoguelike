﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllor : MonoBehaviour {

    //staticじゃなくgetcomponent等でもっていけるように修正が必要
    public  bool AcitonFlg;
    public int iNext, jNext;
    public int iRandom, jRandom;
    int timeCount;


    void CheckBlockState()
    {
        //次に移動予定のマスが壁でないかのチェック
        //敵の移動　ランダムに動かす移動可能のマスになるまでwhile文で繰り返すまで
        //※敵動作については条件で複数パターンあるため現状は仮設定
        //※敵オブジェクトは0になることもあるため複数対応+無しのときの対応も必要
        bool checkRandom = true;
        GameObject Enemy = GameObject.Find("EnemyPrefab(Clone)");
        while (checkRandom == true)
        {
            iRandom = (int)Random.Range(-1, 2);
            jRandom = (int)Random.Range(-1, 2);
            if (MapGenerator.map[Enemy.GetComponent<ActionControllor>().iThisNow + iRandom, Enemy.GetComponent<ActionControllor>().jThisNow + jRandom] == 1)
            {

            }
            else
            {                
                Enemy.GetComponent<ActionControllor>().SetNextStep(iRandom, jRandom);
                checkRandom = false;
            }

        }

        //次に移動予定のマスが壁でないかのチェック
        if (MapGenerator.map[MapGenerator.iNow + iNext, MapGenerator.jNow + jNext] == 1)
        {

        }
        else
        {
            GameObject Player = GameObject.Find("PlayerPrefab(Clone)");
            Player.GetComponent<ActionControllor>().SetNextStep(iNext, jNext);
            MapGenerator.iNow = MapGenerator.iNow + iNext;
            MapGenerator.jNow = MapGenerator.jNow + jNext;
            AcitonFlg = true;
        }

    }

    // Use this for initialization
    void Start () {
        AcitonFlg = false;
        timeCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (AcitonFlg == true)
        {
            timeCount += 1;

            if (timeCount == 10)
            {
                AcitonFlg = false;
                timeCount = 0;
            }
        }

        if (AcitonFlg != true) //移動中は入力無効にする
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
