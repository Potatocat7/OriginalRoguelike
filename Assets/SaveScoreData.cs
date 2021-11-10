using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScoreData : MonoBehaviour
{
    private ReadResultData _scoreData;
    public ScoreStatus[] _scoreDataList;
    private SaveDataScript _stateData;
    [SerializeField]
    private ViewScoreRanking _ranking;


    // Start is called before the first frame update
    void Start()
    {
        _scoreData = ReadResultData.Instance;
        _scoreDataList = _scoreData.GetInputScoreJson();
        _stateData = SaveDataScript.Instance;
        int ListMax = _scoreDataList.Length;
        bool afterSet = false;
        ScoreStatus[] _scoreStackList = new ScoreStatus[ListMax];
        //・リストに今回のデータを並び替えで差し込む
        for (int i = 0;i < ListMax; i++)
        {
            if (_scoreDataList[i].clearFloor > _stateData.GetSaveData().clearFloor)
            {
                _scoreStackList[i] = _scoreDataList[i];
            }
            else
            {
                if(afterSet == false)
                {
                    _scoreStackList[i] = _stateData.GetSaveData();
                    afterSet = true;
                }
                else
                {
                    _scoreStackList[i] = _scoreDataList[i - 1];
                }
            }
        }
        _scoreData.SetInputScoreJson(_scoreStackList);
        //・ENDシーンでランキングを表示
        _scoreData.SaveWriteData();
        _ranking.SetRanking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
