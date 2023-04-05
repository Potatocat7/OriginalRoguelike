using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkEfScript : MonoBehaviour {

    int count;
    [SerializeField]
    bool AttackEffect;
    [SerializeField]
    GameObject Player = null;
    void SetEffecrDirection()
    {
        switch (Player.GetComponent<ActionControllor>().thisNowDirection)//ここのコンポーネントを外せるきがする
        {
            case ActionControllor.Direction.UP:
                this.transform.localPosition = new Vector3(0.01f, 0.15f, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, -135);
                break;
            case ActionControllor.Direction.UP_LEFT:
                this.transform.localPosition = new Vector3(-0.09f, 0.06f, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case ActionControllor.Direction.UP_RIGHT:
                this.transform.localPosition = new Vector3(0.07f, 0.02f, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case ActionControllor.Direction.LEFT:
                this.transform.localPosition = new Vector3(-0.09f, 0, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, -45);
                break;
            case ActionControllor.Direction.RIGHT:
                this.transform.localPosition = new Vector3(0.05f, -0.05f, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, 135);
                break;
            case ActionControllor.Direction.DOWN:
                this.transform.localPosition = new Vector3(0, -0.15f, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, 45);
                break;
            case ActionControllor.Direction.DOWN_LEFT:
                this.transform.localPosition = new Vector3(-0.04f, -0.15f, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case ActionControllor.Direction.DOWN_RIGHT:
                this.transform.localPosition = new Vector3(0.05f, -0.15f, 0);
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {//Update内の処理を外に出せる気がする
        SetEffecrDirection();
        if (Player.GetComponent<ActionControllor>().AtkEfFlg == true)
        {
            this.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            this.GetComponent<Renderer>().enabled = false;
        }

    }
}
