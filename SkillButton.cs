using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image skillIcon;
    [SerializeField] Sprite background; //To replace the skillIcon when there is no skilldata. 
    [SerializeField] Image highlight;
    public SkillDatasheet skillDatasheet;
    Color color;
    [SerializeField] GameObject descriptionPanel;
    [SerializeField] Text unlockLevel;

    public int myIndex;

    private void Start()
    {
        descriptionPanel = GameManager.instance.skillDescriptionPanel;
    }

    public void SetIndex(int index)
    {
        myIndex = index;
    }

    public void SetUnlearnedSkill(SkillDatasheet skill)
    {
        skillIcon.sprite = skill.skillIcon;
        //Set the color to gray to show that the skill is not yet learned. 
        skillIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    public void SetSkill(SkillDatasheet skill)
    {
        skillIcon.sprite = skill.skillIcon;
        //Set the color back to white so that newly learned skills will not be still gray. 
        skillIcon.color = new Color(1f, 1f, 1f, 1f);
        skillDatasheet = skill; //Update the skillDatasheet for this button so that the SkillBarPanel can access this information 
    }

    public void Clear()
    {
        skillIcon.sprite = background;
        highlight.sprite = null;
        skillDatasheet = null;
    }

    public void Highlight(bool b)
    {
        highlight.gameObject.SetActive(b);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowDescriptionPanel();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideDescriptionPanel();
    }

    public void ShowDescriptionPanel()
    {
        descriptionPanel.SetActive(true);
        descriptionPanel.transform.position = new Vector3(Input.mousePosition.x + 0.1f, 
            Input.mousePosition.y - 0.1f, 
            Input.mousePosition.z);
        descriptionPanel.GetComponent<SkillDescriptionPanelScript>().SetDescription(skillDatasheet);
    }

    public void HideDescriptionPanel()
    {
        descriptionPanel.SetActive(false);
    }

    //Called by the SkillbookDetailController script to show at which level will the skill be learned. 
    public void ShowUnlockLevel(int level)
    {
        unlockLevel.gameObject.SetActive(true);
        unlockLevel.text = "Lv." + level.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.instance.skillDragAndDropController.OnLeftClick(this);
        }
    }
}
