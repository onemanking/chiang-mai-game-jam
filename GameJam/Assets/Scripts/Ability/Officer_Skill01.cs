using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill01", menuName = "Skill/01")]
public class Officer_Skill01 : abst_Skill
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] float m_fCooldown;

#pragma warning restore 0649
    #endregion

    public override float CooldownTime { get { return m_fCooldown; } }

    #endregion

    public override void UseSkill()
    {
        var arrGO = GameObject.FindGameObjectsWithTag("Prisoner");

        if (arrGO == null || arrGO.Length <= 0)
            return;

        for(int i = 0; i < arrGO.Length; i++)
        {
            // For test only.
            Destroy(arrGO[i]);
        }
    }
}
