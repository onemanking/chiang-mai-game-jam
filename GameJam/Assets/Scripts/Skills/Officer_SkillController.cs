using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Officer_SkillController : MonoBehaviour
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] abst_Skill m_hSkill;

    [Header("Test")]
    [SerializeField] int m_nOfficerID = -1;

#pragma warning restore 0649
    #endregion

    #endregion

    #region Base - Mono

    private void Awake()
    {
        if(m_nOfficerID >= 0)
            CGlobal_SkillManager.RegisterOfficerCharacterSkill(transform,m_nOfficerID, m_hSkill);

        // Don't use now.
        // Override stats. (Now can't overlap)
        //StatusOverride(1);
    }

    #endregion

    #region Helper

    /// <summary>
    /// 
    /// </summary>
    void StatusOverride(int nLevel)
    {
        int nID = nLevel - 1;
        if (m_hSkill == null || m_hSkill.StatusData == null || m_hSkill.StatusData.AllStatus == null || m_hSkill.StatusData.AllStatus.Length <= nID)
            return;

        var hStat = m_hSkill.StatusData.AllStatus[nID];

        // Override IT!! (But can't now because class that have stat now can't override.)
        Debug.Log(hStat.m_fMaxHP);
    }

    #endregion
}
