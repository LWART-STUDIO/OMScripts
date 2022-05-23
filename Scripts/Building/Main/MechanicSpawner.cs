using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MechanicSpawner : BuildingBase
{
    [SerializeField] private UpgradeData _upgradeInfo;
    [SerializeField] private float _inventorySpeed;
    public float InventorySpeed => _inventorySpeed;
    [SerializeField] private float _bonusSpeed;
    public bool Started;
    [SerializeField] private GameObject _wantEatCanvas;
    [SerializeField] private bool _worked;
    [SerializeField] private BuildingInventoryVisulizer _inventoryVisualizer;
    [SerializeField] private List<GameObject> _models;
    [SerializeField] private GameObject _getZone;
    [SerializeField] private GameObject _upgradeCanvas;
    [SerializeField] private GameObject _upgradeZone;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private Transform _pointToMove;
    [SerializeField] private AudioSource _upgradeSound;
    public bool Worked => _worked;
    [SerializeField] private OilPump _oilPump;
    private void Start()
    {
        _oilPump = FindObjectOfType<OilPump>();
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        Synchronize(SaveManager.instance.MechanicSaveInfo);
        Save(SaveManager.instance.MechanicSaveInfo);

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

        Save(SaveManager.instance.MechanicSaveInfo);
        Synchronize(SaveManager.instance.MechanicSaveInfo);
        ResourceNeedInventory.CurrentSpace = 0;
        for (int i = 0; i < ResourceNeedInventory.MaxCount; i++)
        {
            ResourceNeedInventory.Add(NeedResource);
        }
        Save(SaveManager.instance.MechanicSaveInfo);
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
        ResourceNeedInventory.CurrentSpace = ResourceNeedInventory.MaxCount-ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count;
        NeedResource = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesForColect.ResourcesInfo[0];
        _bonusSpeed = _upgradeInfo.UpgaradesGrades[CurrentLevel].CollectTime;
        _inventorySpeed = _upgradeInfo.UpgaradesGrades[CurrentLevel].CreationSpeed;

        if (infoInSave != null)
        {
            CreateInventory = infoInSave.CreateInventory;
            ResourceNeedInventory = infoInSave.ResourceNeedInventory;
            UpgradeInventory = infoInSave.UpgradeInventory;
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
                                             && CurrentUpgradesResources[0].ResourceInfo.ID == null
                                             && CurrentUpgradesResources[0].Count==0)
        {
            Synchronize(SaveManager.instance.MechanicSaveInfo);
        }
       CheckUpgrades();
       WantToUpgrade = _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
       if (WantToUpgrade&&_oilPump.CurrentLevel>0)
       {
           _upgradeZone.SetActive(true);
           _upgradeCanvas.SetActive(true);
           CanUpgrade = UpgradesCountCheck.Length>0&&UpgradesCountCheck.All(x => x);
       }
       else
       {
           _upgradeZone.SetActive(false);
       }
       if (CanUpgrade && WantToUpgrade)
       {
           Upgrade();
       }
       if (Started&& _worked)
       {
           Started=false;
       }

       if (CurrentLevel < 1)
       {
           _models[0].SetActive(false);
           _getZone.SetActive(false);
       }
       else
       {
           _models[0].SetActive(true);
           _getZone.SetActive(true);
       }
       Save(SaveManager.instance.MechanicSaveInfo);
       if (ResourceNeedInventory is {InventoryInfo: {ResourcesInfo: { }}})
       {
           
           if (ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count > 0 && !_worked)
           {
               StartCoroutine(Work());
           }
       }

    }
    private IEnumerator Work()
    {
        while (true)
        {
            _worked = true;
            if (ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
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
        }
        

    }


}
