using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialog/Dialogue")]
public class DialogContainer : ScriptableObject
{
	public List<string> line;
	public Actor actor;
	public ItemContainer tradePanel; //If the NPC sells something, attach the item container here. 
									
	public NPCQuestController npcQuestController;  //If the NPC has a quest controller, attach the npcQuestControler here. 
	public GameObject eventAfterInteraction; //The event that will happen after the interaction. This will be called by the Conclude() function 
											//...in the DialogSystem script. 

	public void MakeDialogContainer(List<string> input, Actor a = null, ItemContainer tradeItems = null, NPCQuestController controller = null, 
		GameObject eventAfter = null)
    {
		line = input;
		actor = a;
		tradePanel = tradeItems;
		npcQuestController = controller;
		eventAfterInteraction = eventAfter;
    }

}
