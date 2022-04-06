using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Recipe_Ingredient
{
    public Item item;
    public int count;
    public int processTime; //The time that it needs to be processed before the next ingredient is displayed. 
}
