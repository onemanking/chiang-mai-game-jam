using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class CGlobal_StatusManager : MonoBehaviour
{
    #region Struct

    struct BuffAttackCharacterData
    {
        public OfficerBase m_hBase;
        public float m_fOriginalDamage;
        public float m_fOriginalAttackDelay;
    }

    struct BuffAllAttackData
    {
        public List<BuffAttackCharacterData> m_lstCharacterData;
        public bool m_bBuffing;
        public float m_fBuffDamageMultiplier;
        public float m_fBuffDecreaseAttackDelay;
        public float m_fBuffDuration;
    }

    struct DebuffSpeedCharacterData
    {
        public PrisonerBase m_hBase;
        public float m_fOriginalAttackDelay;
        public float m_fOriginalSpeed;
    }

    struct DebuffAllSpeedData
    {
        public List<DebuffSpeedCharacterData> m_lstCharacterData;
        public bool m_bDebuffing;
        public float m_fDebuffAttackDelayMultiplier;
        public float m_fDebuffSpeedMultiplier;
        public float m_fDebuffDuration;
    }

    #endregion

    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [Header("Temp")]
    [SerializeField] UI_OfficerLevelController m_hPrefabUIOfficerLevel;

#pragma warning restore 0649
    #endregion

    #region Variable - Property

    static CGlobal_StatusManager Instance
    {
        get
        {
            if (m_hInstance == null)
                SpawnThisManager();

            return m_hInstance;
        }
    }

    #endregion

    static CGlobal_StatusManager m_hInstance;

    BuffAllAttackData m_hBuffAllAttackData = new BuffAllAttackData();

    DebuffAllSpeedData m_hDebuffAllSpeedData = new DebuffAllSpeedData();

    WaitForEndOfFrame m_wWaitEndFrame = new WaitForEndOfFrame();

    Dictionary<Transform, UnityAction<int>> m_dicOnUpgradeCharacter = new Dictionary<Transform, UnityAction<int>>();

    #endregion

    #region Base - Mono

    private void Awake()
    {
        if (m_hInstance == null)
        {
            m_hInstance = this;
        }
        else if (m_hInstance != this)
        {
            Destroy(this);
            return;
        }
    }

    private void OnEnable()
    {
        CGlobal_CharacterManager.AddActionOnCharacterSpawn(OnCharacterSpawn);
    }

    private void OnDisable()
    {
        CGlobal_CharacterManager.RemoveActionOnCharacterSpawn(OnCharacterSpawn);
    }

    private void Update()
    {
        BuffAttackUpdate();
        DebuffSpeedUpdate();  
    }

    #endregion

    #region Main

    /// <summary>
    /// 
    /// </summary>
    public static bool CheckCanUpgradeCharacter(Transform hCharacter)
    {
        if (m_hInstance == null)
            return false;

        return Instance.MainCheckCanUpgradeCharacter(hCharacter);
    }

    /// <summary>
    /// 
    /// </summary>
    bool MainCheckCanUpgradeCharacter(Transform hCharacter)
    {
        if (hCharacter == null)
            return false;

        var hOfficerBase = hCharacter.GetComponent<OfficerBase>();
        if (hOfficerBase == null)
            return false;
        
        int nMoneyCost = GetUpgrageCost(hOfficerBase.Level);

        if (CGlobal_InventoryManager.Money < nMoneyCost)
        {
            Debug.Log("Don't have enough money to upgrade this character.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool UpgradeCharacter(Transform hCharacter)
    {
        if (m_hInstance == null)
            return false;

        return Instance.MainUpgradeCharacter(hCharacter);
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool UpgradeCharacter(Transform hCharacter,float fUpdamage,float fDecreadAttackDelay0)
    {
        if (m_hInstance == null)
            return false;

        return Instance.MainUpgradeCharacter(hCharacter,fUpdamage,fDecreadAttackDelay0);
    }

    /// <summary>
    /// 
    /// </summary>
    bool MainUpgradeCharacter(Transform hCharacter, float fUpdamage = 1f, float fDecreadAttackDelay = 0.1f)
    {
        // Maybe change.
        if (!CheckCanUpgradeCharacter(hCharacter))
            return false;

        var hOfficerBase = hCharacter.GetComponent<OfficerBase>();

        CGlobal_InventoryManager.MoneyDown(GetUpgrageCost(hOfficerBase.Level));

        hOfficerBase.Upgrade(fUpdamage, fDecreadAttackDelay);

        RebuffAttackUpgradeCharacter(hCharacter);

        // Call action on upgrade
        if (m_dicOnUpgradeCharacter.TryGetValue(hCharacter, out var hAction))
            hAction?.Invoke(hOfficerBase.Level);

        var hTemp = hCharacter.GetComponentInChildren<UI_OfficerLevelController>();
        if (hTemp == null && m_hPrefabUIOfficerLevel != null)
        {

            var hUIController = Instantiate(m_hPrefabUIOfficerLevel, Vector3.zero, Quaternion.identity);
            hUIController.Init(hCharacter,hOfficerBase.Level);
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddActionOnUpgradeThisCharacter(Transform hCharacter, UnityAction<int> hAction)
    {
        Instance?.MainAddActionOnUpgradeThisCharacter(hCharacter, hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainAddActionOnUpgradeThisCharacter(Transform hCharacter,UnityAction<int> hAction)
    {
        if (m_dicOnUpgradeCharacter.ContainsKey(hCharacter))
        {
            m_dicOnUpgradeCharacter[hCharacter] += hAction;
        }
        else
        {
            m_dicOnUpgradeCharacter.Add(hCharacter, hAction);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveActionOnUpgradeThisCharacter(Transform hCharacter,UnityAction<int> hAction)
    {
        if (m_hInstance == null)
            return;

        m_hInstance.MainRemoveActionOnUpgradeThisCharacter(hCharacter, hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRemoveActionOnUpgradeThisCharacter(Transform hCharacter, UnityAction<int> hAction)
    {
        if (hCharacter == null)
            return;

        if (!m_dicOnUpgradeCharacter.ContainsKey(hCharacter))
            return;

        m_dicOnUpgradeCharacter[hCharacter] -= hAction;
    }


    /// <summary>
    /// 
    /// </summary>
    public static void BuffAllOfficerAttack(float fBuffDamageMultiplier, float fBuffDecreaseAttackDelay, float fBuffDuration)
    {
        Instance?.MainBuffAllOfficerAttack(fBuffDamageMultiplier,fBuffDecreaseAttackDelay,fBuffDuration);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainBuffAllOfficerAttack(float fBuffDamageMultiplier,float fBuffDecreaseAttackDelay,float fBuffDuration)
    {
        var lstOfficer = CGlobal_CharacterManager.GetCharacterList(TagType.Officer);
        if (lstOfficer == null || lstOfficer.Count <= 0)
            return;

        ResetBuffAllAttackToOriginal();

        m_hBuffAllAttackData.m_bBuffing = true;
        m_hBuffAllAttackData.m_fBuffDamageMultiplier = fBuffDamageMultiplier;
        m_hBuffAllAttackData.m_fBuffDecreaseAttackDelay = fBuffDecreaseAttackDelay;
        m_hBuffAllAttackData.m_fBuffDuration = fBuffDuration;

        for(int i = 0; i < lstOfficer.Count; i++)
        {
            var hOfficerBase = lstOfficer[i].GetComponent<OfficerBase>();
            if (hOfficerBase == null)
                continue;

            if (m_hBuffAllAttackData.m_lstCharacterData == null)
                m_hBuffAllAttackData.m_lstCharacterData = new List<BuffAttackCharacterData>();

            m_hBuffAllAttackData.m_lstCharacterData.Add(new BuffAttackCharacterData
            {
                m_hBase = hOfficerBase,
                m_fOriginalDamage = hOfficerBase.GetDamage(),
                m_fOriginalAttackDelay = hOfficerBase.GetAttackDelay(),

            });

            hOfficerBase.SetDamage(hOfficerBase.GetDamage() * fBuffDamageMultiplier);
            hOfficerBase.SetAttackDelay(hOfficerBase.GetAttackDelay() - fBuffDecreaseAttackDelay);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static void DebuffAllPrisonerSpeed(float fDebuffSpeedMultiplier,float fDebuffDuration, float fDebuffAttackDelayMultiplier)
    {
        Instance?.MainDebuffAllPrisonerSpeed(fDebuffSpeedMultiplier, fDebuffDuration,fDebuffAttackDelayMultiplier);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainDebuffAllPrisonerSpeed(float fDebuffSpeedMultiplier, float fDebuffDuration,float fDebuffAttackDelayMultiplier)
    {
        var lstPrisoner = CGlobal_CharacterManager.GetCharacterList(TagType.Prisoner);

        if (lstPrisoner == null || lstPrisoner.Count <= 0)
            return;

        ResetDebuffAllSpeedToOriginal();

        m_hDebuffAllSpeedData.m_bDebuffing = true;
        m_hDebuffAllSpeedData.m_fDebuffAttackDelayMultiplier = fDebuffAttackDelayMultiplier;
        m_hDebuffAllSpeedData.m_fDebuffSpeedMultiplier = fDebuffSpeedMultiplier;
        m_hDebuffAllSpeedData.m_fDebuffDuration = fDebuffDuration;

        for(int i = 0; i < lstPrisoner.Count; i++)
        {
            var hPrisonerBase = lstPrisoner[i].GetComponent<PrisonerBase>();

            if (hPrisonerBase == null)
                continue;

            if (m_hDebuffAllSpeedData.m_lstCharacterData == null)
                m_hDebuffAllSpeedData.m_lstCharacterData = new List<DebuffSpeedCharacterData>();

            m_hDebuffAllSpeedData.m_lstCharacterData.Add(new DebuffSpeedCharacterData
            {
                m_hBase = hPrisonerBase,
                m_fOriginalAttackDelay = hPrisonerBase.GetAttackDelay(),
                m_fOriginalSpeed = hPrisonerBase.GetSpeed(),
            });

            hPrisonerBase.SetAttackDelay(hPrisonerBase.GetAttackDelay() * fDebuffAttackDelayMultiplier);
            hPrisonerBase.SetSpeed(hPrisonerBase.GetSpeed() * fDebuffSpeedMultiplier);
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// 
    /// </summary>
    void BuffAttackUpdate()
    {
        if (!m_hBuffAllAttackData.m_bBuffing)
            return;

        m_hBuffAllAttackData.m_fBuffDuration -= Time.deltaTime;

        if (m_hBuffAllAttackData.m_fBuffDuration <= 0)
            ResetBuffAllAttackToOriginal();
    }

    /// <summary>
    /// 
    /// </summary>
    void DebuffSpeedUpdate()
    {
        if (!m_hDebuffAllSpeedData.m_bDebuffing)
            return;

        m_hDebuffAllSpeedData.m_fDebuffDuration -= Time.deltaTime;

        if (m_hDebuffAllSpeedData.m_fDebuffDuration <= 0)
            ResetDebuffAllSpeedToOriginal();
    }

    #endregion

    #region Action

    /// <summary>
    /// 
    /// </summary>
    void OnCharacterSpawn(Transform hCharacter)
    {
        StartCoroutine(WaitBeforeCheckDebuffSpeed(hCharacter));
    }

    #endregion

    #region Helper

    /// <summary>
    /// 
    /// </summary>
    static void SpawnThisManager()
    {
        var hGO = new GameObject();
        hGO.AddComponent<CGlobal_StatusManager>();
        hGO.name = "Status Manager";
    }

    /// <summary>
    /// 
    /// </summary>
    int GetUpgrageCost(int nLevel)
    {
        return nLevel * 10;
    }

    /// <summary>
    /// 
    /// </summary>
    void RebuffAttackUpgradeCharacter(Transform hCharacter)
    {
        if (hCharacter == null || !m_hBuffAllAttackData.m_bBuffing)
            return;

        var hOfficerBase = hCharacter.GetComponent<OfficerBase>();
        if (hOfficerBase == null)
            return;

        for(int i = 0; i < m_hBuffAllAttackData.m_lstCharacterData.Count; i++)
        {
            var hData = m_hBuffAllAttackData.m_lstCharacterData[i];
            if (hData.m_hBase == null || hData.m_hBase != hOfficerBase)
                continue;

            hData.m_fOriginalDamage = hOfficerBase.GetDamage();
            hData.m_fOriginalAttackDelay = hOfficerBase.GetAttackDelay();

            hOfficerBase.SetDamage(hOfficerBase.GetDamage() * m_hBuffAllAttackData.m_fBuffDamageMultiplier);
            hOfficerBase.SetDamage(hOfficerBase.GetAttackDelay() - m_hBuffAllAttackData.m_fBuffDecreaseAttackDelay);

            // Rewrite data.
            m_hBuffAllAttackData.m_lstCharacterData[i] = hData;
            break;
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    void ResetBuffAllAttackToOriginal()
    {
        if (m_hBuffAllAttackData.m_lstCharacterData == null || m_hBuffAllAttackData.m_lstCharacterData.Count <= 0)
            return;

        for(int i = 0; i < m_hBuffAllAttackData.m_lstCharacterData.Count; i++)
        {
            var hData = m_hBuffAllAttackData.m_lstCharacterData[i];
            if (hData.m_hBase == null)
                continue;

            // Reset to it original.
            hData.m_hBase.SetDamage(hData.m_fOriginalDamage);
            hData.m_hBase.SetAttackDelay(hData.m_fOriginalAttackDelay);
        }

        m_hBuffAllAttackData.m_bBuffing = false;
        m_hBuffAllAttackData.m_lstCharacterData.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    void ResetDebuffAllSpeedToOriginal()
    {
        if (m_hDebuffAllSpeedData.m_lstCharacterData == null || m_hDebuffAllSpeedData.m_lstCharacterData.Count <= 0)
            return;

        for(int i = 0; i < m_hDebuffAllSpeedData.m_lstCharacterData.Count; i++)
        {
            if (m_hDebuffAllSpeedData.m_lstCharacterData[i].m_hBase == null)
                continue;

            // Reset to original speed.
            m_hDebuffAllSpeedData.m_lstCharacterData[i].m_hBase.SetAttackDelay(m_hDebuffAllSpeedData.m_lstCharacterData[i].m_fOriginalAttackDelay);
            m_hDebuffAllSpeedData.m_lstCharacterData[i].m_hBase.SetSpeed(m_hDebuffAllSpeedData.m_lstCharacterData[i].m_fOriginalSpeed);
        }

        m_hDebuffAllSpeedData.m_bDebuffing = false;
        m_hDebuffAllSpeedData.m_lstCharacterData.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator WaitBeforeCheckDebuffSpeed(Transform hCharacter)
    {
        yield return m_wWaitEndFrame;

        CheckDebuffSpeedOnCharacterSpawn(hCharacter);
    }

    /// <summary>
    /// Check for debuff prisoner that spawn during debuff reduce speed.
    /// </summary>
    void CheckDebuffSpeedOnCharacterSpawn(Transform hCharacter)
    {
        // If not during debuff. Skip it.
        if (!m_hDebuffAllSpeedData.m_bDebuffing)
            return;

        if (hCharacter == null || !hCharacter.CompareTag(TagType.Prisoner.String()))
            return;

        var hPrisonerBase = hCharacter.GetComponent<PrisonerBase>();
        if (hPrisonerBase == null)
            return;

        m_hDebuffAllSpeedData.m_lstCharacterData.Add(new DebuffSpeedCharacterData
        {

            m_hBase = hPrisonerBase,
            m_fOriginalAttackDelay = hPrisonerBase.GetAttackDelay(),
            m_fOriginalSpeed = hPrisonerBase.GetSpeed()
        });

        
        hPrisonerBase.SetAttackDelay(hPrisonerBase.GetAttackDelay() * m_hDebuffAllSpeedData.m_fDebuffAttackDelayMultiplier);
        hPrisonerBase.SetSpeed(hPrisonerBase.GetSpeed() * m_hDebuffAllSpeedData.m_fDebuffSpeedMultiplier);        
    }

    #endregion
}
