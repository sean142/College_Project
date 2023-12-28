using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack",menuName ="Attack/AttackData")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;

    public float skillRange;

    public float coolDown;

    public float continuousClickMouseLeftCoolTime;

    public int minDamage;

    public int maxDamage;

    public float criticalMultiplier; //爆擊的加成百分比

    public float criticalChance;//爆擊率
}
