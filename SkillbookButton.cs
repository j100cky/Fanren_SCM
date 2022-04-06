using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillbookButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] Text text;
    [SerializeField] Image highlight;
    public Item item; //This is for the reference of ItemPage.

	int myIndex;

	public void SetIndex(int index) //Used by the panel script to tell which button is clicked. 
	{
		myIndex = index;
	}

	public void Set(ItemSlot slot) //Setting up the icon sprite and text for each slot, when they are assigned.  
	{
		item = slot.item; //passing the information of the item in that slot into the item variable so that the item page can use this item's information. 
		icon.sprite = slot.item.icon;
		icon.gameObject.SetActive(true);

		if (slot.item.isStackable == true)
		{
			text.gameObject.SetActive(true);
			text.text = slot.count.ToString();
		}

		else
		{
			text.gameObject.SetActive(false);
		}
	}

	public void Clean() //This method is to remove the icon and text when the item is used up. 
	{
		icon.sprite = null;
		icon.gameObject.SetActive(false);
		text.gameObject.SetActive(false);
	}

    public void OnPointerEnter(PointerEventData eventData) //Showing the item information page.
    {
        if (icon.sprite == null)
        { return; }
        GameManager.instance.itemPageController.Set(new ItemSlot(item));
    }

    public void OnPointerExit(PointerEventData eventData) //Hiding the item information page.
    {
        GameManager.instance.itemPageController.Clear();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
		gameObject.GetComponentInParent<OneSkillbookPanelScript>().OnPointerClick(eventData);
	}

    public void Highlight(bool b)
	{
		highlight.gameObject.SetActive(b);
	}
}
