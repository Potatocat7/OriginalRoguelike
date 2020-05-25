using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject WallObj;
    public GameObject GoalObj;
    public GameObject EnemyObj;
    public List<GameObject> EnemyList = new List<GameObject>();
    public GameObject PlayerObj;
    public GameObject FloorObj;
    public GameObject HealItemObj;
    public GameObject PowerItemObj;
    public GameObject[,] Mapobj = new GameObject[20, 20];
    public static int[,] map = new int[20, 20];       //選択後の
    public static int iNow, jNow, EnemyCount;
    public int mapNum;

    bool CheckMapstate(int tate, int yoko)
    {
        //周囲のマスを調べて床のマスがいくつあるか調べる
        int count = 0;
        for (int iPix = 0; iPix < 3; iPix++) //mapWidth
        {
            for (int jPix = 0; jPix < 3; jPix++) //mapHeight
            {
                if (map[ iPix + tate - 1, jPix + yoko - 1] == 1)//周囲の壁の数を確認
                {
                    count += 1;

                }
            }
        }
        if (count <= 5) //カウントが3以上の時（大体３つ以上なら部屋である可能性）
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }
    void SetUniqObj(  GameObject PrefabObj)
    {
        //MAP上に出口・プレイヤー等のオブジェクトを追加でセットしていく ※かぶさらないようにする必要あり
        bool iLoopflg = false;
        while (iLoopflg == false)　//出口指定
        {
            int randomiPix = Random.Range(1, 19);        // 1～19の乱数を取得
            int randomjPix = Random.Range(1, 19);        // 1～19の乱数を取得

            if (map[randomiPix, randomjPix] != 1)    //MAPが通路のなとき(壁でないとき)
            {
                if (CheckMapstate(randomiPix, randomjPix) == true) //条件が達成されていたら
                {
                    // プレハブを元に、インスタンスを生成、
                    Mapobj[randomiPix, randomjPix] = (GameObject)Instantiate(PrefabObj, new Vector3(randomiPix, randomjPix, -1.0F), Quaternion.identity);

                    iLoopflg = true;
                    if (PrefabObj.tag == "Player") {
                        PrefabObj.GetComponent<ActionControllor>().StartSetUp();
                        iNow = randomiPix;
                        jNow = randomjPix;
                    }
                    else if(PrefabObj.tag == "Enemy")
                    {
                        PrefabObj.GetComponent<ActionControllor>().StartSetUp();

                    }
                }
            }
            else
            {
            }
        }
    }
    // Use this for initialization
    void Start()
    {
    }
    void Awake()
    {
        mapNum = Random.Range(0, 3);        // 0～3の乱数を取得
        EnemyCount = 0;
        //for文で配列に情報を入れていく(MapDataScript.mapDataだと引数が増えるため)
        for (int iPix = 0; iPix < MapDataScript.mapData.GetLength(1); iPix++) //mapWidth
        {
            for (int jPix = 0; jPix < MapDataScript.mapData.GetLength(2); jPix++) //mapHeight
            {
                map[iPix, jPix] = MapDataScript.mapData[mapNum, iPix, jPix];
            }
        }

        //壁・通路だけ先行生成
        for (int iPix = 0; iPix < MapDataScript.mapData.GetLength(1); iPix++)
        {
            for (int jPix = 0; jPix < MapDataScript.mapData.GetLength(2); jPix++)
            {
                if (map[ iPix, jPix] == 1)        //壁
                {
                    // プレハブを元に、インスタンスを生成、
                    Mapobj[iPix, jPix] = (GameObject)Instantiate(WallObj, new Vector3(iPix , jPix , 0.0F), Quaternion.identity);
                }
                else                            //床  
                {
                    // プレハブを元に、インスタンスを生成、
                    Mapobj[iPix, jPix] = (GameObject)Instantiate(FloorObj, new Vector3(iPix, jPix , 0.0F), Quaternion.identity);

                }
            }
        }
        //※すでに追加オブジェクトがある場所には生成しないようにする処理が必要
        SetUniqObj(GoalObj);
        SetUniqObj(PlayerObj);
        //for (int Ecount=0 ; Ecount < 3 ; Ecount++)
        //{
            SetUniqObj(EnemyObj);
            //EnemyList.Add(EnemyObj);
            EnemyCount += 1;

        //}
        //アイテム等はここで同じ用に生成

        //コントローラの初期化関数呼び出し
        //GetComponent<GameControllor>().AftorMakeMapStart();
    }

    // Update is called once per frame
    void Update () {
    }
}
