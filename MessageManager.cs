using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] GameObject messagePanelPrefab;
    [SerializeField] Camera cam;
    [SerializeField] Canvas canvas;

    [SerializeField] GameObject speechBubble;

    public static MessageManager instance;

    void Awake()
    {
        instance = this;
    }

    public void CallMsgPanel(string msg, Transform targetTransform, Color color)
    {
            GameObject o = Instantiate(messagePanelPrefab, GameManager.instance.player.transform);
            
            o.transform.SetParent(canvas.transform); //I have to do this since this is a panel. If not, the panel will not be instantiated. 
            o.GetComponent<MessagePanelController>().ShowMsg(msg, cam, targetTransform, color); //Call the ShowMsg() function in the
                                                                                         //MessagePanelController script. 
    }
    
    public void CallSpeechBubble(Transform target, int bubbleID)
    {
        GameObject go = Instantiate(speechBubble, target);
        go.transform.position = new Vector3(target.position.x, 
            target.position.y+2.5f, target.position.z); //slightly higher above the transform's position. 
        go.GetComponent<Animator>().SetInteger("speechBubbleID", bubbleID);

    }
}
