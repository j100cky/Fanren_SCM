using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OneSkillbookPanelScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] Text skillbookName;
    [SerializeField] Text skillbookLevel;
    [SerializeField] Image highlight;
    [SerializeField] SkillbookButton button;
    [SerializeField] Image mainSkillbookStamp;
    public Skillbooks skillbookItemSlot;
    int myIndex;

    [SerializeField] NumberWordPairs numWordPairs; //For converting arabic numbers to chinese characters. Found in the Asset folder.

    private void Awake()
    {
        numWordPairs.MakeDictionary();
    }

    //Setting the index of this particular oneskillbook panel.
    public void SetIndex(int id)
    {
        myIndex = id;
    }

    //Setting the sprite and name of this particular oneskillbook panel. 
    public void Set(Skillbooks skillbook)
    {
        //Getting the information about the skillbook.
        skillbookItemSlot = skillbook;
        //Showing the sprite of the skillbook. 
        icon.sprite = skillbook.item.icon;
        //Showing the name of the skillbook. 
        skillbookName.text = skillbook.item.Name;
        //Showing the level of the skillbook.
        string temp = ("µÚ" + numWordPairs.GetString(skillbook.currentLevel) + "²ã");
        skillbookLevel.text = temp;
        //Updating the item for the button (for item reference page use).
        button.item = skillbook.item;
    }

    //When the panel is clicked, call the OnSkillbookPanelCLicked() function, which sets a few things up. 
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.GetComponentInParent<SkillbookListPanelController>().OnSkillbookPanelClicked(skillbookItemSlot, myIndex);
        //Also highlight this OneSkillbookPanel.
        Highlight(true);
    }

    //Called by the SkillbookListPanelController's SetMainSkillbook() function. The bool variable allows other panel's stamp to be hidden.
    public void ShowStamp(bool b)
    {
        mainSkillbookStamp.gameObject.SetActive(b);
    }

    //Will be called by the controller when this panel is selected. 
    public void Highlight(bool b)
    {
        highlight.gameObject.SetActive(b);
    }
}

