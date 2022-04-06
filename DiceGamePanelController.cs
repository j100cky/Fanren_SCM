using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceGamePanelController : MonoBehaviour
{
    [SerializeField] GameObject betMessage; //The BetMessage panel to allow the player to make bids. 
    [SerializeField] List<GameObject> dicePanels; //This will contain the three dice panels. 
    [SerializeField] public List<int> combination; //The buttons will use this combination to determine whether they should be highlighted.
    public bool isCalculating; //Used by the buttons to determine whether they should start calculating. 
    [SerializeField] List<CombinationButtonBaseScript> buttonsScripts; //All the combination buttons on the board.
    [SerializeField] List<CombinationButtonBaseScript> betButtons; //All the buttons that got a bet.
    CombinationButtonBaseScript betButton; //A temporary container for the betButton.
    [SerializeField] List<CombinationButtonBaseScript> winButtons; //The buttons that have won. 

    [SerializeField] GameObject betPanel; //Prefab for the placed chips
    [SerializeField] List<GameObject> betPanels; //A list of the prefabs for deletion.

    void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            GamePauseController.instance.isPaused = true;
        }
    }

    public void ShowBetChip(Transform transform, Vector3 position, int betValue) //This function will be called by the BettingButton script.
    {
        GameManager.instance.character.LoseChip(betValue);
        GameObject o = Instantiate(betPanel, transform);
        o.transform.position = position;
        o.GetComponent<PlacedChipPanel>(). GetAmount(betValue);
        o.transform.SetParent(this.transform); //We still have to do this otherwise the prefab will be a children of the BetMessage panel,
                                               //which will be hidden after the bet button is clicked.
        betPanels.Add(o);
        betButton.betValue = betValue; //Give the value from the BetMessage to the betValue in the button's script.
    }

    public void OnPlayButtonClicked()
    {
       
        CalculateComb();
        isCalculating = true;
        DetermineComb();
        CalculateReward();
        Reset();

        
    }

/*    public void OnClicked(Button button)
    {
        Debug.Log(button.name);
    }*/

    public void CallBetMessage() //This function will be called by each button to call the BetMessage panel out. 
    {
        Vector3 position = Input.mousePosition;
        betMessage.SetActive(true);
        betMessage.transform.position = position;
        //Debug.Log(button.name);
        betButton = EventSystem.current.currentSelectedGameObject.
            GetComponent<CombinationButtonBaseScript>(); //This allows the script to obtain which button is being clicked.
                                                          //And I stored the button temporarily to the betButton variable.
                                                          //When an actual bet is made, the AddBetButtonToList() will be called. 
    }

    public void AddBetButtonToList()
    {
        betButtons.Add(betButton);
    }

    public void CalculateComb() //Obtain the combination of die from the DicePanels.
    {
        for (int i = 0; i < dicePanels.Count; i++)
        {
            combination[i] = dicePanels[i].GetComponent<DicePanel>().RollDice(); //Transfer the ith panel's point to the ith element
                                                                                 //in the combination list. 
        }
        Debug.Log(combination[0].ToString()+ combination[1].ToString() + combination[2].ToString());
    }

    void DetermineComb() 
    {
        for(int i = 0; i< buttonsScripts.Count;i++)
        {
            buttonsScripts[i].ToCalculate();
        }
        for(int i = 0; i< buttonsScripts.Count; i++) //After the Tocalculate() function, each button's isWinning variable will have a value. 
                                                       //Those won will be registered to the winButtons list. 
        {
            if(buttonsScripts[i].isWinning == true)
            {
                winButtons.Add(buttonsScripts[i]);
            }
        }
        isCalculating = false;
    }

    public void CalculateReward()
    {
        for(int i = 0; i<betButtons.Count; i++)
        {
            if(winButtons.Contains(betButtons[i]))
            {
                betButtons[i].CalculateReward(); //Call the CalculateReward() function in the button's script. 
                Debug.Log("you won "+betButtons[i].rewardValue.ToString()); //Will make some animation to report this. 
            }
        }
    }

    void Reset()
    {
        winButtons = new List<CombinationButtonBaseScript>(); //Refresh the list for every new game.
        betButtons = new List<CombinationButtonBaseScript>();//Refresh the list for every new game.
        for(int i = 0; i<betPanels.Count; i++)
        {
            Destroy(betPanels[i], 3); //Reset after 5 seconds.
        }
        betPanels = new List<GameObject>();
    }


}
