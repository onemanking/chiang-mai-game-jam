using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="UI_EventAddOn_UpgradeStatus",menuName ="UI/Event Add On/Upgrade Status")]
public class UI_EventAddOn_UpgradeStatus : UI_BaseEventAddOn
{
    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] float m_fUpDamage = 1;
    [SerializeField] float m_fDecreadAttackDelay = 0.1f;

#pragma warning restore 0649
    #endregion

    public override void Run(Transform hOwner)
    {
        CGlobal_StatusManager.UpgradeCharacter(hOwner, m_fUpDamage, m_fDecreadAttackDelay);
    }
}
