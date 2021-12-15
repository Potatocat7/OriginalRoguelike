using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PItemScript : ItemScript
{


    // Use this for initialization
    void Start () {
		
	}
	
    public override void GetDestroy()
    {
        GameControllor.Instance.OnGetPItemFlg();
        Destroy(gameObject);

    }
    // Update is called once per frame
    void Update () {
	}
}
