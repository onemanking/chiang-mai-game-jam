using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_RocketBomb", menuName = "Skill/Rocket Bomb")]
public class Officer_Skill_RocketBomb : Officer_BaseSkill
{
    #region Variable - Inspector

    [Header("Bomb")]
    [SerializeField] float m_fDamageMultiplier = 6f;
    [SerializeField] Vector3 m_vThrowForce = new Vector3(150, 200, 0);
    [SerializeField] Weapon_BombController m_hPrefabBomb;

    #endregion

    public override void UseSkill(Transform hOfficer)
    {
       
        if (hOfficer == null || m_hPrefabBomb == null)
            return;

        var hBombController = Instantiate(m_hPrefabBomb, hOfficer.position, Quaternion.identity);
        hBombController.Init(hOfficer, m_fDamageMultiplier,m_vThrowForce);
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
