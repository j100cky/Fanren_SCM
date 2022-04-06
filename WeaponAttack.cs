using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
	public virtual void Hit()
	{
		
	}

	public virtual bool CanBeHit(List<EnemyType> canBeHit)
	{
		return true;
	}
}