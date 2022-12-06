using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Attack : BTBaseNode
{
    private readonly IDamageable target;
    private readonly GameObject attacker;
    
    
    public BT_Attack(IDamageable _target, GameObject _attacker)
    {
        target = _target;
        attacker = _attacker;
    }

    public override TaskStatus Run()
    {
        target.TakeDamage(attacker, 100);
        return TaskStatus.Success;
    }
}
