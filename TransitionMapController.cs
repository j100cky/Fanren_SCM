using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMapController : MonoBehaviour
{
    public TransitionTargetsPanel panel;

    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            gameObject.SetActive(false);
        }
        if(Input.GetKeyDown("t"))
        {
            gameObject.SetActive(true);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
