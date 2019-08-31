using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region Data

public abstract class abst_Skill : ScriptableObject
{
    public abstract string SkillName { get; }
    public abstract Sprite SkillSprite { get; }

    public abstract Sprite SkillCutsceneSprite { get; }

    public abstract float CooldownTime { get; }

    // Status
    public abstract abst_Data_CharacterStatus StatusData { get; }

    public abstract void UseSkill(Transform hOfficer);
}

#endregion

public sealed class CGlobal_SkillManager : MonoBehaviour
{
    #region Struct

    struct SkillData
    {
        public Transform m_hOfficer;
        public abst_Skill m_hSkill;
        public float m_fCooldownTime;
    }

    #endregion

    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] UI_SkillCutsceneController m_hSkillCutsceneController;

#pragma warning restore 0649
    #endregion

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


    Dictionary<int, UnityAction<Sprite>> m_dicActSpriteChange = new Dictionary<int, UnityAction<Sprite>>();

    Dictionary<int, UnityAction<float>> m_dicActCooldownChange = new Dictionary<int, UnityAction<float>>();

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
        if (hController == null)
            return;

        if (m_lstButtonSkill.Contains(hController))
            return;

        m_lstButtonSkill.Add(hController);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddActionSpriteChange(int nOfficerID,UnityAction<Sprite> hAction)
    {
        Instance?.MainAddActionSpriteChange(nOfficerID,hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainAddActionSpriteChange(int nOfficerID, UnityAction<Sprite> hAction)
    {
        // Set current sprite
        if (m_dicOfficerSkill.ContainsKey(nOfficerID) && m_dicOfficerSkill[nOfficerID].m_hSkill != null)
        {
            hAction?.Invoke(m_dicOfficerSkill[nOfficerID].m_hSkill.SkillSprite);
        }

        if (m_dicActSpriteChange.ContainsKey(nOfficerID))
        {
            m_dicActSpriteChange[nOfficerID] += hAction;
        }
        else
        {
            m_dicActSpriteChange.Add(nOfficerID, hAction);
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveActionSpriteChange(int nOfficerID, UnityAction<Sprite> hAction)
    {
        if (m_hInstance == null)
            return;

        Instance.MainRemoveActionSpriteChange(nOfficerID, hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRemoveActionSpriteChange(int nOfficerID, UnityAction<Sprite> hAction)
    {
        if (!m_dicActSpriteChange.ContainsKey(nOfficerID))
            return;

        m_dicActSpriteChange[nOfficerID] -= hAction;
    }

    public static void AddActionCooldownChange(int nOfficerID,UnityAction<float> hAction)
    {
        Instance?.MainAddActionCooldownChange(nOfficerID, hAction);
    }

    void MainAddActionCooldownChange(int nOfficerID, UnityAction<float> hAction)
    {
        if (!m_dicActCooldownChange.ContainsKey(nOfficerID))
        {
            m_dicActCooldownChange.Add(nOfficerID, hAction);
        }
        else
        {
            m_dicActCooldownChange[nOfficerID] += hAction;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveActionCooldownChange(int nOfficerID,UnityAction<float> hAction)
    {
        if (m_hInstance == null)
            return;

        Instance?.MainRemoveActionCooldownChange(nOfficerID, hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRemoveActionCooldownChange(int nOfficerID, UnityAction<float> hAction)
    {
        if (!m_dicActCooldownChange.ContainsKey(nOfficerID))
            return;

        m_dicActCooldownChange[nOfficerID] -= hAction;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RegisterOfficerCharacterSkill(Transform hOfficer,int nOfficerID,abst_Skill hSkill)
    {
        Instance?.MainRegisterOfficerCharacterSkill(hOfficer,nOfficerID, hSkill);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRegisterOfficerCharacterSkill(Transform hOfficer, int nOfficerID, abst_Skill hSkill)
    {
        SkillData hData = new SkillData
        {
            m_hOfficer = hOfficer,
            m_hSkill = hSkill,
        };

        if (m_dicOfficerSkill.ContainsKey(nOfficerID))
        {
            m_dicOfficerSkill[nOfficerID] = hData;
        }
        else
        {
            m_dicOfficerSkill.Add(nOfficerID, hData);
        }

        // Change Sprite Skill
        ChangeSpriteSkill(nOfficerID, hSkill.SkillSprite);
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

            hOfficerSkill.m_hSkill.UseSkill(hOfficerSkill.m_hOfficer);
            hOfficerSkill.m_fCooldownTime = m_dicOfficerSkill[nOfficerID].m_hSkill.CooldownTime;

            // Rewrite data
            m_dicOfficerSkill[nOfficerID] = hOfficerSkill;

            if (m_dicActCooldownChange.ContainsKey(nOfficerID))
            {
                m_dicActCooldownChange[nOfficerID]?.Invoke(0);
            }

            m_hSkillCutsceneController?.Show(hOfficerSkill.m_hSkill.SkillCutsceneSprite);
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
            var hValue = hOfficerSkillKeyPair.Value;
            m_dicOfficerSkill[hOfficerSkillKeyPair.Key] = hValue;

            if (hValue.m_hSkill.CooldownTime != 0 && m_dicActCooldownChange.ContainsKey(hOfficerSkillKeyPair.Key))
            {
                float fCalCooldown = 1 - (hValue.m_fCooldownTime / hValue.m_hSkill.CooldownTime);
                m_dicActCooldownChange[hOfficerSkillKeyPair.Key]?.Invoke(fCalCooldown);
            }
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
    void ChangeSpriteSkill(int nOfficerID,Sprite hSprite)
    {
        if (!m_dicActSpriteChange.ContainsKey(nOfficerID))
            return;

        m_dicActSpriteChange[nOfficerID]?.Invoke(hSprite);
    }

    #endregion
}
