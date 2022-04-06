using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBarScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image timeBarFilling;

    public void SetMaxTime(float totalTime)
    {
        slider.maxValue = totalTime;
        slider.value = totalTime;
        timeBarFilling.color = gradient.Evaluate(1f);
    }

    public void SetTime(float time)
    {
        slider.value = time;
        timeBarFilling.color = gradient.Evaluate(slider.normalizedValue);
    }

}
