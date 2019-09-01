using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_SlowBullet", menuName = "Skill/Slow Bullet")]
public class Officer_Skill_SlowBullet : Officer_BaseSkill
{
    #region Variable - Inspector
#pragma warning disable 0649

    [Header("Debuff Attack")]
    [SerializeField] float m_fDebuffAttackSpeedMultiplier = 2f;

    [Header("Debuff Speed")]
    [SerializeField] float m_fDebuffSpeedMultiplier = 0.3f;
    [SerializeField] float m_fDebuffSpeedDuration = 5f;

    [Header("Particle")]
    [SerializeField] ParticleSystem m_hParticle;
    [SerializeField] Vector3 m_vOffset;

#pragma warning restore 0649
    #endregion

    public override void UseSkill(Transform hOfficer)
    {
        CGlobal_StatusManager.DebuffAllPrisonerSpeed(m_fDebuffSpeedMultiplier, m_fDebuffSpeedDuration,m_fDebuffAttackSpeedMultiplier);


        if (m_hParticle)
        {
            var hParticle = Instantiate(m_hParticle, hOfficer.position + m_vOffset, Quaternion.Euler(-90,0,0));
            Destroy(hParticle, m_fDebuffSpeedDuration);
        }
    }
}
