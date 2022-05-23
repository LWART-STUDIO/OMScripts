using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class FoodCreator : BuildingBase
{
    [SerializeField] private ResourceInfo _resourceToCreate;
    private UpgradeData _upgradeInfo;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private List<GameObject> _upgradeModels;
    [SerializeField] private GameObject _setPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private GameObject _needPanel;
    [SerializeField] private GameObject _needCloud;
    [SerializeField] private BuildingCreateInventoryVisulizer _inventoryCreateVisualizer;
    [SerializeField] private BuildingInventoryVisulizer _inventoryVisualizer;
    [SerializeField] private Transform _getPoint;
    private float _inventorySpeed;
    public float InventorySpeed => _inventorySpeed;
    private bool _work;
    public bool Work => _work;
    [SerializeField] private AudioSource _upgradeSound;

    [SerializeField] private OilPump _oilPump;
    private void Start()
    {
        _oilPump = FindObjectOfType<OilPump>();
        foreach (GameObject model in _upgradeModels)
        {
            model.SetActive(false);
        }

        Synchronize(SaveManager.instance.FoodCreatorSaveInfo);
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

        Save(SaveManager.instance.FoodCreatorSaveInfo);
        Synchronize(SaveManager.instance.FoodCreatorSaveInfo);
        if (CurrentLevel == 1)
        {
            for (int i = 0; i < CreateInventory.MaxCount; i++)
            {
                CreateInventory.Add(_resourceToCreate);
                _inventoryCreateVisualizer.CreateResource(_resourceToCreate.ID);
            }

            for (int i = 0; i < ResourceNeedInventory.MaxCount; i++)
            {
                ResourceNeedInventory.Add(NeedResource);
            }

            ResourceNeedInventory.CurrentSpace = 0;
        }

        UpgradeBool = true;
        Save(SaveManager.instance.FoodCreatorSaveInfo);
    }

    private void Synchronize(SaveInfo infoInSave)
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name);
        if (infoInSave.NeedResource.ID != null)
        {
            CurrentLevel = infoInSave.UpgradeLevel;
        }
        NeedResource = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesForColect.ResourcesInfo[0];
        ResourceNeedInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].MaxInputStorage;
        UpgradeInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToUpgrade.ResourcesInfo.Count;
        CreateInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].MaxStorage;
        _inventorySpeed = _upgradeInfo.UpgaradesGrades[CurrentLevel].CreationSpeed;
        if (_upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToCreate.ResourcesInfo.Count > 0)
        {
            _resourceToCreate = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToCreate.ResourcesInfo[0];
        }

        if (infoInSave.CreateInventory.MaxCount != 0)
        {
            CreateInventory = infoInSave.CreateInventory;
            ResourceNeedInventory = infoInSave.ResourceNeedInventory;
            UpgradeInventory = infoInSave.UpgradeInventory;
            
        }

        if (CurrentLevel != 0)
        {
           // _setPanel.SetActive(true);
            _needPanel.SetActive(true);
            _needCloud.SetActive(true);
        }
        else
        {
            //_setPanel.SetActive(false);
            _needPanel.SetActive(false);
            _needCloud.SetActive(false);
        }

        if (CurrentLevel > 1)
        {
            _upgradeModels[CurrentLevel - 2].SetActive(false);
        }

        if (CurrentLevel > 0)
        {
            _upgradeModels[CurrentLevel - 1].SetActive(true);
            _needPanel.SetActive(true);
            _needCloud.SetActive(true);
        }

        Save(infoInSave);
    }

    private void Update()
    {
        CheckUpgrades();
        InventorySpaceCheck();
        WantToUpgrade = _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
        if (WantToUpgrade&&_oilPump.CurrentLevel>0)
        {
            _upgradePanel.SetActive(true);
            CanUpgrade = UpgradesCountCheck.Length > 0 && UpgradesCountCheck.All(x => x);
        }
        else
        {
            _upgradePanel.SetActive(false);
        }

        if (CanUpgrade && WantToUpgrade)
        {
            Upgrade();
        }

        if (ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count > 0 && !_work &&
            _inventoryVisualizer.ResourcecsInInventory.Count != 0)
        {
            StartCreation();
        }
    }

    private void Save(SaveInfo infoInSave)
    {
        infoInSave.UpgradeLevel = CurrentLevel;
        infoInSave.CreateInventory = CreateInventory;
        infoInSave.ResourceNeedInventory = ResourceNeedInventory;
        infoInSave.UpgradeInventory = UpgradeInventory;
        infoInSave.NeedResource = NeedResource;
    }

    private void StartCreation()
    {
        StartCoroutine(CreateResource());
    }

    private IEnumerator CreateResource()
    {
        while (true)
        {
            if (CreateInventory.CurrentSpace > 0 && ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                _work = true;
                _inventoryVisualizer.GetResource(_getPoint, transform, true);
                ResourceNeedInventory.Remove(NeedResource);
            }
            else
            {
                _work = false;
                StopAllCoroutines();
            }

            yield return new WaitForSecondsRealtime(_inventorySpeed);
            CreateInventory.Add(_resourceToCreate);
            _inventoryCreateVisualizer.CreateResource(_resourceToCreate.ID);
            Save(SaveManager.instance.FoodCreatorSaveInfo);
        }
    }
}