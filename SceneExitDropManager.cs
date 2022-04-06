using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExitDropManager : MonoBehaviour
{
    public static SceneExitDropManager instance; //For more convenient access from EnemySpawner. 
    //[SerializeField] EnemySpawner enemySpawner;
    [SerializeField] GameObject transitionPrefab; //Referencing the transition portal. Will show this portal to simulate "dropiing".
    [SerializeField] int numOfEnemyLeft; //This will be set by the EnemySpawner script, will be used to determine the chance of dropping. 
    bool isTransitionInstantiated; //When this is true, transition portal will not be shown again. 

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isTransitionInstantiated = false;

    }

    //This function is called by the EnemySpawner to tell how many enemies were instantiated. 
    public void SetEnemyLeft(int value)
    {
        numOfEnemyLeft = value;
        //Sometimes the chances may go and no enemy is spawned. In that case, set the transition portal active at where it is. 
        if (numOfEnemyLeft == 0)
        {
            transitionPrefab.SetActive(true);
            return;
        }
    }

    //This function is called when an enemy dies. 
    public void SpawnTransition(Vector3 position)
    {
        //If a transition portal already exists, don't do anything. 
        if (isTransitionInstantiated == true) { return; }
        //Otherwise, reduce the enemy count by 1 since it is killed. 
        numOfEnemyLeft -= 1;
        //If the enemy was the last enemy, a transition portal will be spawned. 
        if (numOfEnemyLeft == 0)
        {
            ShowTransitionPortal(position);
        }
        //If the enemy is not the last one, the portal will be shown at a chance (1/(number of enemies)). 
        else
        {
            float rand = Random.Range(0f, 1f);
            if (rand < 1f / numOfEnemyLeft)
            {
                ShowTransitionPortal(position);
            }
            else
            {
                return;
            }

        }


       
    }

    private void ShowTransitionPortal(Vector3 position)
    {
        transitionPrefab.SetActive(true);
        transitionPrefab.transform.position = position;
        isTransitionInstantiated = true;
    }
}

