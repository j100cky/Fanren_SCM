using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum RecoveryType
{
    HP,
    MP,
}

[System.Serializable]
public class ItemRecoveryType
{
    public RecoveryType type;
    public float value;
}


[CreateAssetMenu(menuName = "Data/Tool Action/Dan Consumption")]
public class Dan_Consumption : ToolAction
{
    public List<ItemRecoveryType> recoveryTypes;

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        Debug.Log("used");
        for(int i = 0; i < recoveryTypes.Count; i++)
        {
            if(recoveryTypes[i].type == RecoveryType.HP)
            {
                GameManager.instance.character.RestoreHealth(recoveryTypes[i].value);
            }
            else if(recoveryTypes[i].type == RecoveryType.MP)
            {
                GameManager.instance.character.RestoreMana(recoveryTypes[i].value);
            }
            else
            {
                continue;
            }
        }
        inventory.Remove(usedItem);
    }
}
