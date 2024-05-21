using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    //[SerializeField] private GameControllor _gameCtrl;
    [SerializeField] private MapStatus _mapPrefab;
    [SerializeField] private GameObject _wallObj;
    [SerializeField] private GameObject _goalObj;
    [SerializeField] private ActionControllor _enemyObj;
    [SerializeField] private PlayerSelector _playerSelectObj;
    [SerializeField] private GameObject _floorObj;
    [SerializeField] private GameObject _healItemObj;
    [SerializeField] private ItemScript _weaponItemObj;
    [SerializeField] private ItemScript _consumptionItemObj;
    [SerializeField] private ItemScript _powerItemObj;
    [SerializeField] private MapStatus[,] _mapobj = new MapStatus[20, 20];
    private ActionControllor _playerObj;
    private SaveCharaSelect CharaNum;
    public static int[,] map = new int[20, 20];       //選択後の
    public static int iNow, jNow, EnemyCount,UniqObjCount;
    public int mapNum;
    [SerializeField]
    private SaveDataScript _saveData;
    [SerializeField]
    private DisplayScript _displayScript;
    private StatusDataScript _playerData;
    //public List<ActionControllor> EnemyList = new List<ActionControllor>();
    //public List<StatusDataScript> EnemyListStateData = new List<StatusDataScript>();
    public List<int> iObjState = new List<int>();
    public List<int> jObjState = new List<int>();
    private bool _saveDataFlg;

    private Action<ItemScript> makeItem;
    private Action<GameObject> goal;

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
    /// <summary>
    /// Uniqオブジェクト既にあるかチェック
    /// </summary>
    /// <param name="tate"></param>
    /// <param name="yoko"></param>
    /// <returns></returns>
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
        ItemScript PrefabObj = _consumptionItemObj;

        if (map[iPix, jPix] != 1)    //MAPが通路のなとき(壁でないとき)
        {
            // プレハブを元に、インスタンスを生成
            ItemScript setItem = (ItemScript)Instantiate(PrefabObj, new Vector3(iPix, jPix, -1.0F), Quaternion.identity);
            _mapobj[iPix, jPix].SetItem(setItem);
            if (PrefabObj.tag == "Item")
            {
                //GetComponent。InstantiateがGameObject出しか作れないなんてことがなかったはずなのでなおせるならなおした
                _mapobj[iPix, jPix]._Item.GetPosition(iPix, jPix);
                makeItem.Invoke(_mapobj[iPix, jPix]._Item);
                //GameControllor.Instance.AddCountItemObj(setItem);
            }
        }
        else
        {
        }
    }

    void SetUniqObj(GameObject PrefabObj)
    {
        //MAP上に出口・プレイヤー等のオブジェクトを追加でセットしていく ※かぶさらないようにする必要あり
        bool iLoopflg = false;
        while (iLoopflg == false)　//出口指定
        {
            int randomiPix = UnityEngine.Random.Range(1, 19);        // 1～19の乱数を取得
            int randomjPix = UnityEngine.Random.Range(1, 19);        // 1～19の乱数を取得

            if (map[randomiPix, randomjPix] != 1)    //MAPが通路のなとき(壁でないとき)
            {
                if (CheckMapstate(randomiPix, randomjPix) == true) //条件が達成されていたら
                {
                    if (CheckMapstateUobj(randomiPix, randomjPix) == true) //条件が達成されていたら
                    {
                        // プレハブを元に、インスタンスを生成
                        _mapobj[randomiPix, randomjPix].SetMapObject((GameObject)Instantiate(PrefabObj, new Vector3(randomiPix, randomjPix, -1.0F), Quaternion.identity));
                        iLoopflg = true;
                        if (PrefabObj.tag == "Goal")
                        {
                            goal.Invoke(_mapobj[randomiPix, randomjPix]._mapObject);
                            //GameControllor.Instance.SetGoalObj(_mapobj[randomiPix, randomjPix]._mapObject);
                        }

                    }
                }
            }
            else
            {
            }
        }
    }
    /// <summary>
    /// 通路以外にオブジェクトを設置
    /// </summary>
    /// <param name="PrefabObj"></param>
    void SetUniqObj(ActionControllor PrefabObj,Action<ActionControllor,StatusDataScript> charaData = null)
    {
        //MAP上に出口・プレイヤー等のオブジェクトを追加でセットしていく ※かぶさらないようにする必要あり
        bool iLoopflg = false;
        while (iLoopflg == false)　//出口指定
        {
            int randomiPix = UnityEngine.Random.Range(1, 19);        // 1～19の乱数を取得
            int randomjPix = UnityEngine.Random.Range(1, 19);        // 1～19の乱数を取得

            if (map[randomiPix, randomjPix] != 1)    //MAPが通路のなとき(壁でないとき)
            {
                if (CheckMapstate(randomiPix, randomjPix) == true) //条件が達成されていたら
                {
                    if (CheckMapstateUobj(randomiPix, randomjPix) == true) //条件が達成されていたら
                    {
                        // プレハブを元に、インスタンスを生成
                        _mapobj[randomiPix, randomjPix].SetActCtrl((ActionControllor)Instantiate(PrefabObj, new Vector3(randomiPix, randomjPix, -1.0F), Quaternion.identity));
                        iLoopflg = true;
                        if (PrefabObj.tag == "Player")
                        {
                            //_mapobj[randomiPix, randomjPix]._actCtrl.StartSetUp();
                            //GameManager.Instance.GetPlayerManager().SetPlayerActionCtrl(_mapobj[randomiPix, randomjPix]._actCtrl);
                            iNow = randomiPix;
                            jNow = randomjPix;
                            iObjState.Add(randomiPix);
                            jObjState.Add(randomjPix);
                            _playerData = _mapobj[randomiPix, randomjPix]._actCtrl.stateData;
                            _displayScript.SetDisplayScript(_playerData);
                            //GameManager.Instance.GetPlayerManager().SetPlayerState(_playerData);
                            charaData.Invoke(_mapobj[randomiPix, randomjPix]._actCtrl, _playerData);
                        }
                        else if (PrefabObj.tag == "Enemy")
                        {
                            //上がなおせればGetComponentが一気に解消できそう？
                            //_mapobj[randomiPix, randomjPix]._actCtrl.StartSetUp();
                            _mapobj[randomiPix, randomjPix]._actCtrl.enemyAtk.GetPlayerStatusData(_playerData);
                            _mapobj[randomiPix, randomjPix]._actCtrl.enemyAtk.GetThisStatusData(_mapobj[randomiPix, randomjPix]._actCtrl.stateData);
                            _mapobj[randomiPix, randomjPix]._actCtrl.stateData.GetPlayerState(_playerData);
                            iObjState.Add(randomiPix);
                            jObjState.Add(randomjPix);
                            charaData.Invoke(_mapobj[randomiPix, randomjPix]._actCtrl, _mapobj[randomiPix, randomjPix]._actCtrl.stateData);
                        }
                    }
                }
            }
            else
            {
            }
        }
    }
    void SetUniqObj(ItemScript PrefabObj)
    {
        //MAP上に出口・プレイヤー等のオブジェクトを追加でセットしていく ※かぶさらないようにする必要あり
        bool iLoopflg = false;
        while (iLoopflg == false)　//出口指定
        {
            int randomiPix = UnityEngine.Random.Range(1, 19);        // 1～19の乱数を取得
            int randomjPix = UnityEngine.Random.Range(1, 19);        // 1～19の乱数を取得

            if (map[randomiPix, randomjPix] != 1)    //MAPが通路のなとき(壁でないとき)
            {
                if (CheckMapstate(randomiPix, randomjPix) == true) //条件が達成されていたら
                {
                    if (CheckMapstateUobj(randomiPix, randomjPix) == true) //条件が達成されていたら
                    {
                        // プレハブを元に、インスタンスを生成
                        _mapobj[randomiPix, randomjPix].SetItem((ItemScript)Instantiate(PrefabObj, new Vector3(randomiPix, randomjPix, -1.0F), Quaternion.identity));
                        iLoopflg = true;
                        if (PrefabObj.tag == "Item")
                        {
                            _mapobj[randomiPix, randomjPix]._Item.GetPosition(randomiPix, randomjPix);
                            makeItem.Invoke(_mapobj[randomiPix, randomjPix]._Item);
                            //GameControllor.Instance.AddCountItemObj(_mapobj[randomiPix, randomjPix]._Item);
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
    public void MapGeneStart(Action<ActionControllor, List<ActionControllor>> finish, Action<ItemScript> setItem, Action<GameObject> setGoal)
    {
        mapNum = UnityEngine.Random.Range(0, 3);        // 0～3の乱数を取得
        EnemyCount = 0;
        UniqObjCount = 0;
        makeItem = setItem;
        goal = setGoal;
        //for文で配列に情報を入れていく(MapDataScript.mapDataだと引数が増えるため)
        for (int iPix = 0; iPix < MapDataScript.mapData.GetLength(1); iPix++) //mapWidth
        {
            for (int jPix = 0; jPix < MapDataScript.mapData.GetLength(2); jPix++) //mapHeight
            {
                MapStatus mapstatus;
                mapstatus = (MapStatus)Instantiate(_mapPrefab, new Vector3(iPix, jPix, 0.0F), Quaternion.identity);
                _mapobj[iPix, jPix] = mapstatus;
                map[iPix, jPix] = MapDataScript.mapData[2, iPix, jPix];// mapNum　//テストで2固定
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
                    _mapobj[iPix, jPix].SetMapObject((GameObject)Instantiate(_wallObj, new Vector3(iPix, jPix, 0.0F), Quaternion.identity));
                }
                else                            //床  
                {
                    // プレハブを元に、インスタンスを生成、
                    _mapobj[iPix, jPix].SetMapObject((GameObject)Instantiate(_floorObj, new Vector3(iPix, jPix, 0.0F), Quaternion.identity));

                }
            }
        }
        //※すでに追加オブジェクトがある場所には生成しないようにする処理が必要
        SetUniqObj(_goalObj);
        SetPlayerObject();
        //_playerObj.GetComponent<ActionControllor>().SetGameCtrl(_gameCtrl);
        ActionControllor playerAction = null;
        StatusDataScript playerState = null;
        SetUniqObj(_playerObj, charaData:(player,status)=>{
            playerAction = player;
            playerState = status;
        }) ;
        SetUniqObj(_powerItemObj);
        SetUniqObj(_weaponItemObj);
        SetUniqObj(_consumptionItemObj);
        UniqObjCount = 1;
        List<ActionControllor> enemyActionList = new List<ActionControllor>();
        for (int Ecount = 0; Ecount < 5; Ecount++)
        {
            SetUniqObj(_enemyObj, charaData: (enemy, status) => {
                enemyActionList.Add(enemy);
                //EnemyListStateData.Add(status);
            });
            EnemyCount += 1;
            UniqObjCount +=1;
        }
        //アイテム等はここで同じ用に生成

        //コントローラの初期化関数呼び出し
        finish.Invoke(playerAction,enemyActionList);
        //GameControllplayerState or.Instance.AftorMakeMapStart();
    }
}
