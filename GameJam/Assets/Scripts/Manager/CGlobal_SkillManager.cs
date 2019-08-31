using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#region Data

public abstract class abst_Skill : ScriptableObject
{
    public abstract Sprite SkillSprite { get; }

    public abstract Sprite SkillCutsceneSprite { get; }

    public abstract float CooldownTime { get; }

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
        if (m_dicActSpriteChange.ContainsKey(nOfficerID))
        {
            m_dicActSpriteChange[nOfficerID] += hAction;
        }
        else
        {
            m_dicActSpriteChange.Add(nOfficerID, hAction);
        }


        // Set current sprite
        if (m_dicOfficerSkill.ContainsKey(nOfficerID) && m_dicOfficerSkill[nOfficerID].m_hSkill != null)
        {
            m_dicActSpriteChange[nOfficerID]?.Invoke(m_dicOfficerSkill[nOfficerID].m_hSkill.SkillSprite);
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

            // Update UI.
            SkillButtonCooldownChange(nOfficerID);

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
