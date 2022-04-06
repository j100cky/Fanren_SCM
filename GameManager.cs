using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	private void Awake()
	{
		instance = this;
	}

	public GameObject player;
	public ItemContainer inventoryContainer;
	public SkillbookContainer skillbookContainer;
	public SkillbookContainer skillbookLibrary;
	public DragAndDropController dragAndDropController;
	public SkillDragAndDropController skillDragAndDropController;
	public GameObject topPanel;
	public DayTimeController timeController;
	public DialogSystem dialogSystem;
	public YesNoButtonController yesNoButtonController;
	public Character character;
	public GameObject tradePanel;
	public ItemPageController itemPageController;
	public GameObject craftingPanels;
	public GameObject skillPanel;
	public PlaceableObjectReferenceManager placeableObjects;
	public InteractingObjectContainer interactingObjectContainer;
	public ItemDialoguePanelController itemDialoguePanelController;
	public SkillBarPanel skillBarPanel;
	public GameObject npcQuestPanel;
	public GameObject moDialogPanel;
	public GameObject statusPanel;
	public GameObject petPanel;
	public ItemContainer exportBoxContainer;
	public ItemList itemDB;
	public QuestList questDB;
	public SkillList skillDB;
	public CropsList cropDB;
	public PlaceableObjectContainerList placeableObjectContainers;
	public GameObject skillDescriptionPanel;
	public GameObject npcMultipleQuestPanel;
	public SkillBarPanel_Ver2 skillBarPanelV2;
	public SkillBarPanel_Ver2 skillBarOnScreen;
}
