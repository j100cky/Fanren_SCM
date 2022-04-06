using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
	public GameObject structureToFade;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Color temp = structureToFade.GetComponent<SpriteRenderer>().color;
		temp.a = 0.3f;
		structureToFade.GetComponent<SpriteRenderer>().color = temp;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Color temp = structureToFade.GetComponent<SpriteRenderer>().color;
		temp.a = 1.0f;
		structureToFade.GetComponent<SpriteRenderer>().color = temp;
	}
}
