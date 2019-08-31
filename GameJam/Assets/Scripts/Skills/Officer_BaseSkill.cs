using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Officer_BaseSkill : abst_Skill
{
    #region Variable

    #region Variable - Inspector
#pragma warning disable 0649

    [Header("Icon")]
    [SerializeField] protected Sprite m_hSpirte;

    [Header("Cutscene")]
    [SerializeField] protected Sprite m_hCutsceneSprite;

    [Header("Cooldown")]
    [SerializeField] protected float m_fCooldown;

#pragma warning restore 0649
    #endregion

    public override Sprite SkillSprite { get { return m_hSpirte; } }

    public override Sprite SkillCutsceneSprite { get { return m_hCutsceneSprite; } }

    public override float CooldownTime { get { return m_fCooldown; } }


    #endregion

}
