using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour 
{
    /// <summary>
    /// スタートボタン
    /// </summary>
    public void PushStartbutton()
    {
        SceneManager.LoadScene("SelectScene");
    }
}
