using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

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
	[SerializeField] private FloatReactiveProperty m_WallHp = new FloatReactiveProperty(100f);
	[SerializeField] private int m_BossWaveEvery;

	[Header("Wall")]
	[SerializeField] private Renderer m_WallRenderer;
	[SerializeField] private Material m_FlashMat;

	[Header("UI")]
	[SerializeField] private Image m_HpBar;

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
		// Observeable for wall taken damage
		m_WallHp.Subscribe(x =>
		{
			m_HpBar.fillAmount = m_WallHp.Value / 100;
			if (x <= 0)
			{
				// GAME OVER
				// Application.Quit();
				Debug.LogWarning("GameOver");
			}
		}).AddTo(this);
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

	#region SPAWN
	private void SpawnPrisoner()
	{
		var prisoner = Instantiate(m_PrisonerPrefab, RandomPoint(), Quaternion.identity);
		prisoner.Init(_speed: 2, _dmg: 10, _atkDelay: 1);
	}
	#endregion

	private Vector3 RandomPoint()
	{
		return m_SpawnPoint[Random.Range(0, m_SpawnPoint.Length)].position;
	}

	#region WALL
	public void WallTakeDamage(float _dmg)
	{
		m_WallHp.Value -= _dmg;
		// m_WallRenderer.material = m_FlashMat;
	}
	#endregion
}