using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_ScoreParent = null;

	private bool m_AnimatingScore = false;
	private float m_AnimationTimer = 0.0f;
	private Vector2 m_ScoreOriginalPos;
	private Vector2 m_ScoreNewPos;
	private void OnEnable()
	{
		m_AnimatingScore = true;
		m_ScoreOriginalPos = m_ScoreParent.localPosition;
		m_ScoreNewPos = new Vector2(0,0);
		m_AnimationTimer = 0.0f;
	}

	private void OnDisable()
	{
		m_AnimatingScore = false;
	}

	// Update is called once per frame
	void Update()
    {
        if (m_AnimatingScore)
		{
			m_AnimationTimer += Time.deltaTime;
			Vector2 current = new Vector2(Mathf.SmoothStep(m_ScoreOriginalPos.x, m_ScoreNewPos.x, m_AnimationTimer *2),
				Mathf.SmoothStep(m_ScoreOriginalPos.y, m_ScoreNewPos.y, m_AnimationTimer *2));
			m_ScoreParent.localPosition = current;
			if (m_AnimationTimer > 0.5f)
			{
				m_AnimationTimer = 0.0f;
				m_AnimatingScore = false;
			}
		}

    }
}
