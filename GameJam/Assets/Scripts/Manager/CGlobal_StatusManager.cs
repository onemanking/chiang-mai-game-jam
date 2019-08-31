using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CGlobal_StatusManager : MonoBehaviour
{
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
    public static bool UpgradeCharacter(Transform hCharacter,float fUpdamage,float fDecressAttackDelay)
    {
        if (m_hInstance == null)
            return false;

        return Instance.MainUpgradeCharacter(hCharacter,fUpdamage,fDecressAttackDelay);
    }

    /// <summary>
    /// 
    /// </summary>
    bool MainUpgradeCharacter(Transform hCharacter, float fUpdamage = 1f, float fDecressAttackDelay = 0.1f)
    {
        // Maybe change.
        if (!CheckCanUpgradeCharacter(hCharacter))
            return false;

        var hOfficerBase = hCharacter.GetComponent<OfficerBase>();

        CGlobal_InventoryManager.MoneyDown(GetUpgrageCost(hOfficerBase.Level));

        hOfficerBase.Upgrade(fUpdamage, fDecressAttackDelay);
        
        return true;
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

    #endregion
}
