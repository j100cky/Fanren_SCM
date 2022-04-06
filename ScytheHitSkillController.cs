using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheHitSkillController : SkillController
{
	TileMapReadController tilemapReadController; //For using the pick up function. 
    Vector3Int gridPosition;
    //To determine what can be harvested. 
    [SerializeField] List<ResourceNodeType> canHitNodesOfType;

    //This is called at the specific frame of the animation. 
    public override void SetTileMapReadController(Vector3Int pos, TileMapReadController controller)
    {
        tilemapReadController = controller;
        gridPosition = pos;
    }

	public override void SetDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        float horizontal = mousePos.x - player.transform.position.x;
        float vertical = mousePos.y - player.transform.position.y;
        //The calculation below is needed otherwise the functions in the animation will be played twice. 
        if(Math.Abs(horizontal) > Math.Abs(vertical))
        {
            vertical = 0f;
        }
        else
        {
            horizontal = 0f;
        }


        anim.SetFloat("lastHorizontal", horizontal);
        anim.SetFloat("lastVertical", vertical);
    }

    public override void PlayHitAnimation()
    {
        //Pick up crops.
        tilemapReadController.cropManager.PickUp(gridPosition);
        //Cut wild grasses. 
        //Find all colliders within the circle.
        Vector3 gridPos3 = new Vector3(gridPosition.x, gridPosition.y, 0f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gridPos3, skillAttackBoxSize);
        foreach (Collider2D c in colliders)
        {
            Debug.Log(c);
            ToolHit hit = c.GetComponent<ToolHit>(); //to detect the components in the script ToolHit. Store the info in the var "hit"

            if (hit != null)    //If the collider has the hit script attached...
            {
                if (hit.CanBeHit(canHitNodesOfType) == true) //If the type of the object can be hit by the tool selected...
                {
                    hit.Hit(damage);
                }

            }
        }
    }
}
