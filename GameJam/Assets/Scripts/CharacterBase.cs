using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
	[SerializeField] protected float m_Hp;
	[SerializeField] protected float m_Damage;
	[SerializeField] protected float m_AttackDelay;
	[SerializeField] protected string m_TargetTag;

	protected CharacterBase currentTarget;
	private float attackTimer;

	// Start is called before the first frame update
	void Awake()
	{

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

	protected abstract bool IsCanAttack(string _tag, out CharacterBase _target);
}
