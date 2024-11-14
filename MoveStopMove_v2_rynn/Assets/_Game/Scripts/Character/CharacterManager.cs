using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public enum NameCharacter 
    {   Johny, 
        Kevil, 
        Tonny, 
        Depp, 
        Bill, 
        Crytal, 
        David, 
        Tucson, 
        Niel, 
        Tonado 
    };
public class CharacterManager : MonoBehaviour, IHit
{
    //string
    #region
    [HideInInspector] public const string CharacterTag = "Character";

    [HideInInspector] public const string AnimIdleTag = "IsIdle";

    [HideInInspector] public const string AnimAttackTag = "IsAttack";

    [HideInInspector] public const string AnimDeadTag = "IsDead";

    [HideInInspector] public const string OndespawnTag = "OnDespawn";

    [HideInInspector] public const string showWeaponTag = "showWeapon";

    [HideInInspector] public const string bulletTag = "Bullet";

    [HideInInspector] public const string AnimDanceTag = "IsDance";

    [HideInInspector] public const string HammerBulletName = "Hammer Bullet";

    [HideInInspector] public const string CandyBulletName = "Candy Bullet";

    [HideInInspector] public const string KnifeBulletName = "Knife Bullet";

    [HideInInspector] public const string PlayerTag = "Player";

    [HideInInspector] public const string EnemyTag = "Enemy";

    #endregion

    //bool
    #region
    [Header("Logic / Boolean")]
    public bool isDead;

    public bool checkFirstAttack;

    public bool isMoving;

    #endregion

    //float 
    #region Range - Timer
    [Header ("Settings")]
    public float range;

    public float timer;
    #endregion

    //GameObject
    #region Object Weapon
    [Header ("Attack")]
    public GameObject WeaponHand;

    [HideInInspector] public GameObject nearestCharacter;
    
    public GameObject TargetFoot;

    #endregion
    //Transform
    #region Point Spawn Bullet
    [Header("Point spawn bullet")]
    public Transform PointSpawnBullet;
    #endregion

    public TextMeshProUGUI Name;

    [Header("Health")]
    [SerializeField] int heal;

    [Header("Effect")]
    public ParticleSystem effectOnDead;

    [Header("Score")]
    public TextMeshProUGUI ScoreText;

    public int Score = 0;

    [Header("Bullet")]
    public GameObject bullet;

    public Vector3 bulletPos;

    internal Transform CharacterTransform;

    public Animator MyAnimator { get; private set; }

    public int Damage;

    [HideInInspector] public Animator AnimName;
    public virtual void Awake()
    {
        CharacterTransform = this.transform;

        AnimName = Name.GetComponent<Animator>();
    }
    public virtual void Start()
    {
        MyAnimator = GetComponent<Animator>();

        GameManager.Instance.AddCharacter(this);

        nearestCharacter = null;

        Damage = 10;
    }

    public virtual void Update()
    {
        FindAround();

        if (this.gameObject.activeInHierarchy == false)
        {
            isDead = true;

            GameManager.Instance._listCharacter.Remove(this);
        }
    }
    // Find target
    public void FindAround()
    {
        float shortestDistance = Mathf.Infinity; // Find a shortes

        GameObject target = null; // Target

        // check all target in game manager
        for (int i = 0; i < GameManager.Instance._listCharacter.Count; i++) 
        {
            if(this != GameManager.Instance._listCharacter[i])
            {
                float distanceToOtherCharacter = Vector3.Distance(this.gameObject.transform.position, GameManager.Instance._listCharacter[i].transform.position);

                // check thang nao gan nhat
                if (distanceToOtherCharacter < shortestDistance)
                {
                    shortestDistance = distanceToOtherCharacter;

                    target = GameManager.Instance._listCharacter[i].gameObject;
                }
            }
        }
        nearestCharacter = target; // gan lai vao thang target
        
        if (target != null && shortestDistance < range * target.transform.localScale.z /* nhan voi scale cua nhan vat de thay foot target*/)
        {
            nearestCharacter = target;

            if (target.tag == PlayerTag && target.tag != EnemyTag)
            {
                TargetFoot.gameObject.SetActive(true); // active Target sprite
            }
        }
        else
        {
            TargetFoot.gameObject.SetActive(false);

            nearestCharacter = null;
        }
    }

    // DEAD ==========================================
    public void OnDead()
    {
        if(isDead == true)
        {
            ParticleSystem effectDead = Instantiate(effectOnDead, transform.position, Quaternion.identity);

            Destroy(effectDead.gameObject, 1f);

            Invoke(OndespawnTag, 1.2f);

            MyAnimator.SetBool(AnimDeadTag, true);

            GameManager.Instance._listCharacter.Remove(this);
        }
    }

