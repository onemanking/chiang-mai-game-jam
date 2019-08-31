using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Struct

[System.Serializable]
public struct CharacterStatus
{
    public float m_fMaxHP;
    public float m_fDamage;
    public float m_fAttackDelay;
    public int m_nAttackRange;
}

#endregion

public abstract class abst_Data_CharacterStatus : ScriptableObject
{

    public abstract CharacterStatus[] AllStatus { get; }
}
