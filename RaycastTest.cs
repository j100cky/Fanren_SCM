using UnityEngine;

public class RaycastTest : MonoBehaviour
{
	Vector2 lastMotionVector2D;
	RaycastHit2D hitInfo;

    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
        	Shoot();
        }
    }

    void Shoot()
    {
    	lastMotionVector2D = GetComponent<CharacterController2D>().lastMotionVector;	//Obtain the player's current position
    	RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, lastMotionVector2D);	//Get the information of the object hit. Return a bool variable.
    	if(hitInfo)
    	{
    		Debug.Log(hitInfo.transform.name);	//Report the name of the object being hit.
    	}
    }
}
