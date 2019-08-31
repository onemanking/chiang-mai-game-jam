using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MoneyController : MonoBehaviour
{
    #region Variable

    TextMeshProUGUI m_hText;

    #endregion

    #region Base - Mono

    private void Awake()
    {
        m_hText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CGlobal_InventoryManager.AddActionMoneyChange(MoneyChange);
    }

    private void OnDisable()
    {
        CGlobal_InventoryManager.RemoveActionMoneyChange(MoneyChange);
    }

    #endregion

    /// <summary>
    /// Money text change.
    /// </summary>
    void MoneyChange(int nMoney)
    {
        if (m_hText == null)
            return;

        m_hText.text = nMoney.ToString();
    }
}
