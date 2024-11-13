using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopWeaponState {CantBuy, CanBuy, Select, Equipped}

public enum WeaponType {Hammer, Candy, Knife}

public class WeaponShop : MonoBehaviour
{
    public int[] WeaponPrice = { 1000, 1500, 2000 };

    [SerializeField] private TextMeshProUGUI _CoinsText;

    [SerializeField] private TextMeshProUGUI _CanBuyText;

    [SerializeField] private TextMeshProUGUI _CantBuyText;

    [SerializeField] private Image ShopCurrentWeapon;

    [SerializeField] private Transform _ListWeapon;

    [SerializeField] private Transform WeaponStateButton;

    Dictionary<WeaponType, ShopWeaponState> WeaponShopInfo = new Dictionary<WeaponType, ShopWeaponState>();

    public int WeaponID;

    public void Awake()
    {
        WeaponID = 0;

        for(int i = 0; i < WeaponPrice.Length; i++)
        {
            WeaponShopInfo.Add((WeaponType)i, ShopWeaponState.CantBuy);
        }

        PlayerPrefs.SetInt("WeaponShop" + (WeaponType)((int)(WeaponType.Hammer)), 3);
        PlayerPrefs.Save();
    }

    public void OnEnable()
    {
        for(int i = 0; i < WeaponPrice.Length; i++)
        {
            if(PlayerPrefs.GetInt("WeaponShop" + (WeaponType)i) == 1)
            {
                WeaponShopInfo.Remove((WeaponType)i);

                WeaponShopInfo.Add((WeaponType)i, ShopWeaponState.CantBuy);
            }
            else if(PlayerPrefs.GetInt("WeaponShop" + (WeaponType)i) == 3)
            {
                WeaponShopInfo.Remove((WeaponType)i);

                WeaponShopInfo.Add((WeaponType)i, ShopWeaponState.Select);
            }
            else if (PlayerPrefs.GetInt("WeaponShop" + (WeaponType)i) == 4)
            {
                WeaponShopInfo.Remove((WeaponType)i);

                WeaponShopInfo.Add((WeaponType)i, ShopWeaponState.Equipped);
            }
        }
    }

    public void Start()
    {
        _CoinsText.text = "" + GameManager.Instance.Coins;

        UpdateWeaponShopState();

        showWeaponIMG((WeaponType)WeaponID);

        SetPriceText(WeaponPrice[WeaponID]);

        showState();
    }

    public void NextButton()
    {
        if(WeaponID < 2)
        {
            WeaponID++;

            showWeaponIMG((WeaponType)WeaponID);

            UpdateWeaponShopState();

            SetPriceText(WeaponPrice[WeaponID]);

            showState();
        }
    }

    public void PrevButton()
    {
        if(WeaponID > 0)
        {
            WeaponID--;

            showWeaponIMG((WeaponType)WeaponID);

            UpdateWeaponShopState();

            SetPriceText(WeaponPrice[WeaponID]);

            showState();
        }
    }

    public void showWeaponIMG(WeaponType _weaponType)
    {
        for(int i = 0; i < _ListWeapon.childCount; i++)
        {
            if (i == (int)_weaponType)
            {
                _ListWeapon.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                _ListWeapon.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateWeaponShopState()
    {
        for(int i = 0; i < WeaponPrice.Length; i++)
        {
            if(GameManager.Instance.Coins >= WeaponPrice[i] && WeaponShopInfo[(WeaponType)i] == ShopWeaponState.CantBuy)
            {
                WeaponShopInfo.Remove((WeaponType)i);

                WeaponShopInfo.Add((WeaponType)i, ShopWeaponState.CanBuy);
            }
            else if(GameManager.Instance.Coins < WeaponPrice[i] && WeaponShopInfo[(WeaponType)i] == ShopWeaponState.CanBuy)
            {
                WeaponShopInfo.Remove((WeaponType)i);

                WeaponShopInfo.Add((WeaponType)i, ShopWeaponState.CantBuy);
            }
        }
    }
    public void BuyWeapon()
    {
        if(WeaponShopInfo[(WeaponType)WeaponID] == ShopWeaponState.CanBuy)
        {
            WeaponShopInfo.Remove((WeaponType)WeaponID);

            WeaponShopInfo.Add((WeaponType)WeaponID, ShopWeaponState.Select);

            GameManager.Instance.Coins -= WeaponPrice[(int)((WeaponType)WeaponID)];

            showState();

            UpdateCoins();

            PlayerPrefs.SetInt("Coins", GameManager.Instance.Coins);

            PlayerPrefs.Save();

            PlayerPrefs.SetInt("WeaponShop" + (WeaponType)WeaponID, 3);

            PlayerPrefs.Save();
        }
    }

    public void SelectWeapon()
    {
        if(WeaponShopInfo[(WeaponType)WeaponID] == ShopWeaponState.Select)
        {
            for(int i = 0; i < WeaponShopInfo.Count; i++)
            {
                if(WeaponShopInfo[(WeaponType)i] == ShopWeaponState.Equipped)
                {
                    WeaponShopInfo.Remove((WeaponType)i);

                    WeaponShopInfo.Add((WeaponType)i, ShopWeaponState.Select);

                    PlayerPrefs.SetInt("WeaponShop" + (WeaponType)i, 3);

                    PlayerPrefs.Save();
                }
            }

            GameObject.FindObjectOfType<PlayerCtrl>().WeaponSwitch((PlayerCtrl.WeaponType)WeaponID);

            WeaponShopInfo.Remove((WeaponType)WeaponID);

            WeaponShopInfo.Add((WeaponType)WeaponID, ShopWeaponState.Equipped);

            PlayerPrefs.SetInt("WeaponShop" + (WeaponType)WeaponID, 4);

            PlayerPrefs.Save();

            showState();
        }
    }

    public void showState()
    {
        for(int i = 0; i < WeaponStateButton.transform.childCount; i++)
        {
            if(WeaponShopInfo[(WeaponType)WeaponID] == (ShopWeaponState)i)
            {
                WeaponStateButton.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                WeaponStateButton.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateCoins()
    {
        _CoinsText.text = "" + GameManager.Instance.Coins;
    }

    public void SetPriceText(int price)
    {
        _CanBuyText.text = "" + price;

        _CantBuyText.text = "" + price;
    }
}
