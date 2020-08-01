using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerBounceManager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_SwordBody = null;
    [SerializeField]
    private float m_BounceHeight = 1.0f;
    [SerializeField]
    private AudioClip m_BounceSFX = null;
    [SerializeField]
    private AudioClip m_DeathSFX = null;

    public event Action TapToBegin;
    public event Action PlayerLose;

    private bool m_IsBouncing = false;
    private float m_StartHeight = 0.0f;
    private float m_MaxHeight = 0.0f;
    private Vector3 m_TouchPosition;
    private bool m_Downstroke = false;
    private bool m_IsColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        m_IsBouncing = false;
        m_StartHeight = m_SwordBody.position.y+m_BounceHeight;
        m_MaxHeight = m_StartHeight + m_BounceHeight;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
		{
            if (!m_IsBouncing)
            {
                Begin();
                TimingManager.Instance.Begin();
                Debug.Log("Touch Began");
                TapToBegin?.Invoke();
            }
            m_TouchPosition = Input.mousePosition;
            m_TouchPosition.z = transform.localPosition.z;
            m_TouchPosition = Camera.main.ScreenToWorldPoint(m_TouchPosition);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!m_IsBouncing)
            {
                Begin();
                TimingManager.Instance.Begin();
                Debug.Log("Touch Began");
            }

            m_TouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        }
#endif
        if (m_IsBouncing)
		{
            float Xpos = m_TouchPosition.x;
            float bounce = m_SwordBody.position.y;
            if (!TimingManager.Instance.IsInFirstHalf())
            {
                bounce = Mathf.SmoothStep( m_StartHeight,
                    m_StartHeight + (m_BounceHeight * (TimingManager.Instance.GetBeatPercent() * -2f)),
                    TimingManager.Instance.GetBeatPercent() * 2f);
                if (m_Downstroke == false)
                {
                    m_Downstroke = true;
                }
                
            }
            else
			{
                bounce = Mathf.SmoothStep (m_StartHeight, 
                    m_StartHeight + (m_BounceHeight * ((TimingManager.Instance.GetBeatPercent() - 1f) * 2f)) ,
                    TimingManager.Instance.GetBeatPercent() * 2f);
                if (m_Downstroke == true)
                {
                    m_Downstroke = false;
                    if (m_IsColliding)
                    {
                        Debug.Log("We live another bounce");
                        AudioManager.Instance.PlaySFX(m_BounceSFX);
                    }
                    else
                    {
                        Debug.LogWarning("We Ded");
                        AudioManager.Instance.PlaySFX(m_DeathSFX);
                        PlayerLose?.Invoke();
                    }
                }
            }
            m_SwordBody.position = new Vector3(Xpos, bounce, m_SwordBody.position.z);
        }
    }

    public void Begin()
	{
        m_IsBouncing = true;
	}

    public void End()
	{
        m_IsBouncing = false;
	}

	private void OnTriggerEnter(Collider collision)
	{
        m_IsColliding = true;
        Debug.Log("Colision Started");
    }

	private void OnTriggerExit(Collider collision)
	{
        m_IsColliding = false;
        Debug.Log("Collision Ended");
    }
}
