using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
	[SerializeField]
	private GameObject[] m_ShieldMeshs = null;
	[SerializeField]
	private Pickup[] m_IndependantObjects = null;
	[SerializeField]
	private DependantPickup[] m_DependantObjects = null;
	[SerializeField]
	private GameObject[] m_OffShieldObjects = null;
	[SerializeField]
	private int m_SpawnChance = 2; // 1 in N spawn chance
	[SerializeField]
	private Vector2 m_XRangeOnShieldPickups = new Vector2();
	[SerializeField]
	private BoxCollider m_Collider = null;
	public BoxCollider Collider { get { return m_Collider; } }

	private bool m_IsCollidingWithPlayer;
	private PlayerBounceManager m_PlayerBounce;

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "Player")
		{
			m_IsCollidingWithPlayer = true;
			//Debug.Log("Colision Started");
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.tag == "Player")
		{
			m_IsCollidingWithPlayer = false;
			//Debug.Log("Collision Ended");
		}
	}

	public bool CheckForPlayerCollisionOnBounce()
	{
		if (m_IsCollidingWithPlayer)
		{
			for (int i = 0; i < m_DependantObjects.Length; i++)
			{
				if (m_DependantObjects[i].gameObject.activeSelf)
				{
					if (m_DependantObjects[i] is CritBehaviour)
					{
						m_DependantObjects[i].IsCollidingWithPlayer();
					}
				}
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public void FirstSetup()
	{

	}

	private void ResetAllAttachedObjects()
	{
		if (m_ShieldMeshs != null)
		{
			for (int i = 0; i < m_ShieldMeshs.Length; i++)
			{
				m_ShieldMeshs[i].SetActive(false);
			}
			if (m_ShieldMeshs != null)
			{
				if (m_ShieldMeshs.Length > 0)
				{
					m_ShieldMeshs[Random.Range(0, m_ShieldMeshs.Length)].SetActive(true);
				}
			}
		}
		if (m_IndependantObjects != null)
		{
			for (int i = 0; i < m_IndependantObjects.Length; i++)
			{
				m_IndependantObjects[i].gameObject.SetActive(false);
			}
		}
		if (m_DependantObjects != null)
		{
			for (int i = 0; i < m_DependantObjects.Length; i++)
			{
				m_DependantObjects[i].gameObject.SetActive(false);
			}
		}
		if (m_OffShieldObjects != null)
		{
			for (int i = 0; i < m_OffShieldObjects.Length; i++)
			{
				m_OffShieldObjects[i].SetActive(false);
			}
		}
	}

	public void FirstBlankShield()
	{
		ResetAllAttachedObjects();
	}
	public void Setup()
	{
		ResetAllAttachedObjects();
		if (Random.Range(0, m_SpawnChance) == m_SpawnChance - 1)
		{
			int r =  Random.Range(0, 3);
			if (r == 0)
			{
				if (m_IndependantObjects != null)
				{
					if (m_IndependantObjects.Length > 0)
					{
						int objIndex = Random.Range(0, m_IndependantObjects.Length);
						m_IndependantObjects[objIndex].gameObject.SetActive(true);
						var pos = m_IndependantObjects[objIndex].transform.localPosition;
						pos.x = Random.Range(m_XRangeOnShieldPickups.x, m_XRangeOnShieldPickups.y);
						m_IndependantObjects[objIndex].transform.localPosition = pos;
					}
				}
			}
			else if (r == 1)
			{
				if (m_DependantObjects != null)
				{
					if (m_DependantObjects.Length > 0)
					{
						int objIndex = Random.Range(0, m_DependantObjects.Length);
						m_DependantObjects[objIndex].gameObject.SetActive(true);
						var pos = m_DependantObjects[objIndex].transform.localPosition;
						pos.x = Random.Range(m_XRangeOnShieldPickups.x, m_XRangeOnShieldPickups.y);
						m_DependantObjects[objIndex].transform.localPosition = pos;
					}
				}
			}
			else
			{
				if (m_OffShieldObjects != null)
				{
					if (m_OffShieldObjects.Length > 0)
					{
						int objIndex = Random.Range(0, m_OffShieldObjects.Length);
						m_OffShieldObjects[objIndex].SetActive(true);
						var pos = m_OffShieldObjects[objIndex].transform.localPosition;
						pos.x = Random.value > 0.5f ? pos.x * -1 : pos.x;
						m_OffShieldObjects[objIndex].transform.localPosition = pos;
					}
				}
			}
		}
	}
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, Collider.size);
	}
#endif
}
