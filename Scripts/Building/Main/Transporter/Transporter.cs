using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Transporter : BuildingBase
{

    [SerializeField] private ResourceInfo _resourceToCreate;
    private UpgradeData _upgradeInfo;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private List<GameObject> _upgradeModels;
    [SerializeField] private GameObject _upgradePanel;
    private float _inventorySpeed;
    public float InventorySpeed => _inventorySpeed;
    private bool _work=true;
    public bool Work => _work;
    [SerializeField] private List<Animator> _animators;
    [SerializeField] private AudioSource _upgradeSound;

    private void Start()
    {
        foreach (GameObject model in _upgradeModels)
        {
            model.SetActive(false);

        }
        Synchronize(SaveManager.instance.TransporterSveInfo);
        


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
        Save(SaveManager.instance.TransporterSveInfo);
        Synchronize(SaveManager.instance.TransporterSveInfo);
        UpgradeBool = true;

    }

    private void Synchronize(SaveInfo infoInSave)
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name);
        CurrentLevel = infoInSave.UpgradeLevel;
        ResourceNeedInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].MaxInputStorage;
        if (_upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesForColect.ResourcesInfo.Count > 0)
        {
            NeedResource = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesForColect.ResourcesInfo[0];
        }
        UpgradeInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToUpgrade.ResourcesInfo.Count;
        CreateInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].MaxStorage;
        _inventorySpeed = _upgradeInfo.UpgaradesGrades[CurrentLevel].CreationSpeed;
        if (_upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToCreate.ResourcesInfo.Count > 0)
                 {
                     _resourceToCreate = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToCreate.ResourcesInfo[0];
                 }

        if (infoInSave.UpgradeInventory.InventoryInfo.ResourcesInfo.Count != 0)
        {
            UpgradeInventory = infoInSave.UpgradeInventory;
        }
        

        if (CurrentLevel > 1)
        {
            _upgradeModels[CurrentLevel - 2].SetActive(false);
        }

        if (CurrentLevel > 0)
        {
            _upgradeModels[CurrentLevel - 1].SetActive(true);
        }

        Save(SaveManager.instance.TransporterSveInfo);

    }

    public void StartWork()
    {
        foreach (Animator animator in _animators)
        {
            if (!animator.GetBool("Stop"))
            {
                animator.speed = 1;
            }
            
        }
        _work = true;
    }

    public void StopWork()
    {
        foreach (Animator animator in _animators)
        {
            animator.speed = 0;
        }
        _work = false;
    }
    private void Update()
    {
        CheckUpgrades();
        InventorySpaceCheck();
        WantToUpgrade = _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
        if (WantToUpgrade)
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
        

    }

    private void Save(SaveInfo infoInSave)
    {

        infoInSave.UpgradeLevel = CurrentLevel;
        infoInSave.CreateInventory = CreateInventory;
        infoInSave.ResourceNeedInventory = ResourceNeedInventory;
        infoInSave.UpgradeInventory = UpgradeInventory;
        infoInSave.NeedResource = NeedResource;

    }

   
}
