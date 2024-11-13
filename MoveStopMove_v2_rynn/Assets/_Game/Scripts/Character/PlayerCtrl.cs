using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerCtrl : CharacterManager
{
    public static PlayerCtrl Instance;
    [HideInInspector] public enum WeaponType {Hammer, Candy, Knife};

    [HideInInspector] public enum ItemsType
    {
        //Pant
        Chambi, Comy, Dabao, Batman, Pokemon, Onion, Vantim, Rainbow,

        //Head
        ArrowHead, Cowboy, Police, Snap, Tainghe, Taitho, Murom,

        //Shield
        CaptainShield1, CaptainShield2,

        //SkinSet
        Devil, Angel, Witch
    }

    [HideInInspector] public enum SetFullOrNormal { SetFull, Normal }

    [HideInInspector] public SetFullOrNormal lastItems;

    public ItemsInfo PlayerItems;

    public Transform HeadPos;

    public Transform TailPos;

    public Transform WingPos;

    public Transform ShieldPos;

    public Renderer PanPos;

    public Renderer SkinPos;

    [Header("Joystick")]
    [SerializeField] FloatingJoystick joystick;

    [SerializeField] GameObject joystickObject;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    
    [SerializeField] private float rotationSpeed;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Collider")]
    Collider _collider;

    [Header("Time Cooldown")]
    public float timeStart = 1.5f;

    public float timeCountdownt = 0;

    Transform playerTransform;

    public GameObject[] weaponArray = new GameObject[3];

    public GameObject[] BulletArray = new GameObject[3];

    public WeaponInfo weaponInfo;

    public override void Awake()
    {
        base.Awake();
        
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        playerTransform = this.transform;

        _collider = GetComponent<Collider>();

        UpdatePlayerItems();
    }

    public override void Update()
    {
        base.Update();
        
        if (GameManager.Instance.isGameActive == true)
        {
            joystickObject.SetActive(true);

            MyAnimator.SetBool(AnimDanceTag, false);
        }
        else
        {
            MyAnimator.SetBool(AnimDanceTag, true);
        }

        for(int i = 0; i < GameManager.Instance._listCharacter.Count; i++)
        {
            if (GameManager.Instance._listCharacter.Count == 1 && this == GameManager.Instance._listCharacter[i])
            {
                GameManager.Instance.isWin = true;
            }
        }

        if (GameManager.Instance.isWin == true)
        {
            moveSpeed = 0;

            _collider.enabled = false;

            MyAnimator.SetBool(AnimIdleTag, true);

            MyAnimator.SetBool(AnimDanceTag, true);
        }

        if(this.isDead == true)
        {
            GameManager.Instance.isLose = true;
        }

        if (GameManager.Instance.isGameActive == true)
        {
            if (isDead == false)
            {
                PlayerMovement();
            }

            if (nearestCharacter != null)
            {
                Transform nearestCharacterTransform = nearestCharacter.transform;

                if (Vector3.Distance(playerTransform.position, nearestCharacterTransform.position) < range && timeCountdownt <= 0 && checkFirstAttack && isMoving == false)
                {
                    timeCountdownt = timeStart;

                    checkFirstAttack = false;

                    StartCoroutine(Attacking());
                }
            }
            timeCountdownt -= Time.deltaTime;

            timeCountdownt = Mathf.Clamp(timeCountdownt, 0, Mathf.Infinity);

            if (nearestCharacter != null && isMoving == false)
            {
                Transform nearestTransform = nearestCharacter.transform;

                playerTransform.LookAt(nearestTransform);
            }
        }

        if(timeCountdownt <= 0)
        {
            showWeapon();
        }
    }

    public void PlayerMovement()
    {
        isMoving = true;

        float xInput = joystick.Horizontal;

        float zInput = joystick.Vertical;

        Vector3 movementDirection = new Vector3(xInput, 0f, zInput);

        movementDirection.Normalize();

        transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);

            checkFirstAttack = true;

            MyAnimator.SetBool(AnimIdleTag, false);
        }
        else
        {
            isMoving = false;

            MyAnimator.SetBool(AnimIdleTag, true);
        }
    }

    public void UpdatePlayerItems()
    {
        for(int i = 0; i < 20; i++)
        {
            if(PlayerPrefs.GetInt("ItemsShop" + (ItemsType)i) == 4)
            {
                ChangeItems((ItemsType)i);
            }
        }

        for(int i = 0; i < 2; i++)
        {
            if(PlayerPrefs.GetInt("WeaponShop" + (WeaponType)i) == 4)
            {
                WeaponSwitch((WeaponType)i);
            }
        }
    }

    // Change Skin

    public void WeaponSwitch(WeaponType _weaponType)
    {
        for(int i = 0; i < weaponArray.Length; i++)
        {
            if(i == (int)_weaponType)
            {
                weaponArray[i].gameObject.SetActive(true);

                WeaponHand = weaponArray[i].gameObject;

                bullet = BulletArray[i].gameObject;
            }
            else
            {
                weaponArray[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChangeItems(ItemsType _ItemsType)
    {
        SetFullOrNormal _setFullOrNormal;

        _setFullOrNormal = ((int)_ItemsType > 16) ? (SetFullOrNormal.SetFull) : (SetFullOrNormal.Normal);

        if (_setFullOrNormal != lastItems || _setFullOrNormal == SetFullOrNormal.SetFull)
        {
            ResetItem();
        }

        switch (_ItemsType)
        {
            case ItemsType.Chambi:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[0];
                    break;
                }
            case ItemsType.Comy:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[1];
                    break;
                }
            case ItemsType.Dabao:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[2];
                    break;
                }
            case ItemsType.Batman:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[3];
                    break;
                }
            case ItemsType.Pokemon:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[4];
                    break;
                }
            case ItemsType.Onion:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[5];
                    break;
                }
            case ItemsType.Vantim:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[6];
                    break;
                }
            case ItemsType.Rainbow:
                {
                    GetDefaultItems();

                    ResetHeadPos();

                    ResetShieldPos();

                    PanPos.sharedMaterial = PlayerItems.PantsMaterials[7];
                    break;
                }
            case ItemsType.ArrowHead:
                {
                    ResetHeadPos();

                    GetDefaultItems();

                    ResetShieldPos();

                    Instantiate(PlayerItems.HeadPos[0], HeadPos);
                    
                    break;
                }
            case ItemsType.Cowboy:
                {
                    ResetHeadPos();

                    GetDefaultItems();

                    ResetShieldPos();

                    Instantiate(PlayerItems.HeadPos[1], HeadPos);

                    break;
                }
            case ItemsType.Police:
                {
                    ResetHeadPos();

                    GetDefaultItems();

                    ResetShieldPos();

                    Instantiate(PlayerItems.HeadPos[2], HeadPos);

                    break;
                }
            case ItemsType.Snap:
                {
                    ResetHeadPos();

                    GetDefaultItems();

                    ResetShieldPos();

                    Instantiate(PlayerItems.HeadPos[3], HeadPos);

                    break;
                }
            case ItemsType.Tainghe:
                {
                    ResetHeadPos();

                    GetDefaultItems();

                    ResetShieldPos();

                    Instantiate(PlayerItems.HeadPos[4], HeadPos);

                    break;
                }
            case ItemsType.Taitho:
                {
                    ResetHeadPos();

                    GetDefaultItems();

                    ResetShieldPos();

                    Instantiate(PlayerItems.HeadPos[5], HeadPos);

                    break;
                }
            case ItemsType.Murom:
                {
                    ResetHeadPos();

                    GetDefaultItems();

                    ResetShieldPos();

                    Instantiate(PlayerItems.HeadPos[6], HeadPos);

                    break;
                }
            case ItemsType.CaptainShield1:
                {
                    ResetShieldPos();

                    ResetHeadPos();

                    GetDefaultItems();

                    Instantiate(PlayerItems.ShieldPos[0], ShieldPos);

                    break;
                }
            case ItemsType.CaptainShield2:
                {
                    ResetShieldPos();

                    ResetHeadPos();

                    GetDefaultItems();

                    Instantiate(PlayerItems.ShieldPos[1], ShieldPos);

                    break;
                }
            case ItemsType.Devil:
                {
                    Instantiate(PlayerItems.HeadPos[7], HeadPos);

                    Instantiate(PlayerItems.TailPos[0], TailPos);

                    Instantiate(PlayerItems.WingPos[0], WingPos);

                    SkinPos.sharedMaterial = PlayerItems.SkinMaterials[1];

                    break;
                }
            case ItemsType.Angel:
                {
                    Instantiate(PlayerItems.HeadPos[8], HeadPos);

                    Instantiate(PlayerItems.ShieldPos[2], ShieldPos);

                    Instantiate(PlayerItems.WingPos[1], WingPos);

                    SkinPos.sharedMaterial = PlayerItems.SkinMaterials[2];

                    break;

                }
            case ItemsType.Witch:
                {
                    Instantiate(PlayerItems.HeadPos[9], HeadPos);

                    Instantiate(PlayerItems.ShieldPos[3], ShieldPos);

                    SkinPos.sharedMaterial = PlayerItems.SkinMaterials[3];

                    break;

                }
        }

        lastItems = _setFullOrNormal;
    }

    //Reset Skin or Items To Default
    
    public void ResetItem()
    {
        ResetHeadPos();
        ResetShieldPos();
        ResetTailPos();
        ResetWingPos();
        GetDefaultItems();
    }

    public void GetDefaultItems()
    {
        PanPos.sharedMaterial = PlayerItems.PantsMaterials[8];

        SkinPos.sharedMaterial = PlayerItems.SkinMaterials[0];
    }

    public void ResetHeadPos()
    {
        foreach(Transform item in HeadPos)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetTailPos()
    {
        foreach (Transform item in TailPos)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetWingPos()
    {
        foreach (Transform item in WingPos)
        {
            Destroy(item.gameObject);
        }
    }

    public void ResetShieldPos()
    {
        foreach(Transform item in ShieldPos)
        {
            Destroy(item.gameObject);
        }
    }
}
