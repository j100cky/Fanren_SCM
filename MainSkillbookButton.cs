using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainSkillbookButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] ProgressBar progressBar;
    //[SerializeField] Text text;
    //[SerializeField] Image highlight;
    public Skillbooks mainSkillbookSlot;

    void Start()
    {
        //icon.enabled = false; //Set the Image to inactive otherwise there will be a white blank when nothing is there.
        //text.enabled = false; //Set the Text to inactive otherwise there will be a "9999" when nothing is there.
    }

    public void Set(Skillbooks mainSkillbook)
    {
        mainSkillbookSlot = mainSkillbook;
        icon.enabled = true;
        icon.sprite = mainSkillbook.item.icon;
        progressBar.SetValues(mainSkillbook.levelUpEXPs[mainSkillbook.currentLevel], mainSkillbook.currentEXP);
        //icon.gameObject.SetActive(true);

    }

    public void Clear()
    {
        icon.enabled = false; //Set the Image to inactive otherwise there will be a white blank when nothing is there.
        icon.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (icon.sprite == null)
        { return; }
        GameManager.instance.itemPageController.Set(new ItemSlot(mainSkillbookSlot.item));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.itemPageController.Clear();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.player.GetComponent<InventoryController>().ShowSkillPanel();

    }


}
