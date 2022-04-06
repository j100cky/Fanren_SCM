using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolToSkillBarController : MonoBehaviour
{
    public SkillBarController skillBarController;
    public ToolbarController toolBarController;
    public static ToolToSkillBarController instance;


    private void Awake()
    {
        instance = this;
    }
/*    private void Start()
    {
        if (toolBarController.isToolbarActive == true)
        {
            skillBarController.HideSkillBar();
            Debug.Log("3");
        }
    }*/

    void Update()
    {
        // For unknown reasons the skillbar keeps showing up when scene switched. This is patched dumbly here.
        if (toolBarController.isToolbarActive == true)
        {
            skillBarController.HideSkillBar();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            skillBarController.isSkillBarActive = (!skillBarController.isSkillBarActive);
            toolBarController.isToolbarActive = (!toolBarController.isToolbarActive);
            if(skillBarController.isSkillBarActive)
            {
                skillBarController.ShowSkillBar();

                Debug.Log("Switch to skillbar");
            }
            else
            {
                skillBarController.HideSkillBar();

                Debug.Log("Switch to toolbar");
            }
            
        }
        
    }
}
