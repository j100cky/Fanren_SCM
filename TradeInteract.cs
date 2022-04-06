using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeInteract : Interactable
{
  [SerializeField] ItemContainer tradeItemContainer;

    //When right-click on a trader, this will run. 
  	public override void Interact(Character character)
  	{
        //Show the panel. 
        GameManager.instance.tradePanel.GetComponent<TradeSystem>().SetTradeItemContainer(tradeItemContainer);
  		//Pause the game.
        GamePauseController.instance.SetPause(true);
  	}

    //When pressed Esc key, this will run. 
  	public override void StopInteract(Character character)
  	{
        //Hide the trade panel. 
        GameManager.instance.tradePanel.GetComponent<TradeSystem>().Show(false);
        //Resumes game.
        GamePauseController.instance.SetPause(false);
  	}
}
