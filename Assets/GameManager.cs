using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField]
    //private ActionControllor _actionCtrl = null;
    [SerializeField]
    private MapGenerator _mapGeneObj = null;
    [SerializeField]
    private GameControllor _gameCtrl = null;
    // Start is called before the first frame update 
    void Start()
    {
        _mapGeneObj.MapGeneStart();
        _gameCtrl.GameCtrlStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
