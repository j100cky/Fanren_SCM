using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
	[SerializeField] GameObject highlighter; //allow we define the icon of the highlighter 

	GameObject currentTarget; //prevent the Highlight method to run when the target is the same. 

	public void Highlight(GameObject target) //make a highlight function, it contains a parameter "target"
	{
		if(currentTarget == target)
		{
			return;
		}
		Vector3 position = target.transform.position + Vector3.up * 1f; //make a vector var "position", its parameters are transferred from the target.
		ShowHighlight(position); //Use the method Highlight at the position defined by the var "position"
	}

	public void ShowHighlight(Vector3 position) //more definition on the method Highlight. It contains a parameter position
	{
		highlighter.SetActive(true); //When the method Highlight is toggled, it shows the highlighter icon. 
		highlighter.transform.position = position; //set the highlighter position as the position defined by the var "position". Which is defined by the target. 
	}

	public void Hide()
	{
		currentTarget = null;
		highlighter.SetActive(false); //this Hide method will make the highlighter icon disappear when used. 
	}
}
