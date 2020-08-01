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
	private float m_GapShrinkPerCycle = 0.0001f;

	private float m_CurrentGapDuration = 0.25f;
	private float m_GapTimer = 0.5f;
	private bool m_TimerRunning = false;

	public void Begin()
	{
		m_CurrentGapDuration = m_BaseGapDuration;
		m_TimerRunning = true;
	}

	public void Update()
	{
		if (m_TimerRunning)
		{
			m_GapTimer += Time.deltaTime;
			if (m_GapTimer > m_CurrentGapDuration)
			{
				m_GapTimer = 0.0f;
				m_CurrentGapDuration -= m_GapShrinkPerCycle;
				Debug.Log("Timer Expired.");
			}
		}
	}

	public void End()
	{
		m_TimerRunning = false;
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
