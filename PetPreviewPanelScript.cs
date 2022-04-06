using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetPreviewPanelScript : MonoBehaviour
{
    [SerializeField] GameObject animatorImage;
    [SerializeField] GameObject summonButton;
    [SerializeField] GameObject revokeButton;
    [SerializeField] GameObject world; //This is providing the transform that the pet will be Instantiated as a child to...
                                       //so that pets don't move along the player.
    GameObject petPrefab; 

    private void Start()
    {
        animatorImage.SetActive(false); //Hide the image which has a white background.
    }

/*    private void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            SummonPet();
        }
    }*/

    public void SetAnimator(GameObject petPrefab) //This function will be called by the the ShowPreview() function in the
                                                  //OnePetBlockPanel script and used by the OnClick() of the button. 
    {
        animatorImage.SetActive(true); //Show the image.
        this.petPrefab = petPrefab; //Set the petPrefab in this script to be the petPrefab that is passed by the script that called this function.
        GameObject petPrefabAnim = petPrefab.GetComponent<PetPrefabPreviewController>().prefabPreview;
        animatorImage.GetComponent<Animator>().runtimeAnimatorController =
            petPrefabAnim.GetComponent<Animator>().runtimeAnimatorController;
    }

    public void SummonPet()
    {
        GameObject pet = Instantiate(petPrefab, world.transform);
        pet.gameObject.tag = "Pet"; //Set the tag to Pet so it does not detect itself as an Enemy and start attacking. 
        pet.transform.position = GameManager.instance.player.transform.position;
        pet.GetComponent<PetController>().isSummoned = true; //Allow the Petcontroller functions to run. 
        pet.GetComponent<EnemyController>().isSummonedAsPet = true; //Stop the EnemyController functions from running.
    }
}
