using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_ItemController : MonoBehaviour
{
    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] int m_nMoneyDrop = 10;

#pragma warning restore 0649
    #endregion

    private void OnDestroy()
    {
        CGlobal_InventoryManager.MoneyUp(m_nMoneyDrop);
    }
}
