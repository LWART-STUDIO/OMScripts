using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInformator : MonoBehaviour
{
    [EditorButton(nameof(LevelEnd), "LevelEnd", activityType: ButtonActivityType.OnPlayMode)]
    [SerializeField] private AllAiControl _aiControl;
    [SerializeField] private CameraControllManager _cameraControllManager;
    [SerializeField] private int _levelNumber;
    [SerializeField] private Shop _shopData;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _oilPump;
    [SerializeField] private GameObject _plasticCreator;
    [SerializeField] private GameObject _foodCreator;
    [SerializeField] private GameObject _waterPump;
    [SerializeField] private GameObject _bur;
    [SerializeField] private GameObject _transporter;
    [SerializeField] private GameObject _mechanic;
    [SerializeField] private GameObject _tanker;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<GameObject> _workers;
    [SerializeField] private GameObject _aiBrains;
    [SerializeField] private Tanker _tankerScript;
    [SerializeField] private GameObject _startResources;
    [SerializeField] private float _timeToEndLevel=15;
    private UpgradeManager _upgradeManager;

    public int LevelNumber => _levelNumber;

    private void Awake()
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _shop.SetActive(true);
        _oilPump.SetActive(false);
        _plasticCreator.SetActive(false);
        _foodCreator.SetActive(false);
        _waterPump.SetActive(false);
        _bur.SetActive(false);
        _transporter.SetActive(false);
        _mechanic.SetActive(false);
        _player.SetActive(false);
        _aiBrains.SetActive(false);
        _tankerScript.enabled = false;
        _startResources.SetActive(false);
        foreach (var worker in _workers)
        {
            worker.SetActive(false);
        }
        if (SaveManager.instance.CurrentLvl != _levelNumber)
        {
            NewLevelStart();
        }
        else
        {
            _startResources.SetActive(true);
            _aiBrains.SetActive(true);
            _tankerScript.enabled = true;
            _player.SetActive(true);
        }
        SaveManager.instance.DoCheckNewLebel();
        AnalyticsEvent.RestoreTimer();
      
    }


    public void NewLevelStart()
    {
        AnalyticsEvent.SendEventStart();
        _cameraControllManager.SwitchToTankerCamera();
        _tanker.GetComponent<Animator>().Play("Ship In");
        StartCoroutine(WaitToStart());
    }

    public void LevelEnd()
    {
        _tanker.GetComponent<Animator>().SetBool("OpenDoor",true);
        _aiControl.GoToTanker();
        _shop.GetComponent<Shop>().enabled = false;
        StartCoroutine(LevelEndWait());

    }
    private void Update()
    {
        if (_shopData.CurrentLevel > -1)
        {
            _oilPump.SetActive(true);

            if (_shopData.CurrentLevel > 0)
            {
                _foodCreator.SetActive(true);
                if (_shopData.CurrentLevel > 1)
                {
                    _waterPump.SetActive(true);
                    if (_shopData.CurrentLevel > 7)
                    {
                        _plasticCreator.SetActive(true);
                        
                        if (_shopData.CurrentLevel > 8 && _plasticCreator.GetComponent<PlasticCreator>().CurrentLevel>0)
                        {
                            _transporter.SetActive(true);

                        }
                        if (_shopData.CurrentLevel > 11)
                        {
                            _mechanic.SetActive(true);
                            if (_shopData.CurrentLevel > 16)
                            {
                                _bur.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSecondsRealtime(7f);
        _startResources.SetActive(true);
        _aiBrains.SetActive(true);
        _tankerScript.enabled = true;
        _player.SetActive(true);
    }

    private IEnumerator LevelEndWait()
    {
        if (LevelNumber != 7)
        {
            SaveManager.instance.NextLevel++;
            AnalyticsEvent.SendEventLevelFinish();
        }
        else
        {
            SaveManager.instance.NextLevel=1;
            SaveManager.instance.TankerSaveInfo.UpgradeLevel = 0;
            SaveManager.instance.CurrentLvl = 0;
            SaveManager.instance.NewLevelStart();
            SaveManager.instance.Save();
            AnalyticsEvent.SendEventLastLevelFinish();

        }

        SaveManager.instance.LevelCounter++;
        _aiControl.GoToTanker();
        yield return new WaitForSecondsRealtime(_timeToEndLevel);
        _aiControl.DisableAllAi();
        _tanker.GetComponent<Animator>().SetBool("Start",true);
        _tanker.GetComponent<Animator>().Play("ShipOut");
        _cameraControllManager.SwitchToTankerCamera();
        StartCoroutine(TakerEndWait());
        
        
    }

    private IEnumerator TakerEndWait()
    {
        yield return new WaitForSecondsRealtime(4);
        LoadingScreen.instance.LoadScene(8);
        //SceneManager.LoadScene(8);
    }
}
