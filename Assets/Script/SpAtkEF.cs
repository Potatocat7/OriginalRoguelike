using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpAtkEF : MonoBehaviour {
    /// <summary>プレイヤー情報</summary>
    [SerializeField]
    ActionControllor Player = null;
    /// <summary>エフェクトのレンダー</summary>
    [SerializeField]
    SpriteRenderer spriteRenderer;
    /// <summary>アニメーション</summary>
    [SerializeField]
    Animator animator;

    /// <summary>
    /// TODO:Updateで処理をしないようにする
    /// </summary>
	void Update () {
        if (Player.SpAtkEfFlg == true)
        {
            spriteRenderer.enabled = true;
            animator.SetTrigger("StartEF");
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
