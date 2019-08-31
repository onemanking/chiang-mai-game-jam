using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerBase : CharacterBase
{
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
}
