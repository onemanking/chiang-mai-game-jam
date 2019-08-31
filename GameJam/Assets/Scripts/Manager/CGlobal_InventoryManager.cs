using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CGlobal_InventoryManager : MonoBehaviour
{
    #region Struct

    struct InventoryData
    {
        public int m_nMoney;
    }

    #endregion

    #region Variable

    #region Variable - Property

    static CGlobal_InventoryManager Instance
    {
        get
        {
            if (m_hInstance == null)
                SpawnThisManager();

            return m_hInstance;
        }
    }

    #endregion

    static CGlobal_InventoryManager m_hInstance;

    // Maybe change to use with database for save data in future.(But in game jam. I think it not happen.)
    InventoryData m_hInventoryData = new InventoryData();

    UnityAction<int> m_actMoneyChange;

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
    public static void MoneyUp(int nMoney)
    {
        if (m_hInstance == null)
            return;

        Instance.MainMoneyUp(nMoney);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainMoneyUp(int nMoney)
    {
        if (nMoney < 0)
            return;

        m_hInventoryData.m_nMoney += nMoney;

        MoneyChangeUpdate();
    }

    /// <summary>
    /// 
    /// </summary>
    public static void MoneyDown(int nMoney)
    {
        if (m_hInstance == null)
            return;

        Instance?.MainMoneyDown(nMoney);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainMoneyDown(int nMoney)
    {
        if (nMoney > 0)
            return;

        m_hInventoryData.m_nMoney -= nMoney;

        MoneyChangeUpdate();
    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddActionMoneyChange(UnityAction<int> hAction)
    {
        Instance?.MainAddActionMoneyChange(hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainAddActionMoneyChange(UnityAction<int> hAction)
    {
        // Call change at first add.
        hAction?.Invoke(m_hInventoryData.m_nMoney);

        m_actMoneyChange += hAction;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveActionMoneyChange(UnityAction<int> hAction)
    {
        if (m_hInstance == null)
            return;

        Instance?.MainRemoveActionMoneyChange(hAction);
    }

    /// <summary>
    /// 
    /// </summary>
    void MainRemoveActionMoneyChange(UnityAction<int> hAction)
    {
        m_actMoneyChange -= hAction;
    }

    #endregion

    #region Helper

    /// <summary>
    /// 
    /// </summary>
    static void SpawnThisManager()
    {
        var hGO = new GameObject();
        hGO.AddComponent<CGlobal_InventoryManager>();
        hGO.name = "Inventory Manager";
    }

    /// <summary>
    /// 
    /// </summary>
    void MoneyChangeUpdate()
    {
        m_actMoneyChange?.Invoke(m_hInventoryData.m_nMoney);
    }

    #endregion

}
