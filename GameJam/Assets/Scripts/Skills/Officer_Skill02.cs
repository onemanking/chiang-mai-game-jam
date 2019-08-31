using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill02", menuName = "Skill/02")]
public class Officer_Skill02 : Officer_BaseSkill
{
    public override void UseSkill(Transform hOfficer)
    {
        var arrGO = GameObject.FindGameObjectsWithTag("Prisoner");

        if (arrGO == null || arrGO.Length <= 0)
            return;

        for (int i = 0; i < arrGO.Length; i++)
        {
            // For test only.
            arrGO[i].SendMessage("TakeDamage", 50, SendMessageOptions.DontRequireReceiver);


            //Destroy(arrGO[i]);
        }
    }
}
