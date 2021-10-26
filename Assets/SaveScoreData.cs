using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveScoreData : MonoBehaviour
{
    ReadResultData _scoreData;
    //private string inputString;

    // Start is called before the first frame update
    void Start()
    {
        _scoreData = ReadResultData.Instance;
        //inputString = Resources.Load<TextAsset>("Score").ToString();

        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(_scoreData); 

        writer = new StreamWriter(Application.dataPath + "/Resources/Score.json", false);
        //writer = new StreamWriter(inputString, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
