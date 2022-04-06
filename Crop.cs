using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Crop")]
public class Crop : ScriptableObject
{
    public int cropID; //Used for saving and Loading
    public int timeToGrow = 10;
    public Item yield; //what will grow out in the end.
    public int count = 1; //how many is grown.
    public float giveForagingEXP; //How much exp is given for this seed when the crop is harvested.

    public List<Sprite> sprites; //this will contain the sprite of crop at different stages.
    public List<int> growthStageTime; //This will represent at which time will the crop transit into the next sprite. 
}
