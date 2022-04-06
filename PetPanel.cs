using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPanel : MonoBehaviour
{
    [SerializeField] public List<GameObject> pets; //Include the list of pets as prefabs.
    [SerializeField] public GameObject petPreviewPanel;
    [SerializeField] public PetStatPanel petStatsPanel;
    [SerializeField] public ListOfPetsPanel listOfPetPanel;

    public void ResetPetList()
    {
        pets = new List<GameObject>();
    }

}
