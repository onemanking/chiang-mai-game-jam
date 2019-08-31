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
    [SerializeField] int m_nOfficerID;

#pragma warning restore 0649
    #endregion

    #endregion

    #region Base - Mono

    private void Awake()
    {
        CGlobal_SkillManager.RegisterOfficerCharacterSkill(transform,m_nOfficerID, m_hSkill);
    }

    #endregion
}
