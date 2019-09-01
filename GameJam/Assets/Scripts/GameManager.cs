using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

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

	public enum GameState
	{
		None,
		WaitForWave,
		Playing,
		Pause,
		Over
	}
	public GameState gameState;
	[SerializeField] private Transform[] m_SpawnPoint;
	[SerializeField] private PrisonerBase m_PrisonerPrefab;
	[SerializeField] private FloatReactiveProperty m_WallHp = new FloatReactiveProperty(100f);
	[SerializeField] private int m_BossWaveEvery;
	[SerializeField] private float m_SpawmDelay;

    [SerializeField] private PrisonerBase[] m_PisonerGroupPrefab;

	[Header("Wall")]
	[SerializeField] private Renderer m_WallRenderer;
	[SerializeField] private Material m_FlashMat;
	[SerializeField] private Transform m_ExplosionTransform;

	[Header("UI")]
	[SerializeField] private Image m_HpBar;
    [SerializeField] private TextMeshProUGUI m_WaveText;
    [SerializeField] private float m_WaveTextShowDuration = 1.5f;

    [Header("Wave")]
    [SerializeField] private float m_WaveStartNewRoundDelay = 5f;
    [SerializeField] private int m_WaveStartEnemyNumber = 10;
    [SerializeField] private int m_NextWaveEnemyUpNumber = 2;
    [SerializeField] private CharacterStatus m_NextWaveEnemyUpgrade;

	private float spawnTimer;

    Coroutine flashWallCouroutine;
    Material wallMaterial;

    // For wave
    private bool spawningEnemy;
    private bool waveEnd;
    private int waveCount;
    private int spawnCount;
    private int waveSpawnMaxCount;
    Coroutine nextWaveStartCouroutine;
    private float showWaveTextDuration;

    //Temp
    float startSpeed = 2f;
    float startDamage = 10f;
    float startAtkDelay = 1;

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
				GameOver();
			}
		}).AddTo(this);

        nextWaveStartCouroutine = StartCoroutine(NextWaveStart());
	}

	private void GameOver()
	{
        // Stop couroutin
        if (nextWaveStartCouroutine != null)
        {
            m_WaveText.text = "Game Over";
            StopCoroutine(nextWaveStartCouroutine);
        }

		gameState = GameState.Over;
		var characters = GameObject.FindObjectsOfType<CharacterBase>();
		foreach (var character in characters)
		{
			character.enabled = false;
			if (character.GetType() == typeof(OfficerBase))
			{
				var rb = character.GetComponent<Rigidbody>();
				rb.constraints = RigidbodyConstraints.None;
				rb.AddExplosionForce(500, m_ExplosionTransform.position, 200, 50, ForceMode.Force);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
        if(gameState != GameState.Over)
            WaveUpdate();

		if (gameState != GameState.Playing || !spawningEnemy) return;
		if (Time.time >= spawnTimer)
		{
			spawnTimer = Time.time + m_SpawmDelay;
			SpawnPrisoner();
		}
	}

	#region SPAWN
	private void SpawnPrisoner()
	{
        PrisonerBase hRandomPrisoner = m_PrisonerPrefab;

        if (m_PisonerGroupPrefab != null && m_PisonerGroupPrefab.Length > 0)
        {
            int ranNum = Random.Range(0, m_PisonerGroupPrefab.Length);
            hRandomPrisoner = m_PisonerGroupPrefab[ranNum];
        }

        

		var prisoner = Instantiate(hRandomPrisoner, RandomPoint(), Quaternion.identity);
		
        OnSpawnPrisoner(prisoner);
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

        if (flashWallCouroutine != null)
        {
            FlashWallEnd();
            StopCoroutine(FlashWall());
        }
        flashWallCouroutine = StartCoroutine(FlashWall());
	}

    public void WallHeal(float _heal)
    {
        m_WallHp.Value += _heal;
    }

	private IEnumerator FlashWall()
	{
		wallMaterial = m_WallRenderer.material;
		m_WallRenderer.material = m_FlashMat;
		yield return new WaitForSeconds(0.05f);
        FlashWallEnd();
	}

    private void FlashWallEnd()
    {
        m_WallRenderer.material = wallMaterial;
    }

    #endregion

    #region Wave

    IEnumerator NextWaveStart()
    {
        spawnCount = 0;

        if (gameState != GameState.None)
        {
            yield return new WaitForSeconds(1.5f);

            m_WaveText.text = "Waiting for next wave.";

            yield return new WaitForSeconds(m_WaveStartNewRoundDelay - 3f);

            // Wave set
            waveSpawnMaxCount = m_WaveStartEnemyNumber + waveCount * m_NextWaveEnemyUpNumber;                 
        }
        else
        {
            // First wave set
            waveSpawnMaxCount = m_WaveStartEnemyNumber;
        }

        m_WaveText.text = "Wave : " + (waveCount + 1);

        yield return new WaitForSeconds(1.5f);

        m_WaveText.text = "Wave Start!!";

        yield return new WaitForSeconds(1f);
        

        waveEnd = false;
        spawningEnemy = true;
        gameState = GameState.Playing;

       
        showWaveTextDuration = m_WaveTextShowDuration - 1;
    }

    void WaveUpdate()
    {
        if (showWaveTextDuration > 0)
        {
            showWaveTextDuration -= Time.deltaTime;

            if (showWaveTextDuration <= 0)
                m_WaveText.text = "";
        }

        if(spawnCount >= waveSpawnMaxCount && !waveEnd)
        {
            var lstPrisoner = CGlobal_CharacterManager.GetCharacterList(TagType.Prisoner);
            if(lstPrisoner == null || lstPrisoner.Count <= 0)
                OnWaveEnd();

            spawningEnemy = false;
            return;
        }


    }

    void OnSpawnPrisoner(PrisonerBase prisoner)
    {
        float fSpeed = prisoner.GetSpeed() + waveCount * m_NextWaveEnemyUpgrade.m_fSpeed;
        float fDamage = prisoner.GetDamage() + waveCount * m_NextWaveEnemyUpgrade.m_fDamage;
        float fAttackDelay = prisoner.GetAttackDelay() - waveCount * m_NextWaveEnemyUpgrade.m_fAttackDelay;

        prisoner.Init(_speed: fSpeed, _dmg: fDamage, _atkDelay: fAttackDelay);


        spawnCount++;
    }

    void OnWaveEnd()
    {
        gameState = GameState.Pause;
        waveCount++;
        waveEnd = true;

        m_WaveText.text = "Wave End";

        if (nextWaveStartCouroutine != null)
            StopCoroutine(nextWaveStartCouroutine);

        nextWaveStartCouroutine = StartCoroutine(NextWaveStart());
    }

    #endregion
}