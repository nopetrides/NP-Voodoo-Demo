using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{
	[SerializeField]
	private Transform m_FollowTransform = null;
	[SerializeField]
	private Vector3 m_Offset = new Vector3();
	[SerializeField]
	private float m_SpeedFactor = 0.125f;

	private void OnEnable()
	{
		TimingManager.TimerLooped += IncreaseFollowSpeed;
	}
	private void OnDisable()
	{
		TimingManager.TimerLooped -= IncreaseFollowSpeed;
	}

	private void LateUpdate()
	{
		Vector3 newPos = m_FollowTransform.position + m_Offset;
		newPos.x = Mathf.Lerp(this.transform.position.x, newPos.x, m_SpeedFactor);
		newPos.y = m_Offset.y;
		transform.position = newPos;
	}

	private void IncreaseFollowSpeed()
	{
		m_SpeedFactor += TimingManager.Instance.GapShrinkPerCycle;
	}
}
