using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public float maximum;
    public float current;
    public Image mask;
    public Image fill;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount;
        if(current == 0f)
        {
            fillAmount = 0f;
        }
        else
        {
            fillAmount = current / maximum;
        }
        mask.fillAmount = fillAmount;
        fill.color = color;
    }

    public void SetValues(float max, float curr)
    {
        maximum = max;
        current = curr;
    }

    public void SetCurrentValue(float input)
    {
        current = input;
    }

    public void SetMaxValue(float input)
    {
        maximum = input;
    }
}
