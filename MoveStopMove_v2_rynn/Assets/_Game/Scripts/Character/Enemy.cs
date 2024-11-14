using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Linq;

public class Enemy : CharacterManager
{
    private IEnemyState currentState;

    public List<Transform> wayPoints = new List<Transform>();

    internal NavMeshAgent agent;

    internal Rigidbody rb;

    internal Enemy scripts;

    internal Collider _collider;

    int currentWaypointIndex;

    public float timeStart = 3f;

    public float timeCountdownt = 0;

    public NameCharacter EnemyName;
    public override void Awake()
    {
        base.Awake();

        EnemyName = (NameCharacter)Random.Range(0, 9);

        Name.text = "" + EnemyName;
    }

    public override void Start()
    {
        CharacterTransform = this.transform;

        base.Start();

        checkFirstAttack = true;

        agent = GetComponent<NavMeshAgent>();

       _collider = GetComponent<Collider>();

        rb = GetComponent<Rigidbody>();

        scripts = GetComponent<Enemy>();

        currentWaypointIndex = Random.Range(Random.Range(0, 10), wayPoints.Count);

        ChangeState(new IdleSM());
    }

    public override void Update()
    {
        if (GameManager.Instance.isGameActive == true)
        {
            currentState.Execute();
        }

        deadFunction();

        timeCountdownt -= Time.deltaTime;

        timeCountdownt = Mathf.Clamp(timeCountdownt, 0, Mathf.Infinity);

        base.Update();

        if (timeCountdownt <= 0)
        {
            showWeapon();
        }

        if(isDead == true)
        {
            GameManager.Instance.TotalAlive -= 1;

            GUIManager.Instance.EnemyCountNumber.text = GameManager.Instance.TotalAlive.ToString();
        }

        //if (nearestCharacter != null)
        //{
        //    TargetFoot.SetActive(true);
        //}
        //else
        //{
        //    TargetFoot.SetActive(false);
        //}
    }

    public void Fire()
    {
        if(nearestCharacter == null)
        {
            MyAnimator.ResetTrigger(AnimAttackTag);
            return;
        }

        GameObject poolingBullet = null;

        HideWeapon();

        if (bullet.name == HammerBulletName)
        {
            poolingBullet = PoolBullet.Instance.GetPooledBullet();
        }
        else if (bullet.name == CandyBulletName)
        {
            poolingBullet = PoolCandyBullet.Instance.GetPooledBullet();
        }
        else if (bullet.name == KnifeBulletName)
        {
            poolingBullet = PoolKnife.Instance.GetPooledBullet();
        }

        BulletsWeapon InfoBulletAfterPool = poolingBullet.GetComponent<BulletsWeapon>();

        Transform bulletTransForm = poolingBullet.transform;

        Transform nearestTransform = nearestCharacter.transform;

        bulletTransForm.position = PointSpawnBullet.position;

        bulletTransForm.rotation = bulletTransForm.rotation;

        poolingBullet.SetActive(true);

        InfoBulletAfterPool.SetTargetPosition(nearestTransform.position);

        InfoBulletAfterPool.SetOwnerChar(this);

        InfoBulletAfterPool.SetOwnerPos(CharacterTransform.position);

    }

    public IEnumerator Attacked()
    {
        MyAnimator.SetTrigger(AnimAttackTag);

        yield return new WaitForSeconds(0.4f);

        Fire();
    }

    public void deadFunction()
    {
        if(isDead == true)
        {
            _collider.enabled = false;

            rb.detectCollisions = false;

            scripts.enabled = false;

            agent.enabled = false;

            GameManager.Instance._listCharacter.Remove(this);
        }
    }
    public void Move()
    {
        Transform wp = wayPoints[currentWaypointIndex];

        if (Vector3.Distance(agent.transform.position, wp.position) < 0.01f)
        {
            MyAnimator.SetBool(AnimIdleTag, true);

            agent.transform.position = wp.position;

            currentWaypointIndex = Random.Range(Random.Range(0, 10), wayPoints.Count);
        }
        else
        {
            MyAnimator.SetBool(AnimIdleTag, false);

            agent.SetDestination(wp.position);

            agent.transform.LookAt(wp.position);
        }
    }

    public void CancelDestination()
    {
        currentWaypointIndex = Random.Range(Random.Range(0, 10), wayPoints.Count);

        agent.SetDestination(agent.transform.position);
    }

    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }
  }
