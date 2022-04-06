using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimationFunctionContainer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }




	public void ReleaseLoot()
	{
		gameObject.GetComponent<ToolHit>().ReleaseLoot();
	}

}
