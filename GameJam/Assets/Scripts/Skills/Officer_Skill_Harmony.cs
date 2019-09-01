using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Harmony", menuName = "Skill/Harmony")]
public class Officer_Skill_Harmony : Officer_BaseSkill
{
    #region Variable - Inspector
#pragma warning disable 0649

    [Header("Buff")]
    [SerializeField] float m_fDamageMultiplier = 1.3f;
    [SerializeField] float m_fDecreaseAttackDelay = 0.3f;
    [SerializeField] float m_fDuration = 4f;

    [Header("Paricle")]
    [SerializeField] ParticleSystem m_hParticle;
    [SerializeField] Vector3 m_vOffset;

#pragma warning restore 0649
    #endregion

    public override void UseSkill(Transform hOfficer)
    {
        CGlobal_StatusManager.BuffAllOfficerAttack(m_fDamageMultiplier, m_fDecreaseAttackDelay, m_fDuration);

        if (m_hParticle)
        {
            var hParticle = Instantiate(m_hParticle, hOfficer.position + m_vOffset, Quaternion.Euler(-90, 0, 0));
            Destroy(hParticle.gameObject, m_fDuration);
        }
    }
}
