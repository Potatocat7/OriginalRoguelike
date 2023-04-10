using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    //[SerializeField] private GameControllor _gameCtrl;
    [SerializeField] private GameObject _wallObj;
    [SerializeField] private GameObject _goalObj;
    [SerializeField] private GameObject _enemyObj;
    [SerializeField] private PlayerSelector _playerSelectObj;
    [SerializeField] private GameObject _floorObj;
    [SerializeField] private GameObject _healItemObj;
    [SerializeField] private GameObject _weaponItemObj;
    [SerializeField] private GameObject _consumptionItemObj;
    [SerializeField] private GameObject _powerItemObj;
    [SerializeField] private GameObject[,] _mapobj = new GameObject[20, 20];
    private GameObject _playerObj;
    private SaveCharaSelect CharaNum;
    public static int[,] map = new int[20, 20];       //選択後の
    public static int iNow, jNow, EnemyCount,UniqObjCount;
    public int mapNum;
    [SerializeField]
    private SaveDataScript _saveData;
    [SerializeField]
    private DisplayScript _displayScript;
    private StatusDataScript _playerData;
    public List<ActionControllor> EnemyList = new List<ActionControllor>();
    public List<StatusDataScript> EnemyListStateData = new List<StatusDataScript>();
    public List<int> iObjState = new List<int>();
    public List<int> jObjState = new List<int>();
    private bool _saveDataFlg;

    //シングルトン化
    private static MapGenerator mInstance;
    public static MapGenerator Instance
    {
        get
        {
            return mInstance;
        }
    }
    // Use this for initialization
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        mInstance = this;
        //このFindもどうにかしたい
        CharaNum = GameObject.Find("SaveCharaSelect").GetComponent<SaveCharaSelect>();
    }

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
    bool CheckMapstateUobj(int tate, int yoko)
    {
        bool flg = true;
        for (int count = 0;　count < UniqObjCount; count++)
        {
            if (tate == iObjState[count] && yoko == jObjState[count]) //作成済みオブジェクトと座標がかぶってないか
            {
                flg = false; 
            }

        }
        return (flg);
    }
    public void SetDropItemObj(int iPix,int jPix)//type itemType)
    {
        //MAP上に出口・プレイヤー等のオブジェクトを追加でセットしていく ※かぶさらないようにする必要あり
        var PrefabObj = _consumptionItemObj;

        if (map[iPix, jPix] != 1)    //MAPが通路のなとき(壁でないとき)
        {
            // プレハブを元に、インスタンスを生成、
            _mapobj[iPix, jPix] = (GameObject)Instantiate(PrefabObj, new Vector3(iPix, jPix, -1.0F), Quaternion.identity);
            if (PrefabObj.tag == "Item")
            {
                //GetComponent。InstantiateがGameObject出しか作れないなんてことがなかったはずなのでなおせるならなおした
                _mapobj[iPix, jPix].GetComponent<ItemScript>().GetPosition(iPix, jPix);
                GameControllor.Instance.AddCountItemObj(_mapobj[iPix, jPix]);
            }
        }
        else
        {
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
                    if (CheckMapstateUobj(randomiPix, randomjPix) == true) //条件が達成されていたら
                    {
                        // プレハブを元に、インスタンスを生成、
                        _mapobj[randomiPix, randomjPix] = (GameObject)Instantiate(PrefabObj, new Vector3(randomiPix, randomjPix, -1.0F), Quaternion.identity);
                        iLoopflg = true;
                        if (PrefabObj.tag == "Player")
                        {
                            _mapobj[randomiPix, randomjPix].GetComponent<ActionControllor>().StartSetUp();
                            GameControllor.Instance.SetPlayerActionCtrl(_mapobj[randomiPix, randomjPix].GetComponent<ActionControllor>());
                            iNow = randomiPix;
                            jNow = randomjPix;
                            iObjState.Add(randomiPix);
                            jObjState.Add(randomjPix);
                            _playerData = _mapobj[randomiPix, randomjPix].GetComponent<StatusDataScript>();
                            _displayScript.SetDisplayScript(_playerData);
                            GameControllor.Instance.SetPlayerState(_playerData);
                        }
                        else if (PrefabObj.tag == "Enemy") 
                        {
                            //上がなおせればGetComponentが一気に解消できそう？
                            _mapobj[randomiPix, randomjPix].GetComponent<ActionControllor>().StartSetUp();
                            _mapobj[randomiPix, randomjPix].GetComponent<EnemyAttack>().GetPlayerStatusData(_playerData);
                            _mapobj[randomiPix, randomjPix].GetComponent<EnemyAttack>().GetThisStatusData(_mapobj[randomiPix, randomjPix].GetComponent<StatusDataScript>());
                            _mapobj[randomiPix, randomjPix].GetComponent<StatusDataScript>().GetPlayerState(_playerData);
                            EnemyList.Add(_mapobj[randomiPix, randomjPix].GetComponent<ActionControllor>());
                            EnemyListStateData.Add(_mapobj[randomiPix, randomjPix].GetComponent<StatusDataScript>());
                            iObjState.Add(randomiPix); 
                            jObjState.Add(randomjPix);
                        }
                        else if (PrefabObj.tag == "Item")
                        {
                            _mapobj[randomiPix, randomjPix].GetComponent<ItemScript>().GetPosition(randomiPix, randomjPix);
                            GameControllor.Instance.AddCountItemObj(_mapobj[randomiPix, randomjPix]);
                        }
                        else if (PrefabObj.tag == "Goal")
                        {
                            GameControllor.Instance.SetGoalObj(_mapobj[randomiPix, randomjPix]);
                        }

                    }
                }
            }
            else
            {
            }
        }
    }
    private void SetPlayerObject()
    {
        _playerObj = _playerSelectObj.SelectTypeBullet(CharaNum.CharaNumber);
    }
    public void MapGeneStart()
    {
        mapNum = Random.Range(0, 3);        // 0～3の乱数を取得
        EnemyCount = 0;
        UniqObjCount = 0;
        //for文で配列に情報を入れていく(MapDataScript.mapDataだと引数が増えるため)
        for (int iPix = 0; iPix < MapDataScript.mapData.GetLength(1); iPix++) //mapWidth
        {
            for (int jPix = 0; jPix < MapDataScript.mapData.GetLength(2); jPix++) //mapHeight
            {
                map[iPix, jPix] = MapDataScript.mapData[2, iPix, jPix];// mapNum
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
                    _mapobj[iPix, jPix] = (GameObject)Instantiate(_wallObj, new Vector3(iPix , jPix , 0.0F), Quaternion.identity);
                }
                else                            //床  
                {
                    // プレハブを元に、インスタンスを生成、
                    _mapobj[iPix, jPix] = (GameObject)Instantiate(_floorObj, new Vector3(iPix, jPix , 0.0F), Quaternion.identity);

                }
            }
        }
        //※すでに追加オブジェクトがある場所には生成しないようにする処理が必要
        SetUniqObj(_goalObj);
        SetPlayerObject();
        //_playerObj.GetComponent<ActionControllor>().SetGameCtrl(_gameCtrl);
        SetUniqObj(_playerObj);
        SetUniqObj(_powerItemObj);
        SetUniqObj(_weaponItemObj);
        SetUniqObj(_consumptionItemObj);
        UniqObjCount = 1;
        for (int Ecount = 0; Ecount < 5; Ecount++)
        {
            SetUniqObj(_enemyObj);
            EnemyCount += 1;
            UniqObjCount +=1;

        }
        //アイテム等はここで同じ用に生成

        //コントローラの初期化関数呼び出し

        GameControllor.Instance.AftorMakeMapStart();
    }

    // Update is called once per frame
    void Update () {
    }
}
