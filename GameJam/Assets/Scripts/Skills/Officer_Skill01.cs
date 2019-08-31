using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill01", menuName = "Skill/01")]
public class Officer_Skill01 : Officer_BaseSkill
{
    public override void UseSkill(Transform hOfficer)
    {
        var lstGO = CGlobal_CharacterManager.GetCharacterList(TagType.Prisoner);

        if (lstGO == null || lstGO.Count <= 0)
            return;

        for(int i = 0; i < lstGO.Count; i++)
        {
            // For test only.
            lstGO[i].SendMessage("TakeDamage", 50,SendMessageOptions.DontRequireReceiver);


            //Destroy(arrGO[i]);
        }
    }
}
