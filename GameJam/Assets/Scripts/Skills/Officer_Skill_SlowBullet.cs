using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_SlowBullet", menuName = "Skill/Slow Bullet")]
public class Officer_Skill_SlowBullet : Officer_BaseSkill
{
    #region Variable - Inspector
#pragma warning disable 0649

    [Header("Debuff")]
    [SerializeField] float m_fDebuffSpeedMultiplier = 0.3f;
    [SerializeField] float m_fDebuffSpeedDuration = 5f;

#pragma warning restore 0649
    #endregion

    public override void UseSkill(Transform hOfficer)
    {
        CGlobal_StatusManager.DebuffAllPrisonerSpeed(m_fDebuffSpeedMultiplier, m_fDebuffSpeedDuration);
    }
}
