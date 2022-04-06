using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZDepth : MonoBehaviour
{
    Transform t;

    //For moving object, manually set this to false. Otherwise the z dpeth of the moving object will not change. 
    [SerializeField] bool stationary = true; 

    void Start()
    {
        t = transform;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = pos.y * 0.0001f;
        transform.position = pos;

        if(stationary == true) { Destroy(this); }
    }
}
