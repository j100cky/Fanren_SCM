using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Buff Container")]
public class CharBuffContainer : ScriptableObject
{
    public List<BuffData> buffDataList;
}
