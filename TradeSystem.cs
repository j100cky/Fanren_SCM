using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSystem : MonoBehaviour
{
	public bool isTrading; //This is the variable that the inventory buttons will use so that
						   //left- and right- clicking while trading will sell the items. 
	[SerializeField] TradeItemPanel tradeItemPanel;
	ItemContainer tradeItemContainer;

	public void Show(bool v) //The value of v is determined by the TradeInteract script. 
	{
		gameObject.SetActive(v); //Show the panel or hide accordingly. 
		isTrading = v; //Change this variable together with the showing of the panel. 
	}

	public void SetTradeItemContainer(ItemContainer importedContainer)
	{
		tradeItemContainer = importedContainer;
		tradeItemPanel.itemContainer = tradeItemContainer;
		Show(true);
	}





}
