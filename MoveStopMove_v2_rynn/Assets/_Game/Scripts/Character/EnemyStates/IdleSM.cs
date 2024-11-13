using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSM : IEnemyState
{
    private Enemy enemy;

    private float idleTimer;

    private float idleDuration = Random.Range(2f, 4f);

    private float idleToAttackDelay = Random.Range(1.7f, 2.5f);

    private const string AnimIdleTag = "IsIdle";
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;

        enemy.CancelDestination();
    }

    public void Execute()
    {
        enemy.FindAround();

        Idle();

        IdleToAttack();

        IdleToPatrol();
    }

    public void Exit()
    {
        
    }

    private void Idle()
    {
        enemy.MyAnimator.SetBool(AnimIdleTag, true);

        if(enemy.nearestCharacter != null)
        {
            enemy.transform.LookAt(enemy.nearestCharacter.transform);
        }

    }

    public void IdleToAttack()
    {
        if (enemy.nearestCharacter != null)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleToAttackDelay)
            {
                enemy.checkFirstAttack = true;

                enemy.ChangeState(new AttackSM());
            }
        }
    }

    public void IdleToPatrol()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolSM());
        }
    }
}
