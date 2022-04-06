using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTargetsPanel : MonoBehaviour
{
    public TransitionTargetList targetLists; //Save the list of transition targets database here. 
    public List<TransitionTargetButton> buttons; //Save the list of transition target buttons into here. 
    public GameObject transitionButton; //The prefab that is instantiated. 

    private void Start()
    {
        MakeTransitionButtons();
        SetButtonIndex();
        SetButtonScene();
    }

    //Instantiate buttons according to the length of the targetList. 
    private void MakeTransitionButtons()
    {
        for(int i = 0; i<targetLists.pairs.Count; i++)
        {
            GameObject go = Instantiate(transitionButton, transform);
            buttons.Add(go.GetComponent<TransitionTargetButton>());
        }
    }

    //Tells each button what its index is. 
    private void SetButtonIndex()
    {
        for(int i = 0; i< buttons.Count; i++)
        {
            buttons[i].Set(i);
        }
    }

    //Set the scenename and position for each button. 
    private void SetButtonScene()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetTransitionTarget(targetLists.pairs[i]);
        }
    }

    //This is called when a new target is added. 
    public void RefreshButtons()
    {
        RemoveButtons();
        MakeTransitionButtons();
        SetButtonIndex();
        SetButtonScene();
    }

    public void RemoveButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].RemoveButton();
        }
        buttons = new List<TransitionTargetButton>();
    }

}
