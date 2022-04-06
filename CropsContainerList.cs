using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows the list of cropscontainers to be stored as a DB.
[CreateAssetMenu]

public class CropsContainerList : ScriptableObject
{
    public List<CropsContainer> cropsContainerList;
}
