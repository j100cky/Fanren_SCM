using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script allows the creationg of a database of placeableobjectcontainers. 

[CreateAssetMenu]
public class PlaceableObjectContainerList : ScriptableObject
{
    public List<PlaceableObjectContainer> placeableObjectContainers;
}
