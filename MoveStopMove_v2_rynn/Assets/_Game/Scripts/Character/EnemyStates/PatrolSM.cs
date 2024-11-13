using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSM : IEnemyState
{
    private float patrolTimer;

    private float patrolDuration = Random.Range(3f, 4f);

    private float patrolToAttackDelay = Random.Range(1.5f, 2f);

    private const string AnimIdleTag = "IsIdle";

    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        enemy.FindAround();

        Patrol();

        CheckingTarget();

        PatrolToIdle();
    }

    public void Exit()
    {

    }

    private void Patrol()
    {
        enemy.MyAnimator.SetBool(AnimIdleTag, false);

        enemy.Move();
    }

    public void CheckingTarget()
    {
        if (enemy.nearestCharacter != null)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolToAttackDelay)
            {
                enemy.CancelDestination();

                enemy.MyAnimator.SetBool(AnimIdleTag, true);

                enemy.ChangeState(new IdleSM());
            }
        }
    }

    public void PatrolToIdle()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleSM());
        }
    }
}
