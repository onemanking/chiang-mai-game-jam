using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerBase : CharacterBase
{
	[SerializeField] protected float m_Speed;

	void Start()
	{
	}

	protected override void Update()
	{
		if (IsCanAttack(m_TargetTag, out currentTarget)) AttakeCurrentTarget();
		else Move();
	}

	protected virtual void Move()
	{
		rigid.velocity = transform.forward * m_Speed;
	}

	protected override bool IsCanAttack(TargetTag _tag, out CharacterBase _target)
	{
		Debug.DrawRay(transform.position, transform.forward + transform.up * m_AttackRange);
		var hits = Physics.RaycastAll(transform.position, transform.forward + transform.up, m_AttackRange);
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
