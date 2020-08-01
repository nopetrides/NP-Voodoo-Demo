using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_SpawnableShields = null;
    [SerializeField]
    private GameObject[] m_SpawnablePickups = null;

    [SerializeField]
    private Transform m_PlayerTransform = null;

    private float m_NewShieldDistance = 8.0f;
    private float m_ShieldGap = 4.0f;
    private int m_MaxSpawnedObject = 30;

    void Start()
    {
        SpawnShield();
        SpawnShield();
        SpawnShield();
        SpawnShield();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnShield(int prefabIndex = -1)
	{
        GameObject shield = Instantiate(m_SpawnableShields[0]) as GameObject;
        shield.transform.SetParent(this.transform);
        shield.transform.position = Vector3.forward * m_NewShieldDistance;
        m_NewShieldDistance += m_ShieldGap;

    }
}
