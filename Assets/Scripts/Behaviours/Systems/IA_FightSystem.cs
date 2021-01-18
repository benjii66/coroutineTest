using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_FightSystem : MonoBehaviour
{
    public event Action<bool, float> OnAttack = null;
    public event Action OnAttackRange = null;
    public event Action OnAttackRangeLost = null;

    [SerializeField, Range(.1f, 100)] float attackRange = 8;
    [SerializeField, Range(.1f, 10)] float attackRate = 1;
    [SerializeField, Range(0, 100)] int fightDamages = 10;

    float attackTimer = 0;
    ITarget attackTarget = null;

    public float AttackRate => attackRate;

    bool IsAtRange()
    {
        if (attackTarget == null) return false;
        return Vector3.Distance(transform.position, attackTarget.TargetPosition) < attackRange;
    }

    public void SetAttackTarget(ITarget _target) => attackTarget = _target;

    public void UpdateFightSystem()
    {
        if (IsAtRange())
            OnAttackRange?.Invoke();
        else OnAttackRangeLost?.Invoke();
    }
    public void AttackTarget()
    {
        if((attackTarget != null && attackTarget.IsDead) || !IsAtRange())
        {
            OnAttack?.Invoke(false, 0);
            return;
        }
        attackTimer += Time.deltaTime;
        if(attackTimer > attackRate)
        {
            OnAttack?.Invoke(true, attackRate);
            attackTarget?.SetDamage(fightDamages);
            attackTimer = 0;
        }
    }
    public void ResetFight() => attackTimer = 0;
}
