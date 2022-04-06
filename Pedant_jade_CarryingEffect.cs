using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedant_jade_CarryingEffect : CarryingEffects
{
    //[SerializeField] GameObject prefab;
    [SerializeField] Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>(); //This script is supposed to be attached to the prefabs so animators are usually also attached to itself. 
    }

/*    private void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            OnActive();
        }
    }*/

    public override void OnEquiped(Transform transform) //Instantiate the prefab into the scene. 
    {
        GameObject o = Instantiate(gameObject, transform);
        o.transform.position = transform.position; //Make the object follow the player. 
        //Update the list of prefabs in the StatusPanelController script, which set up the dictionary of <button, prefab> pair. 
        GameManager.instance.statusPanel.GetComponent<StatusPanelController>().UpdateEquippedPrefabs(o); 
    }

    public override void OnActive() //This function changes the animator's isActive parameter so the active animation will be played. 
    {
        anim.SetBool("isActive", true);
    }

/*    public override void OnUnEquipped()
    {
        Destroy(gameObject);
    }*/

    public void OnAnimationEnds() //This function is here to be called by the animator at the last frame.
                                  //Maybe I should place this to the CarryingEffects base script. 
    {
        anim.SetBool("isActive", false);
    }
}
