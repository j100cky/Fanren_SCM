using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSceneCameraController : MonoBehaviour
{
    float xBound = 12.67f;
    float yBound = 4.96f;
    bool isXReversed = false;
    bool isYReversed = false;

    private void Update()
    {
        MoveCameraX();
        MoveCameraY();
    }

    private void MoveCameraX()
    {
        if(isXReversed == false)
        {
            if (transform.position.x >= xBound) 
            {
                isXReversed = true;
                return;
            }

            transform.position = new Vector3(transform.position.x + 0.0005f, transform.position.y, transform.position.z);
        }
        else
        {
            if (transform.position.x <= (-1) * xBound)
            {
                isXReversed = false;
                return;
            }
            transform.position = new Vector3(transform.position.x - 0.0005f, transform.position.y, transform.position.z);
        }

    }

    private void MoveCameraY()
    {
        if (isYReversed == false)
        {
            if (transform.position.y >= yBound)
            {
                isYReversed = true;
                return;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y+0.0005f, transform.position.z);
        }
        else
        {
            if (transform.position.y <= (-1) * yBound)
            {
                isYReversed = false;
                return;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.0005f, transform.position.z);
        }
    }

}
