using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OilPump : BuildingBase
{
     private UpgradeData _upgradeInfo;
    private float _inventorySpeed;
    [SerializeField] private ResourceInfo _resourceToCreate;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private List<GameObject> _upgradeModels;
    [SerializeField] private GameObject _setPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private BuildingCreateInventoryVisulizer _inventoryVisualizer;
    private bool _work;
    public bool Work => _work;
    public float BonusSpeed = 1;
    [SerializeField] private AudioSource _upgradeSound;
    private void Start()
    {
        foreach (GameObject model in _upgradeModels)
        {
            model.SetActive(false);
        }
        Synchronize( SaveManager.instance.OilPumpSaveInfo);

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
        Save( SaveManager.instance.OilPumpSaveInfo);
        Synchronize( SaveManager.instance.OilPumpSaveInfo);
        UpgradeBool = true;
    }
    private void Synchronize(SaveInfo infoInSave)
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name);
        if (infoInSave != null)
        {
            CurrentLevel = infoInSave.UpgradeLevel;
        }
        ResourceNeedInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].MaxInputStorage;
        ResourceNeedInventory.CurrentSpace = ResourceNeedInventory.MaxCount;
        ResourceNeedInventory.InventoryInfo = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesForColect;
        UpgradeInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToUpgrade.ResourcesInfo.Count;
        CreateInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].MaxStorage;
        CreateInventory.CurrentSpace = CreateInventory.MaxCount-CreateInventory.InventoryInfo.ResourcesInfo.Count;
        _inventorySpeed = _upgradeInfo.UpgaradesGrades[CurrentLevel].CreationSpeed;
        _resourceToCreate = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToCreate.ResourcesInfo[0];
        if (infoInSave != null && infoInSave.CreateInventory.MaxCount != 0)
        {
            CreateInventory = infoInSave.CreateInventory;
            ResourceNeedInventory = infoInSave.ResourceNeedInventory;
            UpgradeInventory = infoInSave.UpgradeInventory;
            NeedResource = infoInSave.NeedResource;
        }
        if (CurrentLevel != 0)
        {
            _setPanel.SetActive(true);
            StartCreation();
        }
        if (CurrentLevel > 1)
        {
            _upgradeModels[CurrentLevel - 2].SetActive(false);
        }
        if (CurrentLevel > 0)
        {
            _upgradeModels[CurrentLevel - 1].SetActive(true);
        }

        Save(SaveManager.instance.OilPumpSaveInfo);
    }

    private void Update()
    {
        
        CheckBreak();
        CheckUpgrades();
        InventorySpaceCheck();
        WantToUpgrade = _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
        if (WantToUpgrade)
        {
            _upgradePanel.SetActive(true);
            CanUpgrade = UpgradesCountCheck.Length>0&&UpgradesCountCheck.All(x => x);
        }
        else
        {
            _upgradePanel.SetActive(false);
        }
        if (CanUpgrade && WantToUpgrade)
        {
            Upgrade();
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
        StopAllCoroutines();
        StartCoroutine(CreateResource());
    }
    private IEnumerator CreateResource()
    {
        while (true)
        {
            if (CreateInventory.CurrentSpace > 0)
            {
                _work = true;
            }
            yield return new WaitForSecondsRealtime(_inventorySpeed*BreakSpeed*BonusSpeed);
            if (CreateInventory.CurrentSpace > 0)
            {
                CreateInventory.Add(_resourceToCreate);
                _inventoryVisualizer.CreateResource(_resourceToCreate.ID);
            }
            else
            {
                _work = false;
            }
            Save(SaveManager.instance.OilPumpSaveInfo);
            
        }
    }
    
}