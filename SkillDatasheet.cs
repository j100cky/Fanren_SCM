using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Skill data")]
public class SkillDatasheet : ScriptableObject
{
    public string skillName;
	public int skillID;
    public string skillDescription;
    public string skillDamageDescriptions1 = "造成";
	public string skillDamageDescriptions2 = "总伤害";
	public string skillDamageDescriptions3 = "伤害)";
	public SkillbookElementType elementType;
    public float baseDamage;
    public float elementMultiplier;
    public float manaMultiplier;
    public Sprite skillIcon;
    public ToolAction skillAction;
    public GameObject skillPrefab; //Store the prefab objects into here.

    public string MakeDamageDescription()
    {
		float elementValue = 0f;
		string elementString = "";
		if (elementType == SkillbookElementType.wood)
		{
			elementValue = GameManager.instance.character.wood;
			elementString = "木";
		}
		else if (elementType == SkillbookElementType.fire)
		{
			elementValue = GameManager.instance.character.fire;
			elementString = "火";
		}
		else if (elementType == SkillbookElementType.ground)
		{
			elementValue = GameManager.instance.character.ground;
			elementString = "土";
		}
		else if (elementType == SkillbookElementType.metal)
		{
			elementValue = GameManager.instance.character.metal;
			elementString = "金";
		}
		else if (elementType == SkillbookElementType.water)
		{
			elementValue = GameManager.instance.character.water;
			elementString = "水";
		}

		float totalDamage = Mathf.Round((baseDamage + elementMultiplier * elementValue +
			manaMultiplier * GameManager.instance.character.GetMana()));

		string damageText = skillDamageDescriptions1 + totalDamage.ToString() + skillDamageDescriptions2 +
			"(+" + baseDamage.ToString() + "基础" + skillDamageDescriptions3 +
			"(+" + (elementMultiplier * elementValue).ToString() + elementString + "属性" + skillDamageDescriptions3 +
			"(+" + (Mathf.Round(manaMultiplier * GameManager.instance.character.GetMana())).ToString() + "法力值" + 
			skillDamageDescriptions3;

		return damageText;
	}

}
