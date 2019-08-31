using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class CGlobal_CharacterManager : MonoBehaviour
{
    #region Struct

    struct CharacterData
    {

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

        m_dicCharacterData.Add(hCharacter, new CharacterData());

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
