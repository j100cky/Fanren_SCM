using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AuctionHostDialogPanelScript : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] Text nameText;
    [SerializeField] Text dialogText;

    [SerializeField] AuctionBidController bidController; //This script updates the bid prices of the item

    [SerializeField] AuctionSceneManager manager; //This script is needed to deliver information about the current item for bidding. 

    [SerializeField] float visibleTextPercent;
    [Range(0f, 1f)]

    [SerializeField] GameObject yesNoPrompt;

    List<string> lines; //A temporary container of strings that can be modified in different functions. 
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;
    string lineToShow;
    int currentTextLine;
    bool hasPrompt = false; //If this is true, the script will show a yes/no prompt. 

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PushText();
        }
        TypeOutText();
    }

    private void TypeOutText()
    {
        if(visibleTextPercent >= 1f) { return; } //If the line is finished, do not keep runing. 
        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType; //Calculate the visibleTextPercent according to how much time is used. 
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0, 1f); //Make sure the visibleTextPercent is between 0 to 1. 
        UpdateText();
    }

    private void UpdateText() //This function updates the showing of text according to visibleTextPercent. 
    {
        int letterCount = (int)(lineToShow.Length * visibleTextPercent); //Each update, letterCount will increase as currentTime increases. 
        dialogText.text = lineToShow.Substring(0, letterCount); //Showing only from the 0th char to the letterCount-th char. 

    }

    private void PushText()
    {
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        if (currentTextLine >= lines.Count)
        {
            Conclude();
        }
        else
        {
            CycleLine();
        }
    }

    public void Initialize(Item item)
    {
        GamePauseController.instance.isPaused = true;
        gameObject.SetActive(true);
        MakeDialog(item); 
        currentTextLine = 0;
        CycleLine();
    }

    private void CycleLine()
    {
        lineToShow = lines[currentTextLine];
        totalTimeToType = lineToShow.Length * timePerLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;
        dialogText.text = "";
        currentTextLine += 1;
    }

    private void MakeDialog(Item itemToSell) //Make a new list of strings according to the different item being sold. 
    {
        lines = new List<string>();
        lines.Add("������Ҫ���ĵ��� " + itemToSell.Name);
        lines.Add("�׼�Ϊ" + itemToSell.purchasePrice.ToString() + "��") ;
        lines.Add("ÿ�μӼ�"+((int)(itemToSell.purchasePrice/10)).ToString() + "��");
    }

    private void Conclude()
    {
        if(hasPrompt == true)
        {
            ShowYesNoPrompt();
        }
        else
        {
            gameObject.SetActive(false); //Hide the speech panel. 
            GamePauseController.instance.isPaused = false; //Resume the game after interaction
            manager.Conclude(); //Call the Conclude() function in the manager script, where different functions
                                //are called depending on the auctionStatus variable value. 
        }

    }

    public void AnnounceWinner(Item item, int currentPrice, string bidderName)
    {
        GamePauseController.instance.isPaused = true;
        gameObject.SetActive(true);
        lines = new List<string>();
        lines.Add(item.Name + "��" + currentPrice + "�ļ۸�������" + bidderName + "��" );
        lines.Add("��ϲ�������������������������ϸ�������������");
        currentTextLine = 0;
        //manager.AwardWinner();
        CycleLine();
    }

    public void ConcludeAuctionSpeech()
    {
        GamePauseController.instance.isPaused = true;
        lines = new List<string>();
        lines.Add("����������ˡ���л��λ���ѵ��͹⡣");
        lines.Add("ף��λ��Ϊ�������ϡ�");
        gameObject.SetActive(true);
        currentTextLine = 0;
        CycleLine();
    }

    public void ExitPrompt()
    {
        GamePauseController.instance.isPaused = true;
        lines = new List<string>();
        lines.Add("��ϣ���뿪��������");
        gameObject.SetActive(true);
        currentTextLine = 0;
        hasPrompt = true; //Set this true so the Conclude() function will run differently. 
        CycleLine();
    }

    private void ShowYesNoPrompt()
    {
        yesNoPrompt.SetActive(true);
    }

    public void YesButtonSelected()
    {
        GamePauseController.instance.isPaused = true;
        gameObject.SetActive(false);
        yesNoPrompt.SetActive(false);
        Debug.Log("Auction ended");
        //Transsit the player out of the room.
    }

    public void NoButtonSelected()
    {
        GamePauseController.instance.isPaused = false;
        gameObject.SetActive(false);
        yesNoPrompt.SetActive(false);
    }

    
}
