using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CGlobal_StatusManager : MonoBehaviour
{
    #region Struct

    struct DebuffSpeedCharacterData
    {
        public PrisonerBase m_hBase;
        public float m_fOriginalSpeed;
    }

    struct DebuffAllSpeedData
    {
        public List<DebuffSpeedCharacterData> m_lstCharacterData;
        public bool m_bDebuffing;
        public float m_fDebuffMultiplier;
        public float m_fDebuffDuration;
    }

    #endregion

    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

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

    DebuffAllSpeedData m_hDebuffAllSpeedData = new DebuffAllSpeedData();

    WaitForEndOfFrame m_wWaitEndFrame = new WaitForEndOfFrame();

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
        
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void DebuffAllPrisonerSpeed(float fDebuffSpeedMultiplier,float fDebuffDuration)
    {
        Instance?.MainDebuffAllPrisonerSpeed(fDebuffSpeedMultiplier, fDebuffDuration);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainDebuffAllPrisonerSpeed(float fDebuffSpeedMultiplier, float fDebuffDuration)
    {
        var arrPrisoner = GameObject.FindGameObjectsWithTag(TagType.Prisoner.String());

        if (arrPrisoner == null || arrPrisoner.Length <= 0)
            return;

        ResetDebuffSpeedToOriginal();

        m_hDebuffAllSpeedData.m_bDebuffing = true;
        m_hDebuffAllSpeedData.m_fDebuffMultiplier = fDebuffSpeedMultiplier;
        m_hDebuffAllSpeedData.m_fDebuffDuration = fDebuffDuration;

        for(int i = 0; i < arrPrisoner.Length; i++)
        {
            var hPrisonerBase = arrPrisoner[i].GetComponent<PrisonerBase>();

            if (hPrisonerBase == null)
                continue;

            if (m_hDebuffAllSpeedData.m_lstCharacterData == null)
                m_hDebuffAllSpeedData.m_lstCharacterData = new List<DebuffSpeedCharacterData>();

            m_hDebuffAllSpeedData.m_lstCharacterData.Add(new DebuffSpeedCharacterData
            {
                m_hBase = hPrisonerBase,
                m_fOriginalSpeed = hPrisonerBase.GetSpeed(),
            });

            hPrisonerBase.SetSpeed(hPrisonerBase.GetSpeed() * fDebuffSpeedMultiplier);
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// 
    /// </summary>
    void DebuffSpeedUpdate()
    {
        if (!m_hDebuffAllSpeedData.m_bDebuffing)
            return;

        m_hDebuffAllSpeedData.m_fDebuffDuration -= Time.deltaTime;

        if (m_hDebuffAllSpeedData.m_fDebuffDuration <= 0)
            ResetDebuffSpeedToOriginal();
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
    void ResetDebuffSpeedToOriginal()
    {
        if (m_hDebuffAllSpeedData.m_lstCharacterData == null || m_hDebuffAllSpeedData.m_lstCharacterData.Count <= 0)
            return;

        for(int i = 0; i < m_hDebuffAllSpeedData.m_lstCharacterData.Count; i++)
        {
            // Reset to original speed.
            m_hDebuffAllSpeedData.m_lstCharacterData[i].m_hBase?.SetSpeed(m_hDebuffAllSpeedData.m_lstCharacterData[i].m_fOriginalSpeed);
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
            m_fOriginalSpeed = hPrisonerBase.GetSpeed()
        });

        hPrisonerBase.SetSpeed(hPrisonerBase.GetSpeed() * m_hDebuffAllSpeedData.m_fDebuffMultiplier);        
    }

    #endregion
}
