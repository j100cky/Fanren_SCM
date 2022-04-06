using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBreakButtonScript : MonoBehaviour
{
    //When the character's level reaches 13, 16, 19, 22, 25, 28, 31, 34, show the button to allow the player to break boundary. 
    [SerializeField] Character character;

    [SerializeField] GameObject boundaryBreakEvent;

/*    private void Start()
    {
        character = GameManager.instance.player.GetComponent<Character>();
    }*/
    
    //This function is called every time the player levels up. (in the Character script).
    public void ShowButton()
    {
        if(character.playerLevel == 13 && character.GetMana() >= 100f)
        {
            gameObject.SetActive(true);
        }
        else if(character.playerLevel == 16 && character.GetMana() >= 300f)
        {
            gameObject.SetActive(true);
        }
        else if(character.playerLevel == 19 && character.GetMana() >= 900f)
        {
            gameObject.SetActive(true);
        }
        else if(character.playerLevel == 22 && character.GetMana() >= 2700f)
        {
            gameObject.SetActive(true);
        }
        else if(character.playerLevel == 25 && character.GetMana() >= 8100f)
        {
            gameObject.SetActive(true);
        }
        else if(character.playerLevel == 28 && character.GetMana() >= 24300f)
        {
            gameObject.SetActive(true);
        }
        else if(character.playerLevel == 31 && character.GetMana() >= 72900f)
        {
            gameObject.SetActive(true);
        }
        else if(character.playerLevel == 34 && character.GetMana() >= 999999f)
        {
            gameObject.SetActive(true);
        }
    }

    //This will be called when the button is being clicked.
    public void PlayBoundaryBreakTest()
    {
        boundaryBreakEvent.SetActive(true);
    }

}
