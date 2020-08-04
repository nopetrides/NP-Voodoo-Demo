using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	[SerializeField]
	private Animator m_Animator;
	[SerializeField]
	private PlayerBounceManager m_PlayerManager;

	private bool m_Attacking = false;
	private void OnEnable()
	{
		m_PlayerManager.PlayerLose += PlayerDied;
		m_PlayerManager.TapToBegin += Begin;
		m_PlayerManager.PlayerBounceCheck += PlayerBounce;
	}

	private void OnDisable()
	{
		m_PlayerManager.PlayerLose -= PlayerDied;
		m_PlayerManager.TapToBegin -= Begin;
		m_PlayerManager.PlayerBounceCheck -= PlayerBounce;
	}

	private void Begin()
	{
		m_Attacking = true;
		m_Animator.SetTrigger("Hit");
	}

	private void PlayerBounce()
	{
		if (m_Attacking)
		{
			m_Animator.SetTrigger("Hit");
		}
	}

	private void PlayerDied()
	{
		m_Attacking = false;
		m_Animator.SetTrigger("Died");
	}
}
