using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is for creating a database for each crop so that the save and load system can use ID to determine which crop to save and load. 

[CreateAssetMenu]
public class CropsList : ScriptableObject
{
    public List<Crop> cropsList;  
}
