using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Tool Action/Medicine Use")]
public class Medicine_ItemUse : ToolAction
{
    [SerializeField] List<ItemRecoveryType> recoveryTypes;

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        //Find colliders at the mouse position. If the collider carries a Furnace script, do the CheckItem() function. 
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.01f);
        if (collider != null) //If there is a collider clicked..
        {
            //Check whether the mortar script is present. If yes, run the CheckItem() in the mortar script. 
            if (collider.GetComponent<Mortar>() != null) 
            {
                bool complete = collider.GetComponent<Mortar>().CheckItem(usedItem, inventory);
            }
            else
            {
                //Check for other collider scripts, like NPC or something. 

                //For now, consume the medicine. 
                for (int i = 0; i < recoveryTypes.Count; i++)
                {
                    if (recoveryTypes[i].type == RecoveryType.HP)
                    {
                        GameManager.instance.character.RestoreHealth(recoveryTypes[i].value);
                    }
                    else if (recoveryTypes[i].type == RecoveryType.MP)
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
        else //If mouse right click is not pointing to a mortar, or a NPC or something, consume the item and get the benefit. 
        {
            for (int i = 0; i < recoveryTypes.Count; i++)
            {
                if (recoveryTypes[i].type == RecoveryType.HP)
                {
                    GameManager.instance.character.RestoreHealth(recoveryTypes[i].value);
                }
                else if (recoveryTypes[i].type == RecoveryType.MP)
                {
                    GameManager.instance.character.RestoreMana(recoveryTypes[i].value);
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
