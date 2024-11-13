using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance;

    internal string AnimGUITagOpen = "IsOpen";

    internal string AnimGUITagClose = "IsClose";

    internal const string showSettingDelayTag = "ShowSettingDelay";

    internal const string showWeaponDelayTag = "showWeaponShopDelay";

    internal const string showSkinDelayTag = "showSkinShopDelay";

    internal const string isDanceTag = "IsDance";
    
    internal const string isIdleTag = "IsIdle";


    public List<Image> _imgList = new List<Image>();

    public Button SettingButton;

    public Button HomeButton;

    public Button PlayButton;

    public Button ZombieMode;

    public Button GoldButton;

    public Button SoundButton;

    public Button RemoveAdsButton;

    public Button VibrateButton;

    public Button ExpButton;

    public Button SkinShopButton;

    public Button WeaponShopButton;

    public Button closeButton;

    public Button ContinueButton;

    public GameObject CanvasSkin;

    public GameObject CanvasGameplay;

    public GameObject CanvasFade;

    public GameObject CanvasWinner;

    public GameObject CanvasLose;

    public GameObject CanvasTopButton;

    public GameObject ControlBoard;

    public GameObject CanvasEnemyCount;

    public Toggle changeSound;

    public Toggle changeVibrate;

    public Image BgchangeSound;

    public Image BgchangeVibrate;

    public TextMeshProUGUI EnemyCountNumber;

    public TextMeshProUGUI CoinsText;

    public float timerDelay;

    public GameObject CanvasWeapon;

    public GameObject ShowCase;

    public Transform SpawnKillFeedPos;

    public Animator AnimPlayer;

    //public TextMeshProUGUI PlayerName;

    //public TextMeshProUGUI EnemyName;

    public GameObject KillFeed;
    //public GameObject CanvasPlayButton;
    private void Awake()
    {
        InitializeSingleton();
    }
    void Update()
    {
        if(GameManager.Instance.isWin == true)
        {
            timerDelay += Time.deltaTime;
            if(timerDelay >= 1.2f)
            {
                CanvasWinner.SetActive(true);

                CanvasGameplay.SetActive(false);
            }
        }

        if(GameManager.Instance.isLose == true)
        {
            timerDelay += Time.deltaTime;

            if(timerDelay >= 1.2f)
            {
                CanvasLose.SetActive(true);

                CanvasGameplay.SetActive(false);
            }
        }

        EnemyCountNumber.text = GameManager.Instance.TotalAlive.ToString();
    }

    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayButtonClick()
    {
        GameManager.Instance.isGameActive = true;

        AnimPlayer.SetBool(isDanceTag, false);

        AnimPlayer.SetBool(isIdleTag, true);

        PlayButton.animator.SetTrigger(AnimGUITagClose);

        ZombieMode.animator.SetTrigger(AnimGUITagClose);

        GoldButton.animator.SetTrigger(AnimGUITagClose);

        SoundButton.animator.SetTrigger(AnimGUITagClose);

        RemoveAdsButton.animator.SetTrigger(AnimGUITagClose);

        VibrateButton.animator.SetTrigger(AnimGUITagClose);

        ExpButton.animator.SetTrigger(AnimGUITagClose);

        SkinShopButton.animator.SetTrigger(AnimGUITagClose);

        WeaponShopButton.animator.SetTrigger(AnimGUITagClose);

        SettingButton.gameObject.SetActive(true);

        CanvasEnemyCount.gameObject.SetActive(true);

        Invoke(showSettingDelayTag, 1.2f);

        if (CameraSwitcher.IsActiveCamera(GameManager.Instance.cameraOnMenu))
        {
            CameraSwitcher.SwitchCamera(GameManager.Instance.cameraOnShop);
        }
        else if (CameraSwitcher.IsActiveCamera(GameManager.Instance.cameraOnShop))
        {
            CameraSwitcher.SwitchCamera(GameManager.Instance.cameraOnMenu);
        }
    }

    public void WeaponShopButtonClick()
    {
        PlayButton.animator.SetTrigger(AnimGUITagClose);

        ZombieMode.animator.SetTrigger(AnimGUITagClose);

        SoundButton.animator.SetTrigger(AnimGUITagClose);

        RemoveAdsButton.animator.SetTrigger(AnimGUITagClose);

        VibrateButton.animator.SetTrigger(AnimGUITagClose);

        ExpButton.animator.SetTrigger(AnimGUITagClose);

        SkinShopButton.animator.SetTrigger(AnimGUITagClose);

        WeaponShopButton.animator.SetTrigger(AnimGUITagClose);

        SkinShopButton.interactable = false;

        WeaponShopButton.interactable = false;

        Invoke(showWeaponDelayTag, 0.5f);
    }

    public void SkinShopButtonClick()
    {
        PlayButton.animator.SetTrigger(AnimGUITagClose);

        ZombieMode.animator.SetTrigger(AnimGUITagClose);

        SoundButton.animator.SetTrigger(AnimGUITagClose);

        RemoveAdsButton.animator.SetTrigger(AnimGUITagClose);

        VibrateButton.animator.SetTrigger(AnimGUITagClose);

        ExpButton.animator.SetTrigger(AnimGUITagClose);

        SkinShopButton.animator.SetTrigger(AnimGUITagClose);

        WeaponShopButton.animator.SetTrigger(AnimGUITagClose);

        SkinShopButton.interactable = false;

        WeaponShopButton.interactable = false;

        Invoke(showSkinDelayTag, 0.5f);
    }

    public void closeButtonClick()
    {
        PlayButton.animator.SetTrigger(AnimGUITagOpen);

        ZombieMode.animator.SetTrigger(AnimGUITagOpen);

        SoundButton.animator.SetTrigger(AnimGUITagOpen);

        RemoveAdsButton.animator.SetTrigger(AnimGUITagOpen);

        VibrateButton.animator.SetTrigger(AnimGUITagOpen);

        ExpButton.animator.SetTrigger(AnimGUITagOpen);

        SkinShopButton.animator.SetTrigger(AnimGUITagOpen);

        WeaponShopButton.animator.SetTrigger(AnimGUITagOpen);

        CanvasSkin.gameObject.SetActive(false);

        closeButton.gameObject.SetActive(false);

        SkinShopButton.interactable = true;

        WeaponShopButton.interactable = true;

        CanvasSkin.SetActive(false);

        CanvasWeapon.SetActive(false);
    }

    public void HomeButtonClick()
    {

        CanvasFade.gameObject.SetActive(true);


        StartCoroutine(loadSceneDelay(0));
    }

    public void NextLevel()
    {
        CanvasFade.gameObject.SetActive(true);

        StartCoroutine(loadSceneDelay(1));

    }

    public void ContinueButtonClick()
    {
        ControlBoard.SetActive(false);

        CanvasGameplay.SetActive(true);
    }

    public void settingButtonClick()
    {
        ControlBoard.SetActive(true);

        CanvasGameplay.SetActive(false);

    }

    IEnumerator loadSceneDelay(int buildIndex)
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        yield return new WaitForSeconds(1.2f);

        GameManager.Instance.LoadLevel();
    }

    public void ShowSettingDelay()
    {
        SettingButton.gameObject.SetActive(true);
    }

    public void showSkinShopDelay()
    {
        closeButton.gameObject.SetActive(true);

        CanvasSkin.SetActive(true);
    }
    
    public void showWeaponShopDelay()
    {
        closeButton.gameObject.SetActive(true);

        CanvasWeapon.SetActive(true);
    }

    public void SoundToggle()
    {
        if(changeSound.isOn == true)
        {
            BgchangeSound.rectTransform.DOAnchorPosX(50, 0.25f);
        }
        else
        {
            BgchangeSound.rectTransform.DOAnchorPosX(-50, 0.25f);
        }
    }

    public void VibrateToggle()
    {
        if (changeVibrate.isOn == true)
        {
            BgchangeVibrate.rectTransform.DOAnchorPosX(50, 0.25f);
        }
        else
        {
            BgchangeVibrate.rectTransform.DOAnchorPosX(-50, 0.25f);
        }
    }

    public void DeleteKey()
    {
        PlayerPrefs.DeleteAll();
    }
}
