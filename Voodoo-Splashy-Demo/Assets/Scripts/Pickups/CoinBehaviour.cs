using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : Pickup
{
	protected override void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "Player")
		{
			ScoreManager.Instance.AddMoney(1);
		}
		base.OnTriggerEnter(collision);
	}

}
