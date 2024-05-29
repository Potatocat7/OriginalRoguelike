using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpAtkEF : MonoBehaviour {
    /// <summary>エフェクトのレンダー</summary>
    [SerializeField]
    SpriteRenderer spriteRenderer;
    /// <summary>アニメーション</summary>
    [SerializeField]
    Animator animator;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        spriteRenderer.enabled = false;
    }
    /// <summary>
    /// エフェクト再生
    /// </summary>
	public void PlayEffect(bool effect) 
    {
        if (effect == true)
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
