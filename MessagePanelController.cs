using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanelController : MonoBehaviour
{
    [SerializeField] Text msgText;
    //[SerializeField] Camera cam;
    //Color originalColor;

/*    void Awake()
    {
        foreach (Camera c in Camera.allCameras)
        {
            if (c.name == "Main Camera")
            {
                Camera cam = c;
            }

        }
    }*/

/*    void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            ShowMsg("message");
        }
    }*/

/*    public void ShowMsg(string msg)
    {
        gameObject.SetActive(true);
        msgText.text = msg;
        transform.position = cam.WorldToScreenPoint(GameManager.instance.player.transform.position);
        transform.position = new Vector3(transform.position.x, 
            transform.position.y + 100f, transform.position.z); //Adjust the y-pos of the message panel.
*//*        for(int i = 0; i < 50; i++)
        {
            transform.position = new Vector3(transform.position.x,
            transform.position.y + 2f, transform.position.z);
            var tempColor = msgText.color;
            tempColor.a = 255f - 5f * i;
            msgText.color = tempColor;
        }*//* //Happening too fast.
        //gameObject.SetActive(false);
    }*/

    public void ShowMsg(string msg, Camera cam, Transform targetTransform, Color color)
    {
        msgText.text = msg;
        msgText.color = color;
        msgText.transform.position = cam.WorldToScreenPoint(targetTransform.position); //The position will be weird if I don't
                                                                                       //convert the position to screen point. 
        transform.position = new Vector3(transform.position.x,
            transform.position.y + 400f, transform.position.z); //Adjust the y-pos of the message panel so it is above the object.
        StartCoroutine(FadePanel(msgText, msgText.color.a, 0,
            transform.position.y, transform.position.y + 100f)); //Starting the Coroutine. 

    }

    public IEnumerator FadePanel(Text msg, float startAlpha, float endAlpha, float startY, float endY, float lerpTime = 0.5f)
    {
        float timeStartedLerping = Time.time; //The beginning time.
        float timeSinceStarted = Time.time - timeStartedLerping; //The time after the beginning time.
        float percentageComplete = timeSinceStarted / lerpTime; //Lerp time is the time needed to finish the action.
                                                                //In this case, how long will the message textbox be visible. 
                                                                //When percentageComplete reaches the lerptime, the loop will break, 
                                                                //stopping the Ienumerator. 

        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping; //Update the timeSinceStarted variable. 
            percentageComplete = timeSinceStarted / lerpTime; //Update the percentageComplete variable. 

            float currentValue = Mathf.Lerp(startAlpha, endAlpha, percentageComplete); //Calculating what the value is between
                                                                                       //startAlpha and endAlpha according to percentageComplete.
                                                                                       //This currentValue will be passed to set the 
                                                                                       //opacity of the textbox. 
            float currentHeight = Mathf.Lerp(startY, endY, percentageComplete); //This currentHeight will be passed to set the y-position
                                                                                //of the textbox. 

            var tempColor = msg.color; //I have to store the color into a new variable because I can't directly change the alpha value. 
            tempColor.a = currentValue;
            msg.color = tempColor;

            msg.transform.position = new Vector3(msg.transform.position.x, currentHeight, msg.transform.position.z);

            if (percentageComplete >= 1) break; //If the action is completed (time reaches 0.5 second), stop the process. 

            yield return new WaitForEndOfFrame(); //Need this line to function properly. 

        }
        Destroy(gameObject); //Destroy the prefab after the message is shown. 
    }

}
