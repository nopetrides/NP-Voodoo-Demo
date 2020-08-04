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
    [SerializeField]
    private AudioClip[] m_DeathCrySFX = null;
    [SerializeField]
    private Transform m_ShieldManagerTransform = null;

    public event Action TapToBegin;
    public event Action PlayerLose;
    public event Action PlayerBounceCheck;

    private float m_StartHeight = 0.0f;
    private float m_MaxHeight = 0.0f;
    private Vector3 m_TouchStartPos;
    private Vector3 m_TouchPosition;
    private bool m_Downstroke = false;
    private bool m_IsColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        m_StartHeight = m_SwordBody.position.y+m_BounceHeight;
        m_MaxHeight = m_StartHeight + m_BounceHeight;
    }

    // Update is called once per frame
    void LateUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
		{
            if (GameLoopManager.Instance.State == GameLoopManager.GameState.Main && !TimingManager.Instance.TimerRunning)
            {
                TimingManager.Instance.Begin();
                //Debug.Log("Touch Began");
                TapToBegin?.Invoke();
            }
            m_TouchStartPos = Input.mousePosition;
            m_TouchStartPos.z = transform.localPosition.z;
            m_TouchStartPos = Camera.main.ScreenToWorldPoint(m_TouchStartPos);
        }
        if (Input.GetMouseButton(0))
		{
            m_TouchPosition = Input.mousePosition;
            m_TouchPosition.z = transform.localPosition.z;
            m_TouchPosition = Camera.main.ScreenToWorldPoint(m_TouchPosition);
        }

#else

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (!TimingManager.Instance.TimerRunning)
                {
                    TimingManager.Instance.Begin();
                    //Debug.Log("Touch Began");
                    TapToBegin?.Invoke();
                }
                m_TouchStartPos = touch.position;
                m_TouchStartPos.z = transform.localPosition.z;
                m_TouchStartPos = Camera.main.ScreenToWorldPoint(m_TouchStartPos);
            }

            m_TouchPosition = touch.position;
            m_TouchPosition.z = transform.localPosition.z;
            m_TouchPosition = Camera.main.ScreenToWorldPoint(m_TouchPosition);

        }
#endif
        if (TimingManager.Instance.TimerRunning)
		{
            float Xpos = (m_TouchStartPos.x - m_TouchPosition.x)*-1;
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
                    PlayerBounceCheck?.Invoke();
                    if (m_IsColliding)
                    {
                        //Debug.Log("We live another bounce");
                        AudioManager.Instance.PlaySFX(m_BounceSFX, 0.5f);
                    }
                }
            }
            m_SwordBody.position = new Vector3(Xpos, bounce, m_SwordBody.position.z);
            Vector3 camPos = m_ShieldManagerTransform.position;
            camPos.x = Mathf.SmoothStep(m_ShieldManagerTransform.position.x, Xpos *-1, TimingManager.Instance.GetBeatPercent() *2f);
            m_ShieldManagerTransform.position = camPos;
        }
    }

	private void OnTriggerEnter(Collider collision)
	{
        m_IsColliding = true;
        //Debug.Log("Colision Started");
    }

	private void OnTriggerExit(Collider collision)
	{
        m_IsColliding = false;
        //Debug.Log("Collision Ended");
    }

    public void FailedToBounce()
    {
        AudioManager.Instance.PlaySFX(m_DeathSFX, 0.4f);
        AudioManager.Instance.PlaySFX(m_DeathCrySFX[UnityEngine.Random.Range(0, m_DeathCrySFX.Length)]);
        PlayerLose?.Invoke();
        Debug.Log("Failed with gap of " + TimingManager.Instance.CurrentGapDuration);
    }
}
