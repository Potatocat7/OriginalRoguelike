using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour {

	/// <summary>
	/// ボタン押下処理
	/// </summary>
    public void PushEndbutton()
	{
		SaveDataScript.Instance.ClearData();
		SaveDataScript.Instance.Destroy();
		SceneManager.LoadScene("StartScene");
    }
}
