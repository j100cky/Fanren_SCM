using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTint : MonoBehaviour
{
    [SerializeField] Color unTintedColor;
    [SerializeField] Color tintedColor;
    float f;
    public float speed = 0.5f;

    Image image;

    private void Awake()
    {
    	image = GetComponent<Image>(); //Import the black image for tinting.
    }

    public void Tint()
    {
    	StopAllCoroutines();
    	f = 0f;
    	StartCoroutine(TintScreen());
    }

    public void UnTint()
    {
    	StopAllCoroutines();
    	f = 0f;
    	StartCoroutine(UnTintScreen());
    }

    private IEnumerator TintScreen()
    {
	    while(f < 1f)
	    {
	    	f +=Time.deltaTime * speed;   //f is increased by real time x the speed.
	    	f = Mathf.Clamp (f, 0, 1f);   //the value of f should be between 0 and 1.
	    	Color c = image.color;    //Get the color of the image, which is black. 
	    	c = Color.Lerp(unTintedColor, tintedColor, f);    //Change the color c from the unTint color (transluscent black) to the Tint color (black), and the alpha is f.
	    	image.color = c;  //Change the color of the image to c. So the color of the image will be from clear to alpha=1 black. 

	    	yield return new WaitForEndOfFrame();
	    }
        //UnTint();   //For testing, call the UnTint() function to return to untint scene. 
    }

    private IEnumerator UnTintScreen()
    {
	    while(f < 1f)
	    {
	    	f +=Time.deltaTime * speed;
	    	f = Mathf.Clamp (f, 0, 1f);
	    	Color c = image.color;
	    	c = Color.Lerp(tintedColor, unTintedColor,  f);
			image.color = c;

	    	yield return new WaitForEndOfFrame();
	    }
    }
}
