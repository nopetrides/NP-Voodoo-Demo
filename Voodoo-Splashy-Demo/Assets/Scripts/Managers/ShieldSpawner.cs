using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class ShieldSpawner : MonoBehaviour
{
    [SerializeField]
    private ShieldBehaviour[] m_SpawnableShields = null;
    [SerializeField]
    private PlayerBounceManager m_Player = null;

    [SerializeField]
    private float m_MaxXOffset = 3.0f;

    private float m_NewShieldDistance = -12.0f;
    private float m_CurrentXOffset = 0.0f;
    private float m_ShieldGap = 4.0f;
    private float m_ShieldOutOfViewOffset = -4.0f;
    private int m_MaxSpawnedObjects = 30;
    private int m_LastMovedObject = 0;
    private int m_TheoreticalColliderIndex = 0;
    List<ShieldBehaviour> m_ShieldPool = new List<ShieldBehaviour>();

    private float m_LastResetPos;
    private float dt = 0.0f;

    void Start()
    {
        int SafeShields = 5;
        m_TheoreticalColliderIndex = SafeShields;
        for (int i = 0; i < m_MaxSpawnedObjects; i++)
        {
			if (i <= SafeShields)
			{
				SpawnFirstShields();
			}
			else
			{
                SpawnShield();
			}
        }
        SpawnShield();
        m_LastResetPos = this.transform.position.z;
    }

	private void OnEnable()
	{
        m_Player.PlayerBounceCheck += CheckPlayerCollision;
    }
	private void OnDisable()
	{
        m_Player.PlayerBounceCheck -= CheckPlayerCollision;
    }

	// Update is called once per frame
	void Update()
    {
        if (TimingManager.Instance.TimerRunning)
        {
            Vector3 movement = this.transform.position;
            dt += ((m_ShieldGap / TimingManager.Instance.CurrentGapDuration) * Time.deltaTime);
            movement.z += ((m_ShieldGap / TimingManager.Instance.CurrentGapDuration) * Time.deltaTime) * -1;
            if (dt >= m_ShieldGap)
            {
                m_LastResetPos += m_ShieldGap * -1;
                movement.z = m_LastResetPos;
                dt = 0;
            }
            this.transform.position = movement;
        }
        if (m_ShieldOutOfViewOffset - this.transform.position.z >= 0)
        {
            m_ShieldOutOfViewOffset -= m_ShieldGap;
            MoveAndUpdate();
            m_CurrentXOffset += TimingManager.Instance.GapShrinkPerCycle;
        }
    }
    private void SpawnFirstShields()
    {
        ShieldBehaviour shield = Instantiate(m_SpawnableShields[0]);
        shield.transform.SetParent(this.transform);
        Vector3 pos = Vector3.forward * m_NewShieldDistance;
        shield.transform.position = pos;
        m_NewShieldDistance += m_ShieldGap;
        shield.FirstBlankShield();
        m_ShieldPool.Add(shield);
    }
    private void SpawnShield()
    {
        ShieldBehaviour shield = Instantiate(m_SpawnableShields[0]);
        shield.transform.SetParent(this.transform);
        Vector3 pos = Vector3.forward * m_NewShieldDistance;
        m_CurrentXOffset += Random.Range(m_MaxXOffset * -1, m_MaxXOffset);
        pos.x = m_CurrentXOffset;        
        shield.transform.position = pos;
        m_NewShieldDistance += m_ShieldGap;
        shield.Setup();
        m_ShieldPool.Add(shield);
    }

    private void MoveAndUpdate()
    {
        ShieldBehaviour shield = m_ShieldPool[m_LastMovedObject];
        Vector3 pos = Vector3.forward * m_NewShieldDistance;
        m_CurrentXOffset += Random.Range(m_MaxXOffset * -1, m_MaxXOffset);
        pos.x = m_CurrentXOffset;
        shield.transform.position = pos;
        shield.Setup();
        m_LastMovedObject++;
        if (m_LastMovedObject >= m_ShieldPool.Count)
        {
            m_LastMovedObject = 0;
        }
    }

    private void CheckPlayerCollision()
    {
        bool collided = false;
        if (m_ShieldPool[m_TheoreticalColliderIndex].CheckForPlayerCollisionOnBounce())
        {
            collided = true;
        }
        // Backup in case physics misses a frame due to high speeds / low framerate
        if (!collided && m_Player.transform.position.x < m_ShieldPool[m_TheoreticalColliderIndex].transform.position.x + m_ShieldPool[m_TheoreticalColliderIndex].Collider.size.x/2 &&
            m_Player.transform.position.x > m_ShieldPool[m_TheoreticalColliderIndex].transform.position.x - m_ShieldPool[m_TheoreticalColliderIndex].Collider.size.x / 2)
		{
            collided = true;
        }
		if (collided)
		{
            ScoreManager.Instance.AddScore(1);
		}
		else
        { 
            m_Player.FailedToBounce();
        }
        m_TheoreticalColliderIndex++;
        if (m_TheoreticalColliderIndex >= m_ShieldPool.Count)
		{
            m_TheoreticalColliderIndex = 0;
        }
    }
}
