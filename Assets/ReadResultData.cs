using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InputScoreJson
{
    public ScoreStatus[] scoreList;
}

[Serializable]
public struct ScoreStatus
{
    public int clearFloor;
    //public BulletSelector.BulletTypeKind typeBullet;
    //public int bulletInterval;
    //public float enemySpeed;
    //public int enemyHitPoint;
    //public EnemyManager.EnemyMoveTypeKind moveType;
    //public EnemyManager.EnBullDirTypeKind bulletDirection;
}
public class ReadResultData : MonoBehaviour
{
    [SerializeField]
    private InputScoreJson _scoreJson;
    [SerializeField]
    private ReadResultData _thisData;
    private string inputString;
    private static ReadResultData mInstance;

    public static ReadResultData Instance
    {
        get
        {
            return mInstance;
        }
    }
    //リザルト結果のデータを読み込み
    //ランキング形式にして上位10位までを表示
    //JSONファイルで保存して読み込む予定
    // Start is called before the first frame update
    void Awake()
    {
        inputString = Resources.Load<TextAsset>("Score").ToString();
        _scoreJson = JsonUtility.FromJson<InputScoreJson>(inputString);
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
