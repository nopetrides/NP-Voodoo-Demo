using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class UISlidingAnimation : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve m_Curve;
	[SerializeField]
	private RectTransform m_ParentRect;

	private float m_ParentWidth;
	private float m_AnimTimer = 0.0f;
	private bool m_TimerFlip = false;
	private void Start()
	{
		m_ParentWidth = m_ParentRect.rect.width;
	}

	private void Update()
	{
		this.transform.localPosition = new Vector3(m_ParentWidth * m_Curve.Evaluate(m_AnimTimer), this.transform.localPosition.y, this.transform.localPosition.z);
		if (m_TimerFlip)
		{
			m_AnimTimer -= Time.deltaTime;
			if (m_AnimTimer <= 0.0f)
			{
				m_AnimTimer = 0.0f;
				m_TimerFlip = false;
			}
		}
		else
		{
			m_AnimTimer += Time.deltaTime;
			if (m_AnimTimer >= 1.0f)
			{
				m_AnimTimer = 1.0f;
				m_TimerFlip = true;
			}
		}
	}
}
