using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PItemScript : ItemScript
{
    //構造体を用意しても良いが今回はタイプだけ準備

    // Use this for initialization
    void Start () {
		
	}
	
    public override void GetDestroy()
    {
        GameManager.Instance.SetPItemFlg(true);
        Destroy(gameObject);

    }
    // Update is called once per frame
    void Update () {
	}
}
