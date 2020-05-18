using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject WallObj;
    public GameObject FloorObj;
    public GameObject[,] Mapobj = new GameObject[20, 20];
    public int mapNum;
   // public int[,] map = new int[20, 20];       //とりあえず固定値

    // Use this for initialization
    void Start () {
        mapNum = Random.Range(0, 3);        // 0～3の乱数を取得
        /*//for文で配列に情報を入れていく
        for (int iPix = 0; iPix < 20; iPix++) //mapWidth
        {
            for (int jPix = 0; jPix < 20; jPix++) //mapHeight
            {
                map[iPix, jPix] = MapDataScript.mapData[mapNum, iPix, jPix];
            }
        }*/

        //壁・通路だけ選考生成
        for (int iPix = 0; iPix < MapDataScript.mapData.GetLength(1); iPix++)
        {
            for (int jPix = 0; jPix < MapDataScript.mapData.GetLength(2); jPix++)
            {
                if (MapDataScript.mapData[mapNum, iPix, jPix] == 1)        //壁
                {
                    // プレハブをGameObject型で取得
                    //GameObject obj = (GameObject)Resources.Load("Image_WALL");
                    //WallObj = (GameObject)Resources.Load("WallPrefab");
                    // プレハブを元に、インスタンスを生成、
                    
                    Mapobj[iPix, jPix] = (GameObject)Instantiate(WallObj, new Vector3(iPix , jPix , 0.0F), Quaternion.identity);
                    //Mapobj[iPix, jPix] = (GameObject)Instantiate(obj, transform.localPosition, Quaternion.identity, parent);
                    //Mapobj[iPix, jPix].transform.Position = new Vector3(iPix * 4.0F , jPix * 4.0F , 0.0F);
                }
                else                            //床  
                {
                    // プレハブをGameObject型で取得
                    //GameObject obj = (GameObject)Resources.Load("Image_PASS");
                    //FloorObj = (GameObject)Resources.Load("FloorPrefab");
                    // プレハブを元に、インスタンスを生成、
                    Mapobj[iPix, jPix] = (GameObject)Instantiate(FloorObj, new Vector3(iPix, jPix , 0.0F), Quaternion.identity);
                    //Mapobj[iPix, jPix] = (GameObject)Instantiate(obj, transform.localPosition, Quaternion.identity, parent);
                    //Mapobj[iPix, jPix].transform.localPosition = new Vector3(iPix * 4 * 1.0f + 2.0f, jPix * 4 * 1.0f + 2.0f, 0.0f);

                }
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
