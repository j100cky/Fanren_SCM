using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to the MainCharacter object. 
public class SkillBarController : MonoBehaviour
{
    public int skillBarSize; //Will pass the number of skills in different skillbooks into this variable.
    public int selectedSkill;
    [SerializeField] GameObject skillBar;
    public bool isSkillBarActive = false; //When R is pressed, this value alternates. 

    [SerializeField] SkillIconHighlight skillIconHighlight; //For skill cast preview.


    public Action<int> onChange; //Essential for detecting actions for scrolling wheel.

    private void Start()
    {
        //This is for showing skill preview icons. 
/*        onChange += HighlightedSkillIcon;
        HighlightedSkillIcon(selectedSkill);*/

        skillIconHighlight.SkillbarActiveCheck(isSkillBarActive);
    }

    private void Update()
    {
        skillIconHighlight.SkillbarActiveCheck(isSkillBarActive);
        if (isSkillBarActive == false)
        {
            return;
        } 
        float delta = Input.mouseScrollDelta.y;
        if(delta != 0)
        {
            Debug.Log("changed");
            if(delta >0)
            {
                selectedSkill -= 1;
                selectedSkill = (selectedSkill < 0 ? skillBarSize - 1 : selectedSkill);
            }
            else
            {
                selectedSkill += 1;
                selectedSkill = (selectedSkill >= skillBarSize ? 0 : selectedSkill);
            }
            onChange ?. Invoke(selectedSkill);
        }
    }

    public void ShowSkillBar()
    {
        skillBar.SetActive(true);
    }

    public void HideSkillBar()
    {
        skillBar.SetActive(false);
    }
  
    

/*    public void HighlightedSkillIcon(int id = 0)
    {
        iconHighlight.Show = true;
    }*/
}
