using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterStatus",menuName ="Status/Character")]
public class Data_CharacterStatus : abst_Data_CharacterStatus
{
    #region Variable - Inspector
#pragma warning disable 0649

    [SerializeField] CharacterStatus[] m_arrStatus;

#pragma warning restore 0649
    #endregion

    public override CharacterStatus[] AllStatus { get { return m_arrStatus; } }
}
