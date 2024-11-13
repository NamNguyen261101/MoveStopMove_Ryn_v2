using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSM : IEnemyState
{
    private Enemy _Enemy;

    private float delayTime;

    Vector3 _EnemyPos, nearestCharPos;

    Transform nearestCharTransform;
    
    public void Enter(Enemy enemy)
    {
        this._Enemy = enemy;

        _EnemyPos = _Enemy.transform.position;

        nearestCharPos = _Enemy.nearestCharacter.transform.position;

        nearestCharTransform = _Enemy.nearestCharacter.transform;

        CheckAttack();

        Attack();
    }

    public void Execute()
    {
        AttackToPatrol();

        AttackToIdle();
    }

    public void Exit()
    {

    }

    private void Attack()
    {
        if (_Enemy.nearestCharacter != null)
        {
            _Enemy.transform.LookAt(nearestCharTransform);
        }

        _Enemy.StartCoroutine(_Enemy.Attacked());
    }

    public void CheckAttack()
    {
        if (Vector3.Distance(_EnemyPos, nearestCharPos) < _Enemy.range && _Enemy.timeCountdownt <= 0 && _Enemy.checkFirstAttack && _Enemy.isMoving == false)
        {
            _Enemy.timeCountdownt = _Enemy.timeStart;

            _Enemy.checkFirstAttack = false;
        }
    }

    public void AttackToPatrol()
    {   delayTime += Time.deltaTime;

        if (delayTime >= Random.Range(1f, 1.5f))
        {
            _Enemy.ChangeState(new PatrolSM());
        }
     
    }

    public void AttackToIdle()
    {
        if (_Enemy.nearestCharacter == null)
        {
            _Enemy.ChangeState(new IdleSM());
        }
    }
}
