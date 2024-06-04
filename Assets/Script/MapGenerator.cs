using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    /// <summary>マッププレハブ</summary>
    [SerializeField] private MapStatus _mapPrefab;
    /// <summary>壁プレハブ</summary>
    [SerializeField] private GameObject _wallObj;
    /// <summary>ゴールプレハブ</summary>
    [SerializeField] private GameObject _goalObj;
    /// <summary>エネミープレハブ</summary>
    [SerializeField] private ActionControllor _enemyObj;
    /// <summary>プレイヤー選択</summary>
    [SerializeField] private PlayerSelector _playerSelectObj;
    /// <summary>床オブジェクト</summary>
    [SerializeField] private GameObject _floorObj;
    /// <summary>未　回復アイテムオブジェクト</summary>
    [SerializeField] private GameObject _healItemObj;
    ///// <summary>装備オブジェクト</summary>
    //[SerializeField] private ItemScript _weaponItemObj;
    /// <summary>落ちてるアイテムオブジェクト</summary>
    [SerializeField] private ItemScript _dropItemObj;
    ///// <summary>SPアイテムオブジェクト</summary>
    //[SerializeField] private ItemScript _powerItemObj;
    /// <summary>MAP情報</summary>
    [SerializeField] private MapStatus[,] _mapobj = new MapStatus[20, 20];
    /// <summary>プレイヤー情報</summary>
    private ActionControllor _playerObj;
    /// <summary>選択キャラクター情報</summary>
    private SaveCharaSelect CharaNum;
    /// <summary>MAP配列</summary>
    public static int[,] map = new int[20, 20];       //選択後の
    /// <summary>現在地・敵数・オブジェクト個数</summary>
    public static int iNow, jNow, EnemyCount,UniqObjCount;
    /// <summary>マップ種類情報</summary>
    public int mapNum;
    /// <summary>プレイヤーステータス</summary>
    private StatusDataScript _playerData;
    /// <summary>オブジェクト情報</summary>
    public List<int> iObjState = new List<int>();
    public List<int> jObjState = new List<int>();
    /// <summary>生成アイテムコールバック</summary>
    private Action<ItemScript> makeItem;
    /// <summary>ゴール達成コールバック</summary>
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

    /// <summary>
    /// MAP状態チェック
    /// </summary>
    /// <param name="tate"></param>
    /// <param name="yoko"></param>
    /// <returns></returns>
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

    /// <summary>
    /// アイテムドロップのランダム値
    /// </summary>
    /// <returns></returns>
    private ItemScript.ItemType GetItemDropRandam()
    {
        int maxCount = Enum.GetNames(typeof(ItemScript.ItemType)).Length;
        int number = UnityEngine.Random.Range(0, maxCount);
        ItemScript.ItemType testType = (ItemScript.ItemType)Enum.ToObject(typeof(ItemScript.ItemType), number);
        return testType;
    }

    /// <summary>
    /// ドロップアイテム設定
    /// </summary>
    /// <param name="iPix"></param>
    /// <param name="jPix"></param>
    public void SetDropItemObj(int iPix,int jPix)//type itemType)
    {
        ItemScript.ItemType itemnum = GetItemDropRandam();

        switch (itemnum)
        {
            case ItemScript.ItemType.NONE:
                ///ドロップなしなので戻る
                return;
            case ItemScript.ItemType.CONSUM:
            case ItemScript.ItemType.EQUIP:
                break;
            case ItemScript.ItemType.SPECIAL:
                //特殊は消費アイテムにしてドロップ
                itemnum = ItemScript.ItemType.CONSUM;
                break;
            default:
                return;
        }


        //MAP上に出口・プレイヤー等のオブジェクトを追加でセットしていく ※かぶさらないようにする必要あり
        ItemScript PrefabObj = _dropItemObj;

        if (map[iPix, jPix] != 1)    //MAPが通路のなとき(壁でないとき)
        {
            // プレハブを元に、インスタンスを生成
            ItemScript setItem = (ItemScript)Instantiate(PrefabObj, new Vector3(iPix, jPix, -1.0F), Quaternion.identity);
            _mapobj[iPix, jPix].SetItem(setItem);
            if (PrefabObj.tag == "Item")
            {
                _mapobj[iPix, jPix]._Item.Init(iPix, jPix, itemnum);
                makeItem.Invoke(_mapobj[iPix, jPix]._Item);
            }
        }
        else
        {
        }
    }

    /// <summary>
    /// オブジェクト生成（GameObject）
    /// </summary>
    /// <param name="PrefabObj"></param>
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
                            iNow = randomiPix;
                            jNow = randomjPix;
                            iObjState.Add(randomiPix);
                            jObjState.Add(randomjPix);
                            _playerData = _mapobj[randomiPix, randomjPix]._actCtrl.stateData;
                            charaData.Invoke(_mapobj[randomiPix, randomjPix]._actCtrl, _playerData);
                        }
                        else if (PrefabObj.tag == "Enemy")
                        {
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

    /// <summary>
    /// アイテムの生成
    /// </summary>
    /// <param name="PrefabObj"></param>
    void SetUniqObj(ItemScript PrefabObj, ItemScript.ItemType type)
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
                            _mapobj[randomiPix, randomjPix]._Item.Init(randomiPix, randomjPix,type);
                            makeItem.Invoke(_mapobj[randomiPix, randomjPix]._Item);
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
    /// プレイヤーの情報設定
    /// </summary>
    private void SetPlayerObject()
    {
        _playerObj = _playerSelectObj.SelectTypeBullet(CharaNum.CharaNumber);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="finish"></param>
    /// <param name="setItem"></param>
    /// <param name="setGoal"></param>
    public void Init(Action<ActionControllor, List<ActionControllor>> finish, Action<ItemScript> setItem, Action<GameObject> setGoal)
    {
        mapNum = UnityEngine.Random.Range(0, 3);        // 0～3の乱数を取得
        EnemyCount = 0;
        UniqObjCount = 0;
        makeItem = setItem;
        goal = setGoal;
        //for文で配列に情報を入れていく
        for (int iPix = 0; iPix < MapDataScript.mapData.GetLength(1); iPix++) //mapWidth
        {
            for (int jPix = 0; jPix < MapDataScript.mapData.GetLength(2); jPix++) //mapHeight
            {
                _mapobj[iPix, jPix] = _mapPrefab;
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
        ActionControllor playerAction = null;
        StatusDataScript playerState = null;
        SetUniqObj(_playerObj, charaData:(player,status)=>{
            playerAction = player;
            playerState = status;
        }) ;
        SetUniqObj(_dropItemObj, ItemScript.ItemType.CONSUM);
        SetUniqObj(_dropItemObj, ItemScript.ItemType.EQUIP);
        SetUniqObj(_dropItemObj, ItemScript.ItemType.SPECIAL);
        UniqObjCount = 1;
        List<ActionControllor> enemyActionList = new List<ActionControllor>();
        for (int Ecount = 0; Ecount < 5; Ecount++)
        {
            SetUniqObj(_enemyObj, charaData: (enemy, status) => {
                enemyActionList.Add(enemy);
            });
            EnemyCount += 1;
            UniqObjCount +=1;
        }
        //コントローラの初期化関数呼び出し
        finish.Invoke(playerAction,enemyActionList);
    }
}
