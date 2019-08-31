using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerBase : CharacterBase
{
	protected override bool IsCanAttack(TargetTag _tag, out CharacterBase _target)
	{
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * m_AttackRange);
		var hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), m_AttackRange);
		foreach (var hit in hits)
		{
			if (hit.collider)
			{
				if (hit.collider.gameObject != gameObject)
				{
					_target = hit.collider.CompareTag(_tag.ToString()) ? hit.collider.GetComponent<CharacterBase>() : null;
					return _target != null;
				}
				else
				{
					continue;
				}
			}
			else
			{
				continue;
			}
		}

		_target = null;
		return false;
	}
}
