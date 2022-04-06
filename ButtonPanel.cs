using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    [SerializeField] public List<PanelButton> panelButtons;

    private void Start()
    {
        SetPanelButton();
    }

    //Set the index for each button. 
    private void SetPanelButton()
    {
        for (int i = 0; i < panelButtons.Count; i++)
        {
            panelButtons[i].Set(i);
        }
    }

    public void InactivateAll()
    {
        for(int i = 0; i< panelButtons.Count; i++)
        {
            panelButtons[i].InactivateMe();
        }
    }

}
