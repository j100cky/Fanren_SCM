using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to store the pairs matching player level and its Chinese name characters. 

[Serializable] 
public class PlayerLevelPair
{
    public int playerLevel;
    public int playerLevelString;
}

[CreateAssetMenu(menuName = "Player Level Name Pair")]
public class PlayerLevelNamePairs : ScriptableObject
{
    [SerializeField] List<PlayerLevelPair> playerLevelNamePairs;
}
