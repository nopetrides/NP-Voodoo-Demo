using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritBehaviour : DependantPickup
{
	public override bool IsCollidingWithPlayer()
	{
		if (base.IsCollidingWithPlayer())
		{
			AudioManager.Instance.PlaySFX(m_PickupSFX);
			ScoreManager.Instance.AddMultiplier();
			SetState(false);
		}
		return base.IsCollidingWithPlayer();
	}

}
