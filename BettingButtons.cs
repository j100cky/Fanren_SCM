using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingButtons : MonoBehaviour
{
    [SerializeField] Text betValueText;//This is the text component that will show the bet value.
    public int betValue;
    [SerializeField] GameObject placedChip; //This is the panel that shows how many chips are placed on this combination. 
    [SerializeField] DiceGamePanelController diceGamePanel; 

    public void AddBet(int value) //This function will be called individually by each button in the OnClick() function.
                                  //Buttons showing different +value will add different values to the overall betValue variable. 
    {
        betValue += value;
        if(betValue >= 9999) { betValue = 9999; } //For now, the maximum bet value does not exceed 9999.
        betValueText.text = betValue.ToString();
    }

    public void ClearBet() //Whis function will be used by the "0" button.
    {
        betValue = 0;
        betValueText.text = "0";
    }

    public void OnBetClicked() //This will be called by the "Bet" button to hide the message panel.
    {
        gameObject.SetActive(false);
        diceGamePanel.ShowBetChip(transform, transform.position, betValue);
        diceGamePanel.AddBetButtonToList(); //Only when a bet is made will the button goes to the betButtons list in the DiceGamePanelController.
    }
}
