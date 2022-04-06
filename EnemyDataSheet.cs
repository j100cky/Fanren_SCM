using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyDataSheet : ScriptableObject
{
    public string Name;
    public int enemyLevel;
    public float HP;
    public float MP;
    public float PATT; //physical attack
    public float MATT; //magical attack
    public float DEF;
    public float enemyMovementSpeed; //The speed that the enemy moves.
    public float enemyAttackRate; //The rate that the enemy attacks.
    public float enemyVisualRange; //The range that the enemy can detect the player.
    public float enemyAttackRange; //The range that the enemy can hit the player (how long its hand is).
    public float enemyPushBackDistance; //For how far does the enemy push the player back. 
    public List<ItemSlot> drops;
    public float EXP; //The amount of EXP that it gives once the enemy is defeated. 
    public Sprite enemyIcon;
    public GameObject animationContainer; //Reference to the GameObject prefab so that the animations can be
                                          //obtained since animator does not seem to be supported by scriptable object. 
    
}
