using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;

	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				var go = new GameObject();
				go.AddComponent<GameManager>();
				go.name = "GameManager";
			}
			return _instance;
		}
	}
	[SerializeField] private Transform[] m_SpawnPoint;
	[SerializeField] private PrisonerBase m_PrisonerPrefab;
	[SerializeField] private float m_TotalHp;
	[SerializeField] private int m_BossWaveEvery;

	private float spawnTimer;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(this);
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time >= spawnTimer)
		{
			spawnTimer = Time.time + 2;
			SpawnPrisoner();
		}
	}

	private void SpawnPrisoner()
	{
		Instantiate(m_PrisonerPrefab, RandomPoint(), Quaternion.identity);
	}

	private Vector3 RandomPoint()
	{
		return m_SpawnPoint[Random.Range(0, m_SpawnPoint.Length)].position;
	}

	#region WALL
	public void WallTakeDamage(float _dmg)
	{
		m_TotalHp -= _dmg;
		// TODO
		if (m_TotalHp <= 0)
		{
			// GAME OVER
		}
	}
	#endregion
}