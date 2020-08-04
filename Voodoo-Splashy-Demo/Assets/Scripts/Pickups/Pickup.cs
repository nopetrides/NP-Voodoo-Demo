using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	[SerializeField]
	protected AudioClip m_PickupSFX = null;
	protected void SetState(bool active)
	{
		this.gameObject.SetActive(active);
	}

	protected virtual void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "Player")
		{
			AudioManager.Instance.PlaySFX(m_PickupSFX);
			SetState(false);
			//Debug.Log("Colision Started");
		}
	}
}
