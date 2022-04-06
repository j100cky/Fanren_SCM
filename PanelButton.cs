using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButton : MonoBehaviour
{
    [SerializeField] GameObject activeIcon;
    [SerializeField] GameObject inactiveIcon;
    int myIndex; 

    public void Set(int id)
    {
        myIndex = id;
    }

    //Called by the ButtonPanel script to make the button look gray. 
    public void InactivateMe()
    {
        activeIcon.SetActive(false);
        inactiveIcon.SetActive(true);
    }

    //Show the active icon. Called by onClick.
    public void ActivateMe()
    {
        if (gameObject.GetComponentInParent<ButtonPanel>() != null)
        {
            gameObject.GetComponentInParent<ButtonPanel>().InactivateAll();
        }
        activeIcon.SetActive(true);
        inactiveIcon.SetActive(false);
    }


}
