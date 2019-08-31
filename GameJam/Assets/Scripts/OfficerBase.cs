using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerBase : CharacterBase
{
	[SerializeField] private int m_Level = 1;

	public int Level { get => m_Level; }

	protected override bool IsCanAttack(TargetTag _tag, out CharacterBase _target)
	{
		RaycastHit hit;
		if (Physics.BoxCast(transform.position, new Vector2(coll.bounds.extents.x, coll.bounds.extents.y * 10), transform.forward, out hit, Quaternion.LookRotation(transform.forward), m_AttackRange, layerMask))
		{
			_target = hit.collider.CompareTag(_tag.ToString()) ? hit.collider.GetComponent<CharacterBase>() : null;
			return _target != null;
		}
		else
		{
			_target = null;
			return false;
		}
	}

	public override void TakeDamage(float _dmg)
	{
		GameManager.Instance.WallTakeDamage(_dmg);
	}

	public virtual void Upgrade(float _upDmg, float _decreadAttackDelay)
	{
		if (m_Level >= Mathf.Infinity) return;

		m_Level += 1;
		UpgradeDamage(_upDmg);
		UpgradeAttackDelay(_decreadAttackDelay);
	}

	protected virtual void UpgradeDamage(float _upDmg)
	{
		m_Damage += _upDmg;
	}

	protected virtual void UpgradeAttackDelay(float _decreadAttackDelay)
	{
		m_AttackDelay = m_AttackDelay != 0 ? m_AttackDelay -= _decreadAttackDelay : 0;
	}
}
