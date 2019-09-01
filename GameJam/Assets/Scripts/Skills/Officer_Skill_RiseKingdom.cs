using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_RiseKingdom", menuName = "Skill/Rise Kingdom")]
public class Officer_Skill_RiseKingdom : Officer_BaseSkill
{
    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] float m_fHeal = 20f;

#pragma warning restore 0649
    #endregion

    public override void UseSkill(Transform hOfficer)
    {
        GameManager.Instance.WallTakeDamage(-m_fHeal);
    }
}
