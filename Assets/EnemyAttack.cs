using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [SerializeField]
    int iiPstate, jjPstate, iiEstate, jjEstate;

    public void AttackHit()
    {
        GameObject　Player = GameObject.Find("PlayerPrefab(Clone)");
        Player.GetComponent<StatusDataScript>().HitDamage(this.GetComponent<StatusDataScript>().Attack);
    }

    public bool CheckPlayerThisAround(int iPstate, int jPstate, int iEstate, int jEstate)
    {
        iiPstate = iPstate;
        jjPstate = jPstate;
        iiEstate = iEstate;
        jjEstate = jEstate;
        if (iEstate == iPstate && jEstate + 1 == jPstate)
        {//UP
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP);
            return true;
        }
        else if (iEstate - 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_LEFT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_LEFT);
            return true;
        }
        else if (iEstate + 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_RIGHT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_RIGHT);
            return true;
        }
        else if (iEstate - 1 == iPstate && jEstate == jPstate)
        {//LEFT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.LEFT);
            return true;
        }
        else if (iEstate + 1 == iPstate && jEstate == jPstate)
        {//RIGHT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.RIGHT);
            return true;
        }
        else if (iEstate == iPstate && jEstate - 1 == jPstate)
        {//DOWN
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN);
            return true;
        }
        else if (iEstate - 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_LEFT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_LEFT);
            return true;
        }
        else if (iEstate + 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_RIGHT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_RIGHT);
            return true;
        }
        else
        {//周囲にいなかったとき
            return false;
        }
    }
    public void SetDirectionPlayerThisAround(int iPstate, int jPstate, int iEstate, int jEstate)
    {
        if (iEstate == iPstate && jEstate + 1 == jPstate)
        {//UP
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP);
        }
        else if (iEstate - 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_LEFT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_LEFT);
        }
        else if (iEstate + 1 == iPstate && jEstate + 1 == jPstate)
        {//UP_RIGHT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.UP_RIGHT);
        }
        else if (iEstate - 1 == iPstate && jEstate == jPstate)
        {//LEFT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.LEFT);
        }
        else if (iEstate + 1 == iPstate && jEstate == jPstate)
        {//RIGHT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.RIGHT);
        }
        else if (iEstate == iPstate && jEstate - 1 == jPstate)
        {//DOWN
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN);
        }
        else if (iEstate - 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_LEFT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_LEFT);
        }
        else if (iEstate + 1 == iPstate && jEstate - 1 == jPstate)
        {//DOWN_RIGHT
            this.GetComponent<ActionControllor>().SetDirection(ActionControllor.Direction.DOWN_RIGHT);
        }
        else
        {//周囲にいなかったとき
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
