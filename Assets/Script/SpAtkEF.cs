using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpAtkEF : MonoBehaviour {
    int count;
    [SerializeField]
    bool AttackEffect;
    [SerializeField]
    ActionControllor Player = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.SpAtkEfFlg == true)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.GetComponent<Animator>().SetTrigger("StartEF");
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
