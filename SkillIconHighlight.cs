using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconHighlight : MonoBehaviour
{
	bool isSkillbarActive;
	bool isWithinRange;

	private void Update()
	{
        if (isSkillbarActive == false)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }
       if (isWithinRange == false)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
		//Set(); //Setting the highlight active only when both the Skillbar is active and the mouse position is within range. 
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //We will use a simple mouse position here because it makes more sense. 
		transform.position = new Vector3(transform.position.x, transform.position.y, 0); //For some reason z position is automatically -10. 
	}

	//Need some condition that take in the consideration of attack range.

	public void SkillbarActiveCheck(bool b) //This function will be called in the SkillBarController script to pass the value of isSkillBarActive to here. 
    {
		isSkillbarActive = b;
    }

	public void RangeCheck(bool c)
    {
		isWithinRange = c;
	}

	private void Set()
    {
        if(isSkillbarActive == true)
        {
			gameObject.SetActive(true);
        }
        else
        {
			gameObject.SetActive(false);
		}
			

    }


}
