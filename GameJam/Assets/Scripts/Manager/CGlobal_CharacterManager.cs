using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class CGlobal_CharacterManager : MonoBehaviour
{
    #region Struct

    struct CharacterData
    {
        public TagType m_eTagType;
    }

    #endregion

    #region Variable

    #region Variable - Property

    static CGlobal_CharacterManager Instance
    {
        get
        {
            if (m_hInstance == null)
                SpawnThisManager();

            return m_hInstance;
        }
    }

    #endregion

    static CGlobal_CharacterManager m_hInstance;

    Dictionary<Transform, CharacterData> m_dicCharacterData = new Dictionary<Transform, CharacterData>();

    List<Transform> m_lstOfficerCharacter = new List<Transform>();
    List<Transform> m_lstPrisonerCharacter = new List<Transform>();

    UnityAction<Transform> m_actOnCharacterSpawn;

    #endregion

    #region Base - Mono

    private void Awake()
    {
        if(m_hInstance == null)
        {
            m_hInstance = this;
        }
        else if(m_hInstance != this)
        {
            Destroy(this);
            return;
        }
    }

    #endregion

    #region Main

    /// <summary>
    /// 
    /// </summary>
    public static void RegisterCharacter(Transform hCharacter)
    {
        Instance?.MainRegisterCharacter(hCharacter);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRegisterCharacter(Transform hCharacter)
    {
        // Skip if already register character.
        if (m_dicCharacterData.ContainsKey(hCharacter))
            return;

        TagType eTagType = Helper.Tag.GetTagType(hCharacter.tag);

        //
        m_dicCharacterData.Add(hCharacter, new CharacterData
        {
            m_eTagType = eTagType,
        });

        switch (eTagType)
        {
            case TagType.Officer:
                m_lstOfficerCharacter.Add(hCharacter);
                break;

            case TagType.Prisoner:
                m_lstPrisonerCharacter.Add(hCharacter);
                break;
        }
        

        // Call action on character spawn.
        m_actOnCharacterSpawn?.Invoke(hCharacter);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void UnregisterCharacter(Transform hCharacter)
    {
        if (m_hInstance == null)
            return;

        m_hInstance.MainUnregisterCharacter(hCharacter);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainUnregisterCharacter(Transform hCharacter)
    {
        // Skip if don't have this character in data.
        if (!m_dicCharacterData.ContainsKey(hCharacter))
            return;

        switch (m_dicCharacterData[hCharacter].m_eTagType)
        {
            case TagType.Officer:
                m_lstOfficerCharacter.Remove(hCharacter);
                break;

            case TagType.Prisoner:
                m_lstPrisonerCharacter.Remove(hCharacter);
                break;
        }

        m_dicCharacterData.Remove(hCharacter);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddActionOnCharacterSpawn(UnityAction<Transform> hAction)
    {
        Instance?.MainAddActionOnCharacterSpawn(hAction);
    }

    void MainAddActionOnCharacterSpawn(UnityAction<Transform> hAction)
    {
        m_actOnCharacterSpawn += hAction;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveActionOnCharacterSpawn(UnityAction<Transform> hAction)
    {
        if (m_hInstance == null)
            return;

        m_hInstance.MainRemoveActionOnCharacterSpawn(hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRemoveActionOnCharacterSpawn(UnityAction<Transform> hAction)
    {
        m_actOnCharacterSpawn -= hAction;
    }

    /// <summary>
    /// 
    /// </summary>
    public static List<Transform> GetCharacterList(TagType eTagType)
    {
        return Instance?.MainGetCharacterGroup(eTagType);
    }

    /// <summary>
    /// 
    /// </summary>
    List<Transform> MainGetCharacterGroup(TagType eTagType)
    {
        switch (eTagType)
        {
            case TagType.Officer:
                return m_lstOfficerCharacter;

            case TagType.Prisoner:
                return m_lstPrisonerCharacter;
        }

        return null;
    }

    #endregion

    #region Helper

    /// <summary>
    /// 
    /// </summary>
    static void SpawnThisManager()
    {
        var hGO = new GameObject();
        hGO.AddComponent<CGlobal_CharacterManager>();
        hGO.name = "Character Manager";
    }

    #endregion
}
