using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{

	[SerializeField]
	private Animator m_Animator = null;
	[SerializeField]
	private PlayerBounceManager m_PlayerManager = null;

	private bool m_Defending = false;
	private void OnEnable()
	{
		m_PlayerManager.TapToBegin += Begin;
		m_PlayerManager.PlayerBounceCheck += PlayerBounce;
		m_PlayerManager.PlayerLose += End;
		ScoreManager.MultiplierScored += Crit;
	}

	private void OnDisable()
	{
		m_PlayerManager.TapToBegin -= Begin;
		m_PlayerManager.PlayerBounceCheck -= PlayerBounce;
		m_PlayerManager.PlayerLose -= End;
		ScoreManager.MultiplierScored -= Crit;
	}

	private void Begin()
	{
		m_Defending = true;
		m_Animator.SetTrigger("Hit");
	}

	private void PlayerBounce()
	{
		if (m_Defending)
		{
			m_Animator.SetTrigger("Hit");
		}
	}

	private void Retry()
	{
		m_Animator.SetTrigger("Reset");
		m_Defending = false;
	}
	private void End()
	{
		m_Animator.SetTrigger("End");
		m_Defending = false;
	}

	private void Crit()
	{
		m_Animator.SetTrigger("Crit");
	}
}
