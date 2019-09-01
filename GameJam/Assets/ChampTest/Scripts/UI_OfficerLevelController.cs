using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_OfficerLevelController : MonoBehaviour
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [Header("Offset")]
    [SerializeField] Vector3 m_vOffset;


    [Header("Setup")]
    [SerializeField] GameObject m_hUIGroup;
    [SerializeField] TextMeshProUGUI m_hText;


#pragma warning restore 0649
    #endregion

    #endregion

    #region Base - Mono

    private void Update()
    {
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.gameState == GameManager.GameState.Over)
            {
                // Temp
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        CGlobal_StatusManager.RemoveActionOnUpgradeThisCharacter(transform.parent, OnUpgradeCharacter);
    }

    #endregion

    #region Main

    public void Init(Transform hOwner, int nLevel)
    {
        if (m_hText)
            m_hText.text = nLevel.ToString();

        transform.SetParent(hOwner);
        CGlobal_StatusManager.AddActionOnUpgradeThisCharacter(hOwner, OnUpgradeCharacter);
        transform.localPosition = Vector3.zero + m_vOffset;


        // Need extension to help this.
        float fRotationToCam = Quaternion.LookRotation(Camera.main.transform.position).y * 60;
        transform.rotation = Quaternion.Euler(0, -fRotationToCam, 0);
    }

    #endregion

    #region Action

    /// <summary>
    /// 
    /// </summary>
    void OnUpgradeCharacter(int nLevel)
    {
        if (m_hText == null)
            return;

        m_hText.text = nLevel.ToString();
    }

    #endregion


}
