using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnePetBlockPanel : MonoBehaviour
{
    [SerializeField] Image enemyIcon;
    [SerializeField] Text enemyName;
    [SerializeField] Text enemyLevel;
    [SerializeField] GameObject thisPet;
    [SerializeField] GameObject petPreviewPanel;
    [SerializeField] Image highlight;

    private void Start()
    {
        petPreviewPanel = GameManager.instance.petPanel.GetComponent<PetPanel>().petPreviewPanel;
    }

    public void UpdateInfo(GameObject pet, EnemyDataSheet enemyData) //This function is called at the Start() of the ListOfPetsPanel script
                                                                     //to update the information of each block. 
    {
        thisPet = pet;
        enemyIcon.sprite = enemyData.enemyIcon;
        enemyName.text = enemyData.Name;
        enemyLevel.text = enemyData.enemyLevel.ToString();
    }

    public void UnHighlight() //This will called by the Parend at each click to reset all highlight. 
    {
        highlight.enabled = false;
    }

    public void Highlight() //This will be called by the OnClick() function to highlight the button clicked. 
    {
        GameManager.instance.petPanel.GetComponent<PetPanel>().listOfPetPanel.UnHighlight();
        highlight.enabled = true;
    }

    public void ShowStats() //This will be called by the OnClick() function to print the stats to the stats panel. 
    {
        GameManager.instance.petPanel.GetComponent<PetPanel>().petStatsPanel.Set(thisPet.GetComponent<EnemyController>().enemy);
    }

    public void ShowPreview() //This will be called by the button's OnCLick() function. 
    {
        //GameObject preview = thisPet.GetComponent<PetPrefabPreviewController>().prefabPreview; 
        GameManager.instance.petPanel.GetComponentInChildren<PetPreviewPanelScript>().SetAnimator(thisPet); 
        //Pass the entire pet prefab to the SetAnimator() function so that it can access to the other components of the pet. 
    }
}
