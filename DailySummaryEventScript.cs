using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DailySummaryEventScript : MonoBehaviour
{
    [SerializeField] Text totalValueText;//Output of the total value.
    int totalValue;
    [SerializeField] ItemContainer exportBoxContainer;//Used to access the items in the export box. 
    [SerializeField] GameObject oneItemSummaryPanelPrefab;//The item panel prefab is used for showing what item is sold. 
    [SerializeField] Transform panel; //This is used for the instantiation of the one
    [SerializeField] GameObject nextDayButton; //For showing the button to proceed to the next day. 
    string currentScene;
    [SerializeField] GameObject canvas; //For hiding the canvas during screen untint. 

    //Calculate the sum of item values in the export box. 

    private void Start()
    {
        canvas.SetActive(false); //For some reason the canvas does not tint with the screen. So I have to hide it first. 
        GamePauseController.instance.SetEvent(true); //Pause the game. 
        GameManager.instance.player.SetActive(false); //Hide the player because I don't want the player in the summary scene. 
        //Start the summary process. 
        StartCoroutine(BeginSummary()); 
    }


    private IEnumerator BeginSummary()
    {
        yield return new WaitForSeconds(1.5f);
        canvas.SetActive(true); //Set the canvas active for icons to show. 
        StartCoroutine(CalculateTotalValue()); //Start calculating the total values and print on the text panel. 
        StartCoroutine(ShowItemSlots()); //Show all the loaded items on the panel. 
    }

    public IEnumerator ShowItemSlots()
    {
        for (int i = 0; i < exportBoxContainer.slots.Count; i++)
        {
            if (exportBoxContainer.slots[i].item != null)
            {
                GameObject go = Instantiate(oneItemSummaryPanelPrefab, panel);
                //Use the Set() function in the OneItemSummaryPanel prefab. 
                go.GetComponent<OneItemSummaryPanelController>().Set(exportBoxContainer.slots[i]);
                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(1f);
        //After all information is shown, show the next day button so the player can proceed. 
        nextDayButton.SetActive(true);

    }

    //Calculate the total value of items in the export box. 
    public IEnumerator CalculateTotalValue()
    {
        for (int i = 0; i < exportBoxContainer.slots.Count; i++)
        {
            if (exportBoxContainer.slots[i].item != null)
            {
                totalValue += exportBoxContainer.slots[i].item.sellingPrice * exportBoxContainer.slots[i].count;
                totalValueText.text = totalValue.ToString();
                yield return new WaitForSeconds(0.2f);
            }
        }
        
        
    }

    //The nextDayButton will run this function to proceed to the next day. 
    public void EndEvent()
    {
        ClearContainer();
        GameManager.instance.character.AddMoney(totalValue);
        canvas.SetActive(false);
        GamePauseController.instance.SetEvent(false);
        //Switch scene, and make the player wake up on his bed. 
        GameSceneManager.instance.InitSwitchScene("Hanli's Room", new Vector3(5.7f, -1.5f, 0f));
    }


    //Clearing the export box. 
    public void ClearContainer()
    {
        for(int i = 0; i < exportBoxContainer.slots.Count; i++)
        {
            if(exportBoxContainer.slots[i].item != null)
            {
                exportBoxContainer.slots[i].Clear();
            }
        }
    }


}
