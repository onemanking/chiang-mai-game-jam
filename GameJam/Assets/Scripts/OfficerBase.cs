using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficerBase : CharacterBase
{
	[SerializeField] protected int attackRange;

	protected override bool IsCanAttack(string _tag, out CharacterBase _target)
	{
		Debug.DrawLine(transform.position, transform.TileDown(attackRange));
		var hits = Physics2D.LinecastAll(transform.position, transform.TileDown(attackRange));
		foreach (var hit in hits)
		{
			if (hit.collider)
			{
				if (hit.collider.gameObject != gameObject)
				{
					_target = hit.collider.CompareTag(_tag) ? hit.collider.GetComponent<CharacterBase>() : null;
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
