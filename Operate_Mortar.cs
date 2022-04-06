using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operate_Mortar : Operatable
{
    Animator anim;
    Mortar mortarScript;
    [SerializeField] Animator containerAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        mortarScript = GetComponent<Mortar>();
    }

    public override void Operate()
    {
        if(mortarScript.isReady == false) 
        {
            Debug.Log("mortar is not ready. ");
            return; 
        }
        if(mortarScript.isContainerReady == false)
        {
            Debug.Log("container is not ready. ");
            return;
        }
        anim.speed = 1;
        containerAnim.speed = 1;
        if(anim.GetBool("start")==false)
        {
            anim.SetBool("start", true);
            containerAnim.SetBool("start", true);
        }
        else
        {
            return;
        }
    }

    public override void StopOperate()
    {
        anim.speed = 0;
        containerAnim.speed = 0;
    }
}