    // SHOW WEAPON ============================
    public void showWeapon()
    {
        WeaponHand.SetActive(true);
    }

    public void /*IEnumerator*/ HideWeapon()
    {
        //yield return new WaitForSeconds(0.42f);

        WeaponHand.SetActive(false);
    }

    // SHOOT
    public void FireBullet()
    {
        // Cancel attack (IF MOVING)
        if(isMoving == true || nearestCharacter == null)
        {
            return;
        }

        // SPAWN BULLET
        GameObject poolingBullet = null;

        // HIDE WEAPON BEFORE SHOOT
        HideWeapon();

        // CHECK THE BULLET NAME (POOLING OUT THE OBJECT)
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

        // GET INFO BULLET AFTER POOL
        BulletsWeapon infoBulletAfterPool = poolingBullet.GetComponent<BulletsWeapon>(); // get component

        // TRANSFORM BULLET
        Transform bulletTransForm = infoBulletAfterPool.transform;

        Transform nearestTransform = nearestCharacter.transform;

        bulletTransForm.localScale = CharacterTransform.localScale;

        bulletTransForm.position = PointSpawnBullet.position;

        bulletTransForm.rotation = bulletTransForm.rotation;

        poolingBullet.SetActive(true);

        // SET TO TARGET POSITION FIRST
        infoBulletAfterPool.SetTargetPosition(nearestTransform.position);

        // SET BULLET SHOOT FROM WHOM
        infoBulletAfterPool.SetOwnerChar(this);

        // POSITION SHOOT
        infoBulletAfterPool.SetOwnerPos(CharacterTransform.position);
    }

    public IEnumerator Attacking()
    {
        MyAnimator.SetTrigger(AnimAttackTag);

        yield return new WaitForSeconds(0.44f);

        FireBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        CandyBullet bulletWeaponScript = other.gameObject.GetComponent<CandyBullet>();

        Transform bulletOfOwnerTransForm = bulletWeaponScript.characterOwner.transform;

        if (other.gameObject.CompareTag(bulletTag))
        {
            if (this != bulletWeaponScript.characterOwner) // kiem tra neu thang nem vu khi khac chinh no thi thuc hien
            {
                OnHit(Damage);

                other.gameObject.SetActive(false);

                GameObject BGKillFeed = Instantiate(GUIManager.Instance.KillFeed, GUIManager.Instance.SpawnKillFeedPos);

                TextMeshProUGUI EnemyTextOfKillfeed = BGKillFeed.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                TextMeshProUGUI PlayerTextOfKillfeed = BGKillFeed.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                PlayerTextOfKillfeed.text = bulletWeaponScript.characterOwner.Name.text;

                EnemyTextOfKillfeed.text = this.Name.text;

                Destroy(BGKillFeed, 2);
            }

            if(this.name != bulletWeaponScript.characterOwner.name)
            {
                bulletWeaponScript.characterOwner.Score++;

                bulletWeaponScript.characterOwner.ScoreText.text = bulletWeaponScript.characterOwner.Score.ToString();

                bulletOfOwnerTransForm.localScale += new Vector3(0.1f, 0.1f, 0.1f);

                bulletWeaponScript.characterOwner.Damage += 1;

                if(bulletWeaponScript.characterOwner.Damage >= 15)
                {
                    bulletWeaponScript.characterOwner.Damage = 15;
                }

                bulletWeaponScript.characterOwner.range += 0.025f;

                if(bulletWeaponScript.characterOwner.tag == PlayerTag)
                {
                    GameManager.Instance.cameraOnMenu.m_Lens.FieldOfView += 2;

                    if(GameManager.Instance.cameraOnMenu.m_Lens.FieldOfView >= 70)
                    {
                        GameManager.Instance.cameraOnMenu.m_Lens.FieldOfView = 70;
                    }
                }

                if (bulletWeaponScript.characterOwner.range >= 0.4f)
                {
                    bulletWeaponScript.characterOwner.range = 0.4f;
                }

                if(bulletOfOwnerTransForm.localScale.x >= 1.5)
                {
                    bulletOfOwnerTransForm.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    // OnHit - Deal damage 
    public void OnHit(int damage)
    {
        heal -= damage;

        if (heal <= 0)
        {
            heal = 0;

            isDead = true;
        }
        OnDead();
    }

    public void GetWeaponHand(GameObject WeaponInHand)
    {
        WeaponHand = WeaponInHand;
    }
}

