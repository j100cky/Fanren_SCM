using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingSkills : MonoBehaviour
{
	public static UsingSkills instance;

	private void Awake()
	{
		instance = this;
	}

	[SerializeField] GameObject hitBySkillEffects; //For referencing the animation object prefab of hit by skills.//
	[SerializeField] GameObject skillEffects; //For referencing the animation object prefab of skill effects.//
	Vector2 motionVector;
	Vector2 lastMotionVector;

	private void Update()
	{
		lastMotionVector = GameManager.instance.player.GetComponent<CharacterController2D>().lastMotionVector;
		motionVector = GameManager.instance.player.GetComponent<CharacterController2D>().motionVector;
	}


	public void summonHitBySkill(Vector3 summonPosition, int hitBySkillEffectID)
	{
		GameObject hitBySkillEffect = Instantiate(hitBySkillEffects, summonPosition, Quaternion.identity); //Instantiate the object prefab on each enemy.//
		hitBySkillEffect.GetComponent<Animator>().SetInteger("hitBySkillEffectID", hitBySkillEffectID); //Set the animator's parameter according to the ID passed from HittingEnemyBaseScript.//
	}

	public void summonSkill(Vector3 summonPosition, int skillID)
	{
		GameObject skillEffect = Instantiate(skillEffects, summonPosition, Quaternion.identity);//Instantiate the object at mouse position defined by the hittingenemybase script.//
		Animator anim = skillEffect.GetComponent<Animator>();
		anim.SetFloat("lastHorizontal", lastMotionVector.x);	//Assign the vectors before skill ID is assigned. 
        anim.SetFloat("lastVertical", lastMotionVector.y);
		anim.SetInteger("skillID", skillID); //Set the parameter ID in the skill prefab.

	}

}
