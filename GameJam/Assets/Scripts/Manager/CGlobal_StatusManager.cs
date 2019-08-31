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

    #endregion
}
