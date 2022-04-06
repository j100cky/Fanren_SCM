using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill List")]
public class SkillList : ScriptableObject
{
    public List<SkillDatasheet> skillDataList;
}
