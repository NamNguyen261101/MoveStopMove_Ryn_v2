using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<CharacterManager> _listCharacter = new List<CharacterManager>();

    public bool isGameActive;

    public bool isWin;

    public bool isLose;

    [HideInInspector] public const string prefsCoinsTag = "Coins";

    public CinemachineVirtualCamera cameraOnMenu;

    public CinemachineVirtualCamera cameraOnShop;

    public int Coins;

    public int EnemyCount;

    public int TotalAlive;

    public GameObject PoolHammer;

    public GameObject PoolCandy;

    public GameObject PoolKnife;

    public int LevelID;
    private void OnEnable()
    {
        CameraSwitcher.Register(cameraOnMenu);

        CameraSwitcher.Register(cameraOnShop);

        CameraSwitcher.SwitchCamera(cameraOnShop);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(cameraOnMenu);

        CameraSwitcher.Unregister(cameraOnShop);

        CameraSwitcher.SwitchCamera(cameraOnShop);
    }

    private void Awake()
    {
        InitializeSingleton();

        LevelID = 1;

        LevelID = PlayerPrefs.GetInt("LevelID");

        isGameActive = false;

        Coins = PlayerPrefs.GetInt("Coins");

        GUIManager.Instance.CoinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }
    private void Update()
    {
        GUIManager.Instance.CoinsText.text = PlayerPrefs.GetInt("Coins").ToString();

        if(Coins <= 0)
        {
            Coins = 0;

            GUIManager.Instance.CoinsText.text = Coins.ToString();
        }

        if(isGameActive == true)
        {
            PoolHammer.SetActive(true);
            PoolCandy.SetActive(true);
            PoolKnife.SetActive(true);
        }
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

    public void AddCoins(int CoinsToAdd)
    {
        Coins += CoinsToAdd;

        PlayerPrefs.SetInt("Coins", Coins);

        GUIManager.Instance.CoinsText.text = Coins.ToString();
    }

    public void AddCharacter(CharacterManager Character)
    {
        _listCharacter.Add(Character);
    }

    public void LoadLevel()
    {
        LevelID++;

        if(LevelID > 2)
        {
            LevelID = 1;
        }

        PlayerPrefs.SetInt("LevelID", LevelID);

        PlayerPrefs.Save();

        SceneManager.LoadScene("Level" + LevelID);
    }
}
