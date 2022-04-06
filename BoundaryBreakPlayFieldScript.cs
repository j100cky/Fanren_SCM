using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBreakPlayFieldScript : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar; //Reference for the progress bar. 
    [SerializeField] GameObject target; //Reference for the flying target that the player controls. 
    [SerializeField] GameObject bullsEye; // Reference for the bullseye that the target needs to be on to get score. 
    [SerializeField] float bullEyeWidth; //For determining the width of the bullsEye. 
    float score = 0; //For updating the progress bar.   
    float increaseSpeed = 1f; //For setting the speed of target movement. 
    float decreaseSpeed = 1f; //For setting the speed of target movement. 
    RectTransform rectTransform; //target RectTransform. 
    RectTransform bullsEyeRectTransform; //BullEye RectTransform.
    float bullsEyeMin; //Used to determine whether score is added or deducted. 
    float bullsEyeMax; //Used to determine whether score is added or deducted. 

    private void Start()
    {
        rectTransform = target.GetComponent<RectTransform>();
        bullsEyeRectTransform = bullsEye.GetComponent<RectTransform>();
        SetBullsEyePos(); //Set the position of the BullsEye. 
    }

    private void Update()
    {
        TargetMovement();
        CalculateScore();
    }

    //This function sets the bullseye position and width, and update the min and max. 
    private void SetBullsEyePos()
    {
        //Create a random number to place the bullseye. 
        float xPos = Random.Range(50f, 900f);
        bullsEyeRectTransform.anchoredPosition = new Vector2(xPos, bullsEyeRectTransform.anchoredPosition.y);
        //Change the width of the bullseye according to the value of bullEyeWidth. 
        bullsEyeRectTransform.sizeDelta = new Vector2(bullEyeWidth, bullsEyeRectTransform.sizeDelta.y);
        //Record the min and max
        bullsEyeMin = (xPos - bullsEyeRectTransform.sizeDelta.x / 2);
        bullsEyeMax = (xPos + bullsEyeRectTransform.sizeDelta.x / 2);
        //Debug.Log("bullseye min and max are "+ bullsEyeMin+","+ bullsEyeMax);
    }

    //This function calculates the score of the game by determining whether the target is within the bullseye. 
    private void CalculateScore()
    {
        
        if(rectTransform.anchoredPosition.x >= bullsEyeMin && rectTransform.anchoredPosition.x <= bullsEyeMax)
        {
            if(score < 60)
            {
                //Add 1% each second since the progress bar max is set to 60. 
                //By multiplying by 6, the total time that the player needs to hold is 10 sec. 
                score = score + Time.deltaTime * 6;
            }
            else
            {
                //Play the success animations.
                GameManager.instance.player.GetComponent<Character>().StageUp();
                transform.parent.gameObject.SetActive(false);
            }

        }
        else
        {
            if(score > 0)
            {
                score = score - Time.deltaTime * 6;
            }
            else
            {
                return;
            }
        }
        progressBar.SetCurrentValue(score);
        //Debug.Log("score is "+ score);
    }

    private void TargetMovement()
    {
        //Debug.Log("target position is " + rectTransform.anchoredPosition);
        if (Input.GetMouseButtonDown(0))
        {
            decreaseSpeed = 1f;
            float xPos = rectTransform.anchoredPosition.x;
            xPos = xPos + 0.5f;
            rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);
        }
        else if (Input.GetMouseButton(0))
        {
            if(rectTransform.anchoredPosition.x < 925f)
            {
                float xPos = rectTransform.anchoredPosition.x;
                increaseSpeed = increaseSpeed * Mathf.Pow(Time.deltaTime + 1f, 2.718f);
                xPos = xPos + increaseSpeed;
                rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);

            }
            else
            {
                return;
            }
        }
        else
        {
            increaseSpeed = 1f; 
            if (rectTransform.anchoredPosition.x > 25f)
            {
                float xPos = rectTransform.anchoredPosition.x;
                decreaseSpeed = decreaseSpeed * Mathf.Pow(Time.deltaTime + 1f, 2.718f);
                xPos = xPos - decreaseSpeed;
                rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);
            }
            else
            { 
                return;
            }
        }        
    }
}
