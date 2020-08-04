using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependantPickup : Pickup
{
	protected bool m_CollidingWithPlayer = false;
	protected override void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "Player")
		{
			m_CollidingWithPlayer = true;
		}
	}
	protected virtual void OnTriggerExit(Collider collision)
	{
		if (collision.tag == "Player")
		{
			m_CollidingWithPlayer = false;
			//Debug.Log("Colision Ended");
		}
	}
	public virtual bool IsCollidingWithPlayer()
	{
		return m_CollidingWithPlayer;
	}
}
