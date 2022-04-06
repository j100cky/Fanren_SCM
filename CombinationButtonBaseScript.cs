using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationButtonBaseScript : MonoBehaviour
{
    [SerializeField] public DiceGamePanelController diceGamePanelController;
    public List<int> combination;
    public bool isWinning;
    public int betValue;
    [SerializeField] int multiplier;
    public int rewardValue;

    private void Start()
    {
        diceGamePanelController = GetComponentInParent<DiceGamePanelController>();
        combination = diceGamePanelController.combination;
    }

    void Update()
    {
        if(diceGamePanelController.isCalculating == false) { return; } //Only do the calculation when it is needed.
        ToCalculate(); //The ToCalculate() function will be different for different buttons.
    }

    public virtual void ToCalculate()
    {

    }

    public void Win()
    {
        gameObject.GetComponent<Image>().color = Color.green; //make the button green is it is a hit.
        isWinning = true;
    }

    public void Lose()
    {
        gameObject.GetComponent<Image>().color = Color.white; //Make the button white if it is a miss.
        isWinning = false;
    }

    public virtual void CalculateReward()
    {
        rewardValue = betValue * multiplier;
        GameManager.instance.character.AddChip(rewardValue+betValue); //Add the reward and the original bid to the character's total chip.
    }
}
