using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkEfScript : MonoBehaviour {

    /// <summary>プレイヤー</summary>
    [SerializeField]
    private ActionControllor Player = null;
    /// <summary>表示レンダー</summary>
    [SerializeField]
    private Renderer thisRender = null;

    /// <summary>
    /// 方向に合わせたエフェクトの回転
    /// </summary>
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

    /// <summary>
    /// エフェクトのオンオフ
    /// </summary>
    /// <param name="flg"></param>
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
}
