using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMountController : MonoBehaviour
{

	public bool isRiding = false;
	GameObject flyingMount;
	GameObject player;

	void Start()
	{
		 player = GameManager.instance.player; 
	}

    //Called by the FlyingSwordSkillScript to set the gameobject of the flying mount. 
	public void SetFlyingMount(GameObject go)
    {
		flyingMount = go;
    }

    void Update()
    {
        //Determine the position and facing of the flying mount. 
        if(isRiding == true && flyingMount != null)
        {
			flyingMount.transform.position = new Vector3(player.transform.position.x, player.transform.position.y-2f, 
                player.transform.position.z);
            flyingMount.GetComponent<SkillController>().SetDirection();
        }
        else
        {
            //Debug.Log("flying mount not found");
            return;
        }
    }
}
