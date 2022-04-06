using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfPetsPanel : MonoBehaviour
{
    [SerializeField] GameObject onePetBlock;
    [SerializeField] List<OnePetBlockPanel> petBlockList; 

    private void Start()
    {
        List<GameObject> pets = new List<GameObject>();
        pets = GetComponentInParent<PetPanel>().pets;
        for(int i = 0; i < pets.Count; i++)
        {
            GameObject go = Instantiate(onePetBlock, transform);
            go.GetComponent<OnePetBlockPanel>().UpdateInfo(pets[i],pets[i].GetComponent<EnemyController>().enemy);
            petBlockList.Add(go.GetComponent<OnePetBlockPanel>());
        }
    }

    public void UnHighlight()
    {
        foreach(OnePetBlockPanel go in petBlockList)
        {
            go.UnHighlight();
        }
    }
}
