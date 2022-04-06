using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Tool Action/Shooting object")]
public class ShootObjects : ToolAction
{
    [SerializeField] List<EnemyType> canHitNodesOfType;
    public float damage; //The damage this skill makes.//
	Quaternion rotation;

	public override bool OnApply(Vector2 worldPoint) //hit each enemy individually with individual skill effect//
	{

		//===========Checking for mana level.===============//
		Character character = GameManager.instance.character; //Check for mana level.//
		if (character.currentMana - manaCost < 0)
		{
			Debug.Log("no enough mana");
			//Play some animation//
			return false;
		}

		if (GamePauseController.instance.GetCoolDown() == true)
		//Do not cast spell if it is in cooldown. Cooldown is set to false in the last frame of animation. 
		{
			Debug.Log("The player is doing something else");
			return false;
		}

		//Find out the rotation of the object that is needed with respect to mouse position and player position. 
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		Vector2 playerPos = new Vector2(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y);
		float xDiff = mousePos.x - playerPos.x;
		float yDiff = mousePos.y - playerPos.y;
		Debug.Log(yDiff);
		Debug.Log(xDiff);
		
		//Set the Quaternion's eulerAngles to the rotation calculated. 
		rotation.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan(yDiff/xDiff));
		character.UseMana(manaCost);

		//Set cooldown. 
		GamePauseController.instance.SetCoolDown(true);

		//Instantiate the object using the Quaternion defined. 
		GameObject go = Instantiate(skillPrefab, GameManager.instance.player.transform.position, rotation);
		Debug.Log(rotation.eulerAngles);
		go.GetComponent<SkillController>().SetDamage(damage);
		return true;
	}

}
