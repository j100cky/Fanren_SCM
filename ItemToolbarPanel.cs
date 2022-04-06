using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolbarPanel : ItemPanel
{
    [SerializeField] ToolbarController toolbarController;

    private void Start()
    {
    	Init();
    	toolbarController.onChange += Highlight;
    	Highlight(0);
    }

    private void LateUpdate() //Ensures that when items are added to the first roll of the bag, the toolbar will get updated automatically. 
    {
        Show();
    }

    public override void OnLeftClick(int id) //When left-clicking the tool on the toolbar, hightlight the tool as well.
    {
    	toolbarController.Set(id);
    	Highlight(id);
    }

    int currentSelectedTool;
    public void Highlight(int id)
    {
    	buttons[currentSelectedTool].Highlight(false);
    	currentSelectedTool = id;
    	buttons[currentSelectedTool].Highlight(true);
    }

    public override void Show()
    {
        base.Show();    //Keep the original codes for the Show() funciton in ItemPanel
        toolbarController.UpdateHighlightIcon(); //Update the high light so that when an item is used up, the highlight will be gone too. 
    }
}
