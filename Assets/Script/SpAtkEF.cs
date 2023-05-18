using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpAtkEF : MonoBehaviour {
    int count;
    [SerializeField]
    bool AttackEffect;
    [SerializeField]
    ActionControllor Player = null;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Animator animator;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.SpAtkEfFlg == true)
        {
            //GetComponentをなくしたい（とくにUpdate内だからなおのこと）
            spriteRenderer.enabled = true;
            animator.SetTrigger("StartEF");
        }
        else
        {
            spriteRenderer.enabled = false;
        }

    }
}
