using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
	[SerializeField] private float m_Speed;
	[SerializeField] private float m_Damage;
	[SerializeField] private float m_Hp;
	[SerializeField] private float m_AttackRange;

	void Start()
	{

	}

	protected virtual void Update()
	{
		if (IsCanAttack()) Attack();
		else Move();
	}

	protected virtual void Attack()
	{
		Debug.Log("attack");
	}

	protected virtual void Move()
	{
		transform.position = Vector2.Lerp(transform.position, transform.TileUp(), Time.deltaTime * m_Speed);
	}

	private bool IsCanAttack()
	{
		Debug.DrawLine(transform.position, transform.TileUp());
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TileUp());
		if (hit.collider)
			return hit.collider.CompareTag("Officer");
		else
			return false;
	}
}
