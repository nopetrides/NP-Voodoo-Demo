using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The timing manager keeps track of the "beats" increasing in speed overtime and allowing all animations to align.
/// </summary>
public class TimingManager : Singleton<TimingManager>
{
	[SerializeField]
	private float m_BaseGapDuration = 0.5f;
	[SerializeField]
	private float m_MinGapDuration = 0.15f;
	[SerializeField]
	private float m_GapShrinkPerCycle = 0.0001f;
	[SerializeField]
	private PlayerBounceManager m_Player = null;

	public static Action TimerLooped;

	private float m_CurrentGapDuration = 0.25f;
	public float CurrentGapDuration { get { return m_CurrentGapDuration; } }
	public float GapShrinkPerCycle { get { return m_GapShrinkPerCycle; } }
	private float m_GapTimer = 0.5f;
	private bool m_TimerRunning = false;
	public bool TimerRunning { get { return m_TimerRunning;}}

	private void OnEnable()
	{
		m_Player.PlayerLose += End;
	}

	private void OnDisable()
	{
		m_Player.PlayerLose -= End;
	}

	public void Begin()
	{
		m_TimerRunning = true;
		ResetTimer();
	}

	public void Update()
	{
		if (m_TimerRunning)
		{
			m_GapTimer += Time.deltaTime;
			if (m_GapTimer >= m_CurrentGapDuration)
			{
				m_GapTimer = 0.0f;
				if (m_CurrentGapDuration > m_MinGapDuration)
				{
					m_CurrentGapDuration -= m_GapShrinkPerCycle;
				}
				TimerLooped?.Invoke();
				//Debug.Log("Timer Expired.");
			}
		}
	}

	public void End()
	{
		m_TimerRunning = false;
		ResetTimer();
	}

	private void ResetTimer()
	{
		m_CurrentGapDuration = m_BaseGapDuration;
		m_GapTimer = m_CurrentGapDuration / 2;
	}

	public bool IsInFirstHalf()
	{
		return m_GapTimer / m_CurrentGapDuration >= 0.5f;
	}
	public float GetBeatPercent()
	{
		return m_GapTimer / m_CurrentGapDuration;
	}
}
