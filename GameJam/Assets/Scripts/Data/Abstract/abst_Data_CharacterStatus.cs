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
    public float m_fSpeed;

	public CharacterStatus(float fMaxHP, float fDamage, float fAttackDelay, int nAttackRange, float fSpeed)
	{
		m_fMaxHP = fMaxHP;
		m_fDamage = fDamage;
		m_fAttackDelay = fAttackDelay;
		m_nAttackRange = nAttackRange;
		m_fSpeed = fSpeed;
	}
}

#endregion

public abstract class abst_Data_CharacterStatus : ScriptableObject
{

    public abstract CharacterStatus[] AllStatus { get; }
}
