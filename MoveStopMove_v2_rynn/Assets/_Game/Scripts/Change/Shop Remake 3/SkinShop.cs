using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopType {PantShop, HeadShop, ShieldShop, SkinSetShop}

public enum ShopState {CanBuy, CantBuy, Select, Equipped}

public enum ShopStatus {Lock, Unlock}

public enum ItemsType
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
public class SkinShop : MonoBehaviour
{
    public static SkinShop Instance;

    public int[] ItemsPrice =
    {
        100, 120, 150, 160, 180, 200, 220, 250,

        250, 290, 300, 330, 350, 380, 400, 

        700, 850,

        1500, 2000, 2500
    };

    public TextMeshProUGUI _CoinText;

    public TextMeshProUGUI _CantBuyPriceText;

    public TextMeshProUGUI _CanBuyPriceText;

    public Transform _ShopSkin;

    public Transform[] _ShopBG;

    public Transform[] _ShopScrollView;

    public Image[] _Lock;

    public Image[] _Equipped;

    public GameObject[] _StateButton;

    public Dictionary<ItemsType, ShopState> ItemsShopInfo = new Dictionary<ItemsType, ShopState>();

    public Dictionary<ItemsType, ShopStatus> ItemsStatusInfo = new Dictionary<ItemsType, ShopStatus>();

    public int ItemsID;

    private void Awake()
    {
        Instance = this;

        ItemsID = 0;

        for(int i = 0; i < ItemsPrice.Length; i++)
        {
            ItemsShopInfo.Add((ItemsType)i, ShopState.CantBuy);

            ItemsStatusInfo.Add((ItemsType)i, ShopStatus.Lock);
        }
    }

    public void Start()
    {
        _CoinText.text = "" + GameManager.Instance.Coins;

        PantShopButtonClick();

        GetChambiID();
    }

    private void OnEnable()
    {
        for (int i = 0; i < ItemsPrice.Length; i++)
        {
            if(PlayerPrefs.GetInt("ItemsShop" + (ItemsType)i) == 1)
            {
                ItemsShopInfo.Remove((ItemsType)i);

                ItemsShopInfo.Add((ItemsType)i, ShopState.CantBuy);
            }
            else if(PlayerPrefs.GetInt("ItemsShop" + (ItemsType)i) == 3)
            {
                ItemsShopInfo.Remove((ItemsType)i);

                ItemsShopInfo.Add((ItemsType)i, ShopState.Select);

                if(PlayerPrefs.GetInt("Unlock" + (ItemsType)i) == 1)
                {
                    _Lock[i].gameObject.SetActive(false);
                }
            }

            if(PlayerPrefs.GetInt("ItemsShop" + (ItemsType)i) == 4)
            {
                ItemsShopInfo.Remove((ItemsType)i);

                ItemsShopInfo.Add((ItemsType)i, ShopState.Equipped);

                if (PlayerPrefs.GetInt("Unlock" + (ItemsType)i) == 1)
                {
                    _Lock[i].gameObject.SetActive(false);
                }
                _Equipped[i].gameObject.SetActive(true);
            }
        }

       

        UpdateUnLockStatus();
    }

    //------------Type Shop Button--------------------------------------
    #region ChooseTypeShopButton
    public void PantShopButtonClick()
    {
        GetChambiID();

        OpenShop(ShopType.PantShop);
    }

    public void HeadShopButtonClick()
    {
        GetArrowHeadID();

        OpenShop(ShopType.HeadShop);
    }

    public void ShieldShopButtonClick()
    {
        GetCaptainShield1ID();

        OpenShop(ShopType.ShieldShop);
    }
    public void SkinSetShopButtonClick()
    {
        GetDevilSetID();

        OpenShop(ShopType.SkinSetShop);
    }

