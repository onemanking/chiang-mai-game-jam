using System;
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

	[SerializeField] protected float m_Hp = 100;
	[SerializeField] protected float m_Damage = 10;
	[SerializeField] protected float m_AttackDelay = 1;
	[SerializeField] protected float m_AttackRange = 1;
	[SerializeField] protected float m_AttackWidth = 1;
	[SerializeField] protected TargetTag m_TargetTag;

	[Header("Animation")]
	[SerializeField] protected Transform head;
	[SerializeField] protected Transform[] rotatableParts;
	[SerializeField] private ParticleSystem m_particle;
	[SerializeField] private AudioClip m_AttackSound;

	protected Rigidbody rigid;
	protected Collider coll;
	private Animator anim;
	private AudioSource audioSource;
	protected int layerMask;
	protected CharacterBase currentTarget;
	private float attackTimer;

	// Start is called before the first frame update
	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		coll = GetComponent<Collider>();
		anim = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		layerMask = 1 << gameObject.layer;
		layerMask = ~layerMask;
	}

    protected virtual void OnEnable()
    {
        // Register character.
        CGlobal_CharacterManager.RegisterCharacter(transform);
    }

    protected virtual void OnDisable()
    {
        // Unregister character.
        CGlobal_CharacterManager.UnregisterCharacter(transform);
    }

    protected virtual void Update()
	{
		if (IsCanAttack(m_TargetTag, out currentTarget)) AttakeCurrentTarget();
		else ResetLooking();
	}

	protected virtual void AttakeCurrentTarget()
	{
		LookingToTarget();
		if (Time.time >= attackTimer)
		{
			attackTimer = Time.time + m_AttackDelay;
			anim.Play("Attack");
			if(audioSource) audioSource.PlayOneShot(m_AttackSound);
			currentTarget.TakeDamage(m_Damage);
		}
	}

	private void ResetLooking()
	{
		foreach (var tr in rotatableParts)
		{
			tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 2f);
		}
	}

	protected virtual void LookingToTarget()
	{
		foreach (var tr in rotatableParts)
		{
			tr.LookAt(currentTarget.head);
		}
	}

	public virtual void TakeDamage(float _dmg)
	{
		m_Hp -= _dmg;
		Instantiate(m_particle, head.position, Quaternion.identity);
		if (m_Hp <= 0) Dead();
	}

	protected virtual void Dead()
	{
		Destroy(gameObject);
	}

	protected abstract bool IsCanAttack(TargetTag _tag, out CharacterBase _target);

	private void OnDrawGizmos()
	{
		if (coll)
			Gizmos.DrawWireCube(transform.position, new Vector3(m_AttackWidth, coll.bounds.extents.y * 10, coll.bounds.size.z * 2 * m_AttackRange));
	}
}
