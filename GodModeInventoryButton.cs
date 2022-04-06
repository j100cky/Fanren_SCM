using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeInventoryButton : MonoBehaviour
{
    [SerializeField] GameObject godModePanel; 

    public void ShowGodModePanel()
    {
        godModePanel.SetActive(!godModePanel.activeInHierarchy);
    }
}
