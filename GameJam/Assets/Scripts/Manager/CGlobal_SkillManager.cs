using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Data

public abstract class abst_Skill : ScriptableObject
{
    public abstract float CooldownTime { get; }

    public abstract void UseSkill();
}

#endregion

public sealed class CGlobal_SkillManager : MonoBehaviour
{
    #region Struct

    struct SkillData
    {
        public abst_Skill m_hSkill;
        public float m_fCooldownTime;
    }

    #endregion

    #region Variable

    #region Variable - Property

    static CGlobal_SkillManager Instance
    {
        get
        {
            if (m_hInstance == null)
                SpawnThisManager();

            return m_hInstance;
        }
    }

    #endregion

    static CGlobal_SkillManager m_hInstance;

    List<Button_SkillController> m_lstButtonSkill = new List<Button_SkillController>();

    Dictionary<int, SkillData> m_dicOfficerSkill = new Dictionary<int, SkillData>();

    Dictionary<int, SkillData> m_dicTempOfficerSkill = new Dictionary<int, SkillData>();


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

    private void Update()
    {
        CooldownUpdate();
    }

    #endregion

    #region Main

    /// <summary>
    /// 
    /// </summary>
    public static void RegisterButtonSkill(Button_SkillController hController)
    {
        Instance?.MainRegisterButtonSkill(hController);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRegisterButtonSkill(Button_SkillController hController)
    {
        if (m_lstButtonSkill.Contains(hController))
            return;

        m_lstButtonSkill.Add(hController);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RegisterOfficerCharacterSkill(int nOfficerID,abst_Skill hSkill)
    {
        Instance?.MainRegisterOfficerCharacterSkill(nOfficerID, hSkill);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRegisterOfficerCharacterSkill(int nOfficerID, abst_Skill hSkill)
    {
        if (m_dicOfficerSkill.ContainsKey(nOfficerID))
        {
            m_dicOfficerSkill[nOfficerID] = new SkillData
            {
                m_hSkill = hSkill,
            };
        }
        else
        {
            m_dicOfficerSkill.Add(nOfficerID, new SkillData
            {
                m_hSkill = hSkill,
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static void UseSkill(int nOfficerID)
    {
        Instance.MainUseSkill(nOfficerID);
    }

    void MainUseSkill(int nOfficerID)
    {
        if (!m_dicOfficerSkill.ContainsKey(nOfficerID))
        {
            Debug.LogError("Don't have this officer ID!");
            return;
        }

        var hOfficerSkill = m_dicOfficerSkill[nOfficerID];

        if (hOfficerSkill.m_hSkill)
        {
            if (hOfficerSkill.m_fCooldownTime > 0)
            {
                Debug.Log("Cooldown");
                return;
            }

            hOfficerSkill.m_hSkill.UseSkill();
            hOfficerSkill.m_fCooldownTime = m_dicOfficerSkill[nOfficerID].m_hSkill.CooldownTime;

            // Rewrite data
            m_dicOfficerSkill[nOfficerID] = hOfficerSkill;

            // Update UI.
            SkillButtonCooldownChange(nOfficerID);
        }
        
    }

    #endregion

    #region Cooldown

    /// <summary>
    /// 
    /// </summary>
    void CooldownUpdate()
    {
        if (m_dicOfficerSkill.Count <= 0)
            return;

        m_dicTempOfficerSkill.Clear();

        foreach(var hOfficerSkillKeyPair in m_dicOfficerSkill)
        {
            var hOfficerSkill = hOfficerSkillKeyPair.Value;
            if (hOfficerSkill.m_hSkill == null || hOfficerSkill.m_fCooldownTime <= 0)
                continue;

            hOfficerSkill.m_fCooldownTime -= Time.deltaTime;

            m_dicTempOfficerSkill.Add(hOfficerSkillKeyPair.Key, hOfficerSkill);
        }

        foreach(var hOfficerSkillKeyPair in m_dicTempOfficerSkill)
        {
            m_dicOfficerSkill[hOfficerSkillKeyPair.Key] = hOfficerSkillKeyPair.Value;

            SkillButtonCooldownChange(hOfficerSkillKeyPair.Key);
        }
    }

    #endregion

    #region Helper

    /// <summary>
    /// Quick test for game jam only.
    /// </summary>
    static void SpawnThisManager()
    {
        var hGO = new GameObject();
        hGO.AddComponent<CGlobal_SkillManager>();
        hGO.name = "Skill Manager";
    }

    /// <summary>
    /// 
    /// </summary>
    void SkillButtonCooldownChange(int nOfficerID)
    {
        if (m_lstButtonSkill.Count <= 0 || !m_dicOfficerSkill.ContainsKey(nOfficerID) || m_dicOfficerSkill[nOfficerID].m_hSkill == null)
            return;

        for(int i = 0; i < m_lstButtonSkill.Count; i++)
        {
            var hButtonSkill = m_lstButtonSkill[i];
            if (hButtonSkill == null || hButtonSkill.SkillOfficerID != nOfficerID)
                continue;

            float fCooldownTimeCount = m_dicOfficerSkill[nOfficerID].m_fCooldownTime;
            float fCooldownTime = m_dicOfficerSkill[nOfficerID].m_hSkill.CooldownTime;

            hButtonSkill.CooldownChange(fCooldownTimeCount,fCooldownTime);

            break;
        }
    }

    #endregion
}
