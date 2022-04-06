using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHit : MonoBehaviour
{

	public virtual void Hit(float damage)
	{
		
	}

	public virtual bool CanBeHit(List<ResourceNodeType> canBeHit)
	{
		return true;
	}

	public virtual void ReleaseLoot()
	{
		
	}
}
