using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Skill data")]
public class SkillDatasheet : ScriptableObject
{
    public string skillName;
	public int skillID;
    public string skillDescription;
    public string skillDamageDescriptions1 = "���";
	public string skillDamageDescriptions2 = "���˺�";
	public string skillDamageDescriptions3 = "�˺�)";
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
			elementString = "ľ";
		}
		else if (elementType == SkillbookElementType.fire)
		{
			elementValue = GameManager.instance.character.fire;
			elementString = "��";
		}
		else if (elementType == SkillbookElementType.ground)
		{
			elementValue = GameManager.instance.character.ground;
			elementString = "��";
		}
		else if (elementType == SkillbookElementType.metal)
		{
			elementValue = GameManager.instance.character.metal;
			elementString = "��";
		}
		else if (elementType == SkillbookElementType.water)
		{
			elementValue = GameManager.instance.character.water;
			elementString = "ˮ";
		}

		float totalDamage = Mathf.Round((baseDamage + elementMultiplier * elementValue +
			manaMultiplier * GameManager.instance.character.GetMana()));

		string damageText = skillDamageDescriptions1 + totalDamage.ToString() + skillDamageDescriptions2 +
			"(+" + baseDamage.ToString() + "����" + skillDamageDescriptions3 +
			"(+" + (elementMultiplier * elementValue).ToString() + elementString + "����" + skillDamageDescriptions3 +
			"(+" + (Mathf.Round(manaMultiplier * GameManager.instance.character.GetMana())).ToString() + "����ֵ" + 
			skillDamageDescriptions3;

		return damageText;
	}

}
