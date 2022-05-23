using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniPumpBase : BuildingBase
{
    [SerializeField] private OilPump _mainPump;
    [SerializeField] private UpgradeData _upgradeInfo;
    [SerializeField] private float _inventorySpeed;
    public float InventorySpeed => _inventorySpeed;
    [SerializeField] private int _index;
    [SerializeField] private float _bonusSpeed;
    public bool Started;
    [SerializeField] private bool _worked;
    [SerializeField] private BuildingInventoryVisulizer _inventoryVisualizer;
    [SerializeField] private MiniPumpBase _previusPump;
    [SerializeField] private int _mainOilPumpLvlNeed;
    [SerializeField] private List<GameObject> _models;
    public List<GameObject> Models => _models;
    [SerializeField] private GameObject _getZone;
    [SerializeField] private GameObject _upgradeCanvas;
    [SerializeField] private GameObject _upgradeZone;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private Transform _pointToMove;
    [SerializeField] private bool _autoFeed;
    public bool Worked => _worked;

    [SerializeField] private AudioSource _upgradeSound;
    private void Start()
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        Synchronize(SaveManager.instance.MiniPumpSaveInfo[_index]);
    }

    private void Upgrade()
    {
        if (!SaveManager.instance.MuteSounds)
        {
            _upgradeSound.Play();
        }
        RunUpgradeParticals();
        UpgradeInventory.InventoryInfo.ResourcesInfo.Clear();
        CurrentLevel++;
        for (int i = 0; i < ResourceNeedInventory.MaxCount; i++)
        {
            ResourceNeedInventory.Add(NeedResource);
        }

        Save(SaveManager.instance.MiniPumpSaveInfo[_index]);
        Synchronize(SaveManager.instance.MiniPumpSaveInfo[_index]);

        ResourceNeedInventory.CurrentSpace = 0;
        UpgradeBool = true;
    }

    private void Synchronize(SaveInfo infoInSave)
    {
        _upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name);

        if (infoInSave != null)
        {
            CurrentLevel = infoInSave.UpgradeLevel;
        }

        ResourceNeedInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].MaxInputStorage;
        ResourceNeedInventory.CurrentSpace = ResourceNeedInventory.MaxCount;
        NeedResource = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesForColect.ResourcesInfo[0];
        _bonusSpeed = _upgradeInfo.UpgaradesGrades[CurrentLevel].CollectTime;
        _inventorySpeed = _upgradeInfo.UpgaradesGrades[CurrentLevel].CreationSpeed;


        if (infoInSave.CreateInventory != null)
        {
            CreateInventory = infoInSave.CreateInventory;
        }

        if (infoInSave.ResourceNeedInventory != null)
        {
            ResourceNeedInventory = infoInSave.ResourceNeedInventory;
        }

        if (infoInSave.UpgradeInventory != null)
        {
            UpgradeInventory = infoInSave.UpgradeInventory;
        }

        if (infoInSave.NeedResource != null)
        {
            NeedResource = infoInSave.NeedResource;
        }
        
    }

    private void Save(SaveInfo infoInSave)
    {
        infoInSave.CreateInventory = CreateInventory;
        infoInSave.ResourceNeedInventory = ResourceNeedInventory;
        infoInSave.UpgradeInventory = UpgradeInventory;
        infoInSave.NeedResource = NeedResource;
        infoInSave.UpgradeLevel = CurrentLevel;
    }

    private void Update()
    {
        InventorySpaceCheck();
        if (CurrentUpgradesResources != null && CurrentUpgradesResources.Count > 0
                                             && CurrentUpgradesResources[0].ResourceInfo != null
                                             && CurrentUpgradesResources[0].ResourceInfo.ID == null
                                             && CurrentUpgradesResources[0].Count == 0)
        {
        }

        if (_mainPump.CurrentLevel >= _mainOilPumpLvlNeed)
        {
            if (_previusPump != null)
            {
                if (_previusPump.CurrentLevel > 0)
                {
                    CheckUpgrades();
                    WantToUpgrade = _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
                    if (WantToUpgrade)
                    {
                        _upgradeZone.SetActive(true);
                        _upgradeCanvas.SetActive(true);
                        CanUpgrade = UpgradesCountCheck.Length > 0 && UpgradesCountCheck.All(x => x);
                    }
                    else
                    {
                        _upgradeZone.SetActive(false);
                    }

                    if (CanUpgrade && WantToUpgrade)
                    {
                        Upgrade();
                    }

                    if (CurrentLevel < 1)
                    {
                        foreach (GameObject model in _models)
                        {
                            model.SetActive(false);
                        }

                        _getZone.SetActive(false);
                    }
                    else if (_mainPump.CurrentLevel > 0 && CurrentLevel > 0)
                    {
                        if (_mainPump.CurrentLevel == 1)
                        {
                            _models[0].SetActive(true);
                            _models[1].SetActive(false);
                            _models[2].SetActive(false);
                        }
                        else if (_mainPump.CurrentLevel == 2)
                        {
                            _models[0].SetActive(false);
                            _models[1].SetActive(true);
                            _models[2].SetActive(false);
                        }
                        else if (_mainPump.CurrentLevel >= 3)
                        {
                            _models[0].SetActive(false);
                            _models[1].SetActive(false);
                            _models[2].SetActive(true);
                        }

                        _getZone.SetActive(true);
                    }
                }
                else
                {
                    foreach (GameObject model in _models)
                    {
                        model.SetActive(false);
                    }

                    _getZone.SetActive(false);
                    _upgradeCanvas.SetActive(false);
                    _upgradeZone.SetActive(false);
                }
            }
            else
            {
                CheckUpgrades();
                WantToUpgrade = _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
                if (WantToUpgrade)
                {
                    _upgradeZone.SetActive(true);
                    _upgradeCanvas.SetActive(true);
                    CanUpgrade = UpgradesCountCheck.Length > 0 && UpgradesCountCheck.All(x => x);
                }
                else
                {
                    _upgradeZone.SetActive(false);
                }

                if (CanUpgrade && WantToUpgrade)
                {
                    Upgrade();
                }

                if (CurrentLevel < 1)
                {
                    foreach (GameObject model in _models)
                    {
                        model.SetActive(false);
                    }

                    _getZone.SetActive(false);
                }
                else if (_mainPump.CurrentLevel > 0 && CurrentLevel > 0)
                {
                    if (_mainPump.CurrentLevel == 1)
                    {
                        _models[0].SetActive(true);
                        _models[1].SetActive(false);
                        _models[2].SetActive(false);
                    }
                    else if (_mainPump.CurrentLevel == 2)
                    {
                        _models[0].SetActive(false);
                        _models[1].SetActive(true);
                        _models[2].SetActive(false);
                    }
                    else if (_mainPump.CurrentLevel == 3)
                    {
                        _models[0].SetActive(false);
                        _models[1].SetActive(false);
                        _models[2].SetActive(true);
                    }


                    _getZone.SetActive(true);
                }

                
            }
            
        }
        else
        {
            foreach (GameObject model in _models)
            {
                model.SetActive(false);
            }

            _getZone.SetActive(false);
            _upgradeCanvas.SetActive(false);
            _upgradeZone.SetActive(false);
        }

        if (Started && _worked)
        {
            Started = false;
        }

        Save(SaveManager.instance.MiniPumpSaveInfo[_index]);
        if (ResourceNeedInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            if (ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count > 0 && !_worked)
            {
                StartCoroutine(Work());
            }
        }
        if (_autoFeed && ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count <= 1)
        {
            ResourceNeedInventory.Add(NeedResource);
        }
    }

    private IEnumerator Work()
    {
        while (true)
        {
            _worked = true;
            if (ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                _mainPump.BonusSpeed -= _bonusSpeed;
                Started = true;
            }
            else
            {
                Started = false;
                _worked = false;
                StopAllCoroutines();
            }

            yield return new WaitForSecondsRealtime(_inventorySpeed);
            if (ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                if (_inventoryVisualizer.ResourcecsInInventory.Count > 0)
                {
                    _inventoryVisualizer.GetResource(_pointToMove, transform, true);
                }

                ResourceNeedInventory.Remove(NeedResource);
            }

            if (ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count == 0)
            {
                _worked = false;
            }

            _mainPump.BonusSpeed += _bonusSpeed;
        }
    }
}