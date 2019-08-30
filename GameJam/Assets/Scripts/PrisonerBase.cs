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
		transform.position = Vector2.Lerp(transform.position, transform.TileUp(), Time.deltaTime * m_Speed);
	}

	protected override bool IsCanAttack(string _tag, out CharacterBase _target)
	{
		Debug.DrawLine(transform.position, transform.TileUp());
		var hits = Physics2D.LinecastAll(transform.position, transform.TileUp());
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
