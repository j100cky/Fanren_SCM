using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeItemPanel : MonoBehaviour
{
    public ItemContainer itemContainer;
	public List<InventoryButton> buttons;
	[SerializeField] InventoryButton button;
	int activeButtonsNumber = 0;

	//This will start running when the panel is set active.
	private void OnEnable()
	{
		SetButtons();
		SetTradePanelIndex();
		Show();
	}

	//This is run when the panel is closed (Esc is pressed).
    private void OnDisable()
    {
		//Hiding all buttons. 
        foreach (InventoryButton button in buttons)
        {
            button.gameObject.SetActive(false);
        }
		//Reset the activeButtonsNumber so that the number will not continuously increase. 
		activeButtonsNumber = 0;


	}

	//Determine how many buttons are to set active, and update the activeButtonsNumber variable. 
    private void SetButtons()
    {
		for (int i = 0; i < itemContainer.slots.Count; i++)
        {
			buttons[i].gameObject.SetActive(true);
			activeButtonsNumber++;
        }
		
    }

	//Accoding to the activeButtonsNumber, set the index for each active button. 
	private void SetTradePanelIndex() 
	{
		for(int i = 0; i < activeButtonsNumber; i++)
		{
			buttons[i].SetTradePanelIndex(i);
		}

	}

	//According to the activeButtonsNumber, set the icon and count of each active button.
	private void Show()
	{
		for(int i = 0; i < activeButtonsNumber; i++)
		{
			if (itemContainer.slots[i].item == null)
			{
				buttons[i].Clean();
			}
			else
			{
				buttons[i].Set(itemContainer.slots[i]);
			}
		}
	}

	public virtual void OnTradePanelLeftClick(int id) //Left click will buy one item.
	{
		if(GameManager.instance.character.GetCashValue() < itemContainer.slots[id].item.purchasePrice)
        {
			Debug.Log("Not enough money");
			return;
        }
		GameManager.instance.dragAndDropController.OnTradePanelLeftClick(itemContainer.slots[id]);
		Show();
		//Purchase price deduction is performed in the DragAndDropController script. 
	}

	public virtual void OnTradePanelRightClick(int id)
	{
		GameManager.instance.dragAndDropController.OnTradePanelRightClick(itemContainer.slots[id]);
		Show();
	}
}
