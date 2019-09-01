using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_RiseKingdom", menuName = "Skill/Rise Kingdom")]
public class Officer_Skill_RiseKingdom : Officer_BaseSkill
{
    #region Variable - Inspector
#pragma warning disable 0649

    [Header("Heal")]
    [SerializeField] float m_fHeal = 20f;

    [Header("Particle")]
    [SerializeField] ParticleSystem m_hParticle;
    [SerializeField] Vector3 m_vOffset;
    [SerializeField] float m_fParticleDuration;

#pragma warning restore 0649
    #endregion

    public override void UseSkill(Transform hOfficer)
    {
        GameManager.Instance.WallHeal(m_fHeal);

        if (m_hParticle)
        {
            var hParticle = Instantiate(m_hParticle, hOfficer.position + m_vOffset, Quaternion.Euler(-90, 0, 0));

            Destroy(hParticle, m_fParticleDuration);
        }
    }
}
