using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerBase : CharacterBase
{
	[SerializeField] protected float m_Speed;

	private Transform lookingTarget;

	void Start()
	{
		RaycastHit hit;
		var boundSize = new Vector2(coll.bounds.extents.x, coll.bounds.extents.y * 2);
		var startPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
		if (Physics.BoxCast(startPos, boundSize, transform.forward, out hit, Quaternion.LookRotation(transform.forward), 100, layerMask))
		{
			lookingTarget = hit.collider.CompareTag(m_TargetTag.ToString()) ? hit.collider.transform : null;
		}
	}

	protected override void Update()
	{
		if (IsCanAttack(m_TargetTag, out currentTarget)) AttakeCurrentTarget();
		else Move();
	}

	protected virtual void Move()
	{
		LookingToTarget();
		rigid.velocity = transform.forward * m_Speed;
	}

	protected override void LookingToTarget()
	{
		foreach (var tr in rotatableParts)
		{
			tr.LookAt(lookingTarget);
		}
	}

	protected override bool IsCanAttack(TargetTag _tag, out CharacterBase _target)
	{
		RaycastHit hit;
		var boundSize = new Vector2(m_AttackWidth, coll.bounds.extents.y * 10);
		if (Physics.BoxCast(transform.position, boundSize, transform.forward, out hit, Quaternion.LookRotation(transform.forward), m_AttackRange, layerMask))
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

	public void Init(float _speed, float _dmg, float _atkDelay)
	{
		m_Speed = _speed;
		m_Damage = _dmg;
		m_AttackDelay = _atkDelay;
	}

	public void SetSpeed(float _speed)
	{
		m_Speed = _speed;
	}

    public float GetSpeed()
    {
        return m_Speed;
    }

    public void SetDamage(float _damage)
    {
        m_Damage = _damage;
    }

    public float GetDamage()
    {
        return m_Damage;
    }

    public void SetAttackDelay(float _attackDelay)
    {
        m_AttackDelay = _attackDelay;
    }

    public float GetAttackDelay()
    {
        return m_AttackDelay;
    }
}