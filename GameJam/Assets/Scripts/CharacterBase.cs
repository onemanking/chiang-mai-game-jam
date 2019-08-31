using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CapsuleCollider))]
public abstract class CharacterBase : MonoBehaviour
{
	protected enum TargetTag
	{
		Officer,
		Prisoner
	}

	[SerializeField] protected float m_Hp;
	[SerializeField] protected float m_Damage;
	[SerializeField] protected float m_AttackDelay;
	[SerializeField] protected float m_AttackRange;
	[SerializeField] protected TargetTag m_TargetTag;

	protected Rigidbody rigid;

	protected CharacterBase currentTarget;
	private float attackTimer;

	// Start is called before the first frame update
	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	protected virtual void Update()
	{
		if (IsCanAttack(m_TargetTag, out currentTarget)) AttakeCurrentTarget();
	}

	protected virtual void AttakeCurrentTarget()
	{
		if (Time.time >= attackTimer)
		{
			attackTimer = Time.time + m_AttackDelay;
			currentTarget.TakeDamage(m_Damage);
			Debug.LogError($"attack to {currentTarget.gameObject.name}");
		}
	}

	protected virtual void TakeDamage(float _dmg)
	{
		m_Hp -= _dmg;
		if (m_Hp <= 0) Dead();
	}

	protected virtual void Dead()
	{
		Destroy(gameObject);
	}

	protected abstract bool IsCanAttack(TargetTag _tag, out CharacterBase _target);
}
