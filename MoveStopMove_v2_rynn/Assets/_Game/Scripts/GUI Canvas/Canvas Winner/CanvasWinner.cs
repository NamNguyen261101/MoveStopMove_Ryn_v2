using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class CanvasWinner : MonoBehaviour
{
    private List<ItemsType> _itemsType = new List<ItemsType>(); 

    private List<WeaponType> _weaponType = new List<WeaponType>();

    [SerializeField] private Animator _sliderAnim;

    [SerializeField] private Image[] percentWeaponImage;

    [SerializeField] private Image[] percentItemImage;

    private bool weaponOrOutfit;

    private ItemsType _ItemsPercent;

    private WeaponType _WeaponPercent;

    private int PercentLoad;
    public void Awake()
    {
        PercentLoad = 0;
    }

    public void Start()
    {
        _itemsType.Clear();

        _weaponType.Clear();

        if(PlayerPrefs.GetInt("WeaponOrOutfit") == 1)
        {
            weaponOrOutfit = true;
        }
        else
        {
            weaponOrOutfit = false;
        }

        for(int i = 1; i < 2; i++)
        {
            if(PlayerPrefs.GetInt("WeaponShop" + (WeaponType)i, 99) != 99)
            {
                if(PlayerPrefs.GetInt("WeaponShop" + (WeaponType)i) != 3 && PlayerPrefs.GetInt("WeaponShop" + (WeaponType)i) != 4)
                {
                    _weaponType.Add((WeaponType)i);
                }
            }
        }

        for(int i = 0; i < 20; i++)
        {
            if(PlayerPrefs.GetInt("ItemsShop" + (ItemsType)i, 99) != 99)
            {
                if(PlayerPrefs.GetInt("ItemsShop" + (ItemsType)i) != 3 && PlayerPrefs.GetInt("ItemsShop" + (ItemsType)i) != 4)
                {
                    _itemsType.Add((ItemsType)i);
                }
            }
        }

        if(PlayerPrefs.GetInt("PercentLoad", 99) != 99)
        {
            PercentLoad = PlayerPrefs.GetInt("PercentLoad");
        }

        if(PlayerPrefs.GetInt("ItemsPercent", 99) == 99 && PlayerPrefs.GetInt("WeaponPercent", 99) == 99)
        {
            RandomPercent();
        }

        if(PlayerPrefs.GetInt("ItemsPercent", 99) != 99)
        {
            _ItemsPercent = (ItemsType)PlayerPrefs.GetInt("ItemsPercent");
        }
        
        if(PlayerPrefs.GetInt("WeaponPercent", 99) != 99)
        {
            _WeaponPercent = (WeaponType)PlayerPrefs.GetInt("WeaponPercent");
        }

        PercentLoad += 100;

        PlayerPrefs.SetInt("PercentLoad", PercentLoad);

        PlayerPrefs.Save();

        StartCoroutine(PlayAnim());

        ShowPercentImage();

        for (int i = 1; i < 2; i++)
        {
            if (PlayerPrefs.GetInt("WeaponShop" + (PlayerCtrl.WeaponType)i) == 4)
            {
                GameObject.FindObjectOfType<PlayerCtrl>().WeaponSwitch((PlayerCtrl.WeaponType)i);
            }
        }

        for (int i = 0; i < 20; i++)
        {
            if (PlayerPrefs.GetInt("ItemsShop" + (PlayerCtrl.ItemsType)i) == 4)
            {
                PlayerPrefs.SetInt("ItemsShop" + (PlayerCtrl.ItemsType)i, 3);

                GameObject.FindObjectOfType<PlayerCtrl>().ChangeItems((PlayerCtrl.ItemsType)i);
            }
        }
    }

    public void Update()
    {
        if (PercentLoad > 100)
        {
            GetPercent();
        }
    }

    private void GetPercent()
    {
        if (!weaponOrOutfit)
        {
            PercentLoad = 0;

            PlayerPrefs.SetInt("PercentLoad", PercentLoad);

            PlayerPrefs.Save();

            PlayerPrefs.SetInt("ItemsShop" + (ItemsType)GetItemID((int)_ItemsPercent), 3);

            PlayerPrefs.Save();

            GameObject.FindObjectOfType<PlayerCtrl>().ChangeItems((PlayerCtrl.ItemsType)_ItemsPercent);

            PlayerPrefs.SetInt("ItemsShop" + (PlayerCtrl.ItemsType)_ItemsPercent, 4);

            PlayerPrefs.SetInt("Unlock" + (PlayerCtrl.ItemsType)_ItemsPercent, 1);

            PlayerPrefs.Save();
        }
        else if (weaponOrOutfit)
        {
            PercentLoad = 0;

            PlayerPrefs.SetInt("PercentLoad", PercentLoad);

            PlayerPrefs.Save();

            PlayerPrefs.SetInt("WeaponShop" + _WeaponPercent, 3);

            PlayerPrefs.Save();

            GameObject.FindObjectOfType<PlayerCtrl>().WeaponSwitch((PlayerCtrl.WeaponType)_WeaponPercent);

            PlayerPrefs.SetInt("WeaponShop" + (PlayerCtrl.WeaponType)_WeaponPercent, 4);

            PlayerPrefs.Save();
        }

        RandomPercent();
    }

    private void RandomPercent()
    {
        if(Random.Range(0, 100) >= 50)
        {
            weaponOrOutfit = true;

            PlayerPrefs.SetInt("WeaponOrOutfit", 1);

            PlayerPrefs.Save();
        }
        else
        {
            weaponOrOutfit = false;

            PlayerPrefs.SetInt("WeaponOrOutfit", 0);

            PlayerPrefs.Save();

        }

        if (!weaponOrOutfit)
        {
            _ItemsPercent = (ItemsType)(Random.Range(Random.Range(0, 20), _itemsType.Count));

            PlayerPrefs.SetInt("ItemsPercent", (int)_ItemsPercent);

            PlayerPrefs.Save();
            
        }
        else
        {
            _WeaponPercent = (WeaponType)(Random.Range(Random.Range(1, 2), _weaponType.Count));

            PlayerPrefs.SetInt("WeaponPercent", (int)_WeaponPercent);

            PlayerPrefs.Save();
        }
    }

    IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(1);

        _sliderAnim.SetTrigger("" + PercentLoad + "%");

        if(PercentLoad == 100)
        {
            GetPercent();
        }
    }

    void ShowPercentImage()
    {
        if (weaponOrOutfit)
        {
            for(int i = 1; i < 2; i++)
            {
                if(i == (int)_WeaponPercent)
                {
                    percentWeaponImage[i].gameObject.SetActive(true);
                }
                else
                {
                    percentWeaponImage[i].gameObject.SetActive(false);
                }
            }

            for(int i = 0; i < 20; i++)
            {
                percentItemImage[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for(int i = 0; i < 20; i++)
            {
                if(i == (int)_ItemsPercent)
                {
                    percentItemImage[i].gameObject.SetActive(true);
                }
                else
                {
                    percentItemImage[i].gameObject.SetActive(false);
                }
            }
            for(int i = 1; i < 2; i++)
            {
                percentWeaponImage[i].gameObject.SetActive(false);
            }
        }
    }

    int GetItemID(int _ID)
    {
        switch (_ID)
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 3;
            case 4:
                return 4;
            case 5:
                return 5;
            case 6:
                return 6;
            case 7:
                return 7;
            case 8:
                return 8;
            case 9:
                return 9;
            case 10:
                return 10;
            case 11:
                return 11;
            case 12:
                return 12;
            case 13:
                return 13;
            case 14:
                return 14;
            case 15:
                return 15;
            case 16:
                return 16;
            case 17:
                return 17;
            case 18:
                return 18;
            default:
                return 19;
        }
    }

}