    public void OpenShop(ShopType _shopType)
    {
        for(int i = 0; i < _ShopSkin.childCount; i++)
        {
            if(i == (int)_shopType)
            {
                _ShopSkin.GetChild(i).gameObject.SetActive(true);

                _ShopBG[i].gameObject.SetActive(false);

                _ShopScrollView[i].gameObject.SetActive(true);
            }
            else
            {
                _ShopBG[i].gameObject.SetActive(true);

                _ShopScrollView[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Buy And Update Button
    //-------------------Buy Items-----------------------------------
    public void BuyItems()
    {
        if(GameManager.Instance.Coins >= ItemsPrice[ItemsID])
        {
            GameManager.Instance.Coins -= ItemsPrice[ItemsID];

            ItemsShopInfo.Remove((ItemsType)ItemsID);

            ItemsShopInfo.Add((ItemsType)ItemsID, ShopState.Select);

            ItemsStatusInfo.Remove((ItemsType)ItemsID);

            ItemsStatusInfo.Add((ItemsType)ItemsID, ShopStatus.Unlock);

            PlayerPrefs.SetInt("ItemsShop" + (ItemsType)ItemsID, 3);

            PlayerPrefs.Save();
        }

        UpdateButtonState();

        UpdateCoins();

        UpdateUnLockStatus();

        PlayerPrefs.SetInt("Coins", GameManager.Instance.Coins);

        PlayerPrefs.Save();
    }

    public void ChangePrice()
    {
        for (int i = 0; i < ItemsPrice.Length; i++)
        {
            if(i == ItemsID)
            {
                _CantBuyPriceText.text = "" + ItemsPrice[i];

                _CanBuyPriceText.text = "" + ItemsPrice[i];
            }
        }

        UpdateButtonState();
    }
    //---------------UpdateInfo---------------------------------------
    public void UpdateButtonState()
    {
        for(int i = 0; i < 20; i++)
        {
            if(GameManager.Instance.Coins < ItemsPrice[i] && ItemsShopInfo[(ItemsType)i] == ShopState.CanBuy)
            {
                ItemsShopInfo.Remove((ItemsType)i);

                ItemsShopInfo.Add((ItemsType)i, ShopState.CantBuy);
            }
            else if(GameManager.Instance.Coins >= ItemsPrice[i] && ItemsShopInfo[(ItemsType)i] == ShopState.CantBuy)
            {
                ItemsShopInfo.Remove((ItemsType)i);

                ItemsShopInfo.Add((ItemsType)i, ShopState.CanBuy);
            }
        }

        for(int i = 0; i < 4; i++)
        {
            if (ItemsShopInfo[(ItemsType)ItemsID] == (ShopState)i)
            {
                _StateButton[i].SetActive(true);
            }
            else
            {
                _StateButton[i].SetActive(false);
            }
        }
    }

    public void UpdateUnLockStatus()
    {
        for(int i = 0; i < ItemsPrice.Length; i++)
        {
            if(ItemsStatusInfo[(ItemsType)i] == ShopStatus.Unlock)
            {
                _Lock[i].gameObject.SetActive(false);
            }
        }
        PlayerPrefs.SetInt("Unlock" + (ItemsType)ItemsID, 1);

        PlayerPrefs.Save();
    }

    public void UpdateCoins()
    {
        _CoinText.text = "" + GameManager.Instance.Coins;
    }
    #endregion

    #region Equip Button

    public void Equip()
    {
        for(int i = 0; i < ItemsPrice.Length; i++)
        {
            if(ItemsShopInfo[(ItemsType)i] == ShopState.Equipped)
            {
                ItemsShopInfo.Remove((ItemsType)i);

                ItemsShopInfo.Add((ItemsType)i, ShopState.Select);

                _Equipped[i].gameObject.SetActive(false);

                PlayerPrefs.SetInt("ItemsShop" + (ItemsType)i, 3);

                PlayerPrefs.Save();
            }
        }

        if(ItemsShopInfo[(ItemsType)ItemsID] == ShopState.Select)
        {
            ItemsShopInfo.Remove((ItemsType)ItemsID);

            ItemsShopInfo.Add((ItemsType)ItemsID, ShopState.Equipped);

            _Equipped[ItemsID].gameObject.SetActive(true);

            GameObject.FindObjectOfType<PlayerCtrl>().ChangeItems((PlayerCtrl.ItemsType)ItemsID);

            PlayerPrefs.SetInt("ItemsShop" + (ItemsType)ItemsID, 4);

            PlayerPrefs.Save();
        }

        UpdateButtonState();
    }


    #endregion

    //-------------Get Id Item---------------------------------------
    #region Pants ID 
    public void GetChambiID()
    {
        ItemsID = 0;

        ChangePrice();
    }
    public void GetComyID()
    {
        ItemsID = 1;

        ChangePrice();
    }
    public void GetDabaoID()
    {
        ItemsID = 2;

        ChangePrice();
    }
    public void GetBatmanID()
    {
        ItemsID = 3;

        ChangePrice();
    }
    public void GetPokemonID()
    {
        ItemsID = 4;

        ChangePrice();
    }
    public void GetOnionID()
    {
        ItemsID = 5;

        ChangePrice();
    }
    public void GetVantimID()
    {
        ItemsID = 6;

        ChangePrice();
    }
    public void GetRainbowID()
    {
        ItemsID = 7;

        ChangePrice();
    }
    #endregion

    #region Head ID
    public void GetArrowHeadID()
    {
        ItemsID = 8;

        ChangePrice();
    }
    public void GetCowboyID()
    {
        ItemsID = 9;

        ChangePrice();
    }
    public void GetPoliceID()
    {
        ItemsID = 10;

        ChangePrice();
    }
    public void GetSnapID()
    {
        ItemsID = 11;

        ChangePrice();
    }
    public void GetTaingheID()
    {
        ItemsID = 12;

        ChangePrice();
    }
    public void GetTaithoID()
    {
        ItemsID = 13;

        ChangePrice();
    }
    public void GetMuromID()
    {
        ItemsID = 14;

        ChangePrice();
    }

    #endregion

    #region Shield ID
    public void GetCaptainShield1ID()
    {
        ItemsID = 15;

        ChangePrice();
    }
    public void GetCaptainShield2ID()
    {
        ItemsID = 16;

        ChangePrice();
    }
    #endregion

    #region SkinSet ID
    public void GetDevilSetID()
    {
        ItemsID = 17;

        ChangePrice();
    }
    public void GetAngelSetID()
    {
        ItemsID = 18;

        ChangePrice();
    }
    public void GetWitchSetID()
    {
        ItemsID = 19;

        ChangePrice();
    }


    #endregion
}
