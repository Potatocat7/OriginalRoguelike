using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkEfScript : MonoBehaviour {

    private int count;
    [SerializeField]
    private bool AttackEffect;
    [SerializeField]
    private ActionControllor Player = null;
    [SerializeField]
    private Renderer thisRender = null;

    public void SetEffecrDirection()
    {
        switch (Player.thisNowDirection)
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
    public void EffectEnabled(bool flg)
    {
        if (flg == true)
        {
            thisRender.enabled = true;
        }
        else
        {
            thisRender.enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {//Update内の処理を外に出せる気がする
        //SetEffecrDirection();
        //if (Player.AtkEfFlg == true)
        //{
        //    thisRender.enabled = true;
        //}
        //else
        //{
        //    thisRender.enabled = false;
        //}

    }
}
