using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightController : TimeAgent

{

    public Light2D globalLight;
    //float intensityValue = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //globalLight = GetComponent<Light>();
        onTimeTick += Tick;
        Init();
        //intensityValue = Mathf.Clamp(intensityValue, 0.3f, 1.0f);
    }

    public void Tick()
    {
        if (globalLight.intensity <= 0.3f)
        {
            return;
        }

        globalLight.intensity -= 0.01f;
    }

}
