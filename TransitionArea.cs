using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionArea : MonoBehaviour
{
	public bool isCharm; 

	private void OnTriggerEnter2D(Collider2D collision)
	{

		if(isCharm == true)
        {
			if (collision.transform.CompareTag("Player"))
			{
				transform.parent.GetComponent<TransisionCharm>().IntiateTransition(collision.transform);
			}
		}

        else
        {
			if (collision.transform.CompareTag("Player"))
			{
				transform.parent.GetComponent<Transition>().InitiateTransition(collision.transform);
			}
		}


	}
}
