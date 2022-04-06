using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedScriptTemporary : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            GameSceneManager.instance.InitSwitchToEventScene("DailySummary", new Vector3(0f,4.7f,0f));
        }
    }
}
