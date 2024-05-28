using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PItemScript : ItemScript
{
    /*SPアイテムをItemScriptで継承*/
    /// <summary>
    /// 取得時処理
    /// </summary>
    public override void GetDestroy()
    {
        GameManager.Instance.SetPItemFlg(true);
        Destroy(gameObject);

    }
}
