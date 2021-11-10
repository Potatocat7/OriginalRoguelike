using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewScoreRanking : MonoBehaviour
{
    [SerializeField]
    private SaveScoreData _scoreData;
    [SerializeField]
    private Text _scoreRanking;
    //private ScoreStatus[] _scoreDataList;

    // Start is called before the first frame update
    public void SetRanking()
    {
        _scoreRanking.text = "";
        for (int i = 0; i < _scoreData._scoreDataList.Length; i++)
        {
            _scoreRanking.text += (i+1)+"位："+_scoreData._scoreDataList[i].clearFloor.ToString()+"\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
