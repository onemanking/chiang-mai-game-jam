using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_RocketBomb", menuName = "Skill/Rocket Bomb")]
public class Officer_Skill_RocketBomb : Officer_BaseSkill
{
    #region Variable - Inspector

    [Header("Damage")]
    [SerializeField] float m_fDamageMultiplier = 6f;

    #endregion

    public override void UseSkill(Transform hOfficer)
    {
        if (hOfficer == null)
            return;

        var hOfficerBase = hOfficer.GetComponent<OfficerBase>();

        if (hOfficerBase == null)
            return;

        var lstGO = CGlobal_CharacterManager.GetCharacterList(TagType.Prisoner);

        if (lstGO == null || lstGO.Count <= 0)
            return;

        float fDamage = hOfficerBase.GetDamage() * m_fDamageMultiplier;

        for(int i = 0; i < lstGO.Count; i++)
        {
            // For test only.
            lstGO[i].SendMessage("TakeDamage", fDamage,SendMessageOptions.DontRequireReceiver);
        }
    }
}
