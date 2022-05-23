using UnityEngine;
using System.Linq;

public class Tanker : BuildingBase
{
    private UpgradeData _upgradeInfo;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private TankerAnimationControll _tankerAnimationControll;
    [SerializeField] private LevelInformator _levelInformator;
    [SerializeField] private Bur _bur;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private StartResourcecs _startResources;
    
    private void Start()
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        if (SaveManager.instance.LevelCounter > 7)
        {
            CurrentLevel = 6;
            _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought = 7;
        }
        Synchronize(SaveManager.instance.TankerSaveInfo);
        _tankerAnimationControll.OpenDorForSec(6f);
        
            
           
            
        

    }
    private void Upgrade()
    {
        _tankerAnimationControll.OpenDorForSec(15f);
        UpgradeInventory.InventoryInfo.ResourcesInfo.Clear();
        CurrentLevel++;
        Save(SaveManager.instance.TankerSaveInfo);
        
        if (SaveManager.instance.LevelCounter < 8)
        {
            if (CurrentLevel > _levelInformator.LevelNumber-1)
            {
                _levelInformator.LevelEnd();
                _upgradePanel.gameObject.SetActive(false);
            }
        }
        else
        {
            if (CurrentLevel > 6)
            {
                _levelInformator.LevelEnd();
                _upgradePanel.gameObject.SetActive(false);
            }
        }
        Synchronize(SaveManager.instance.TankerSaveInfo);
        
        
    }
    private void Synchronize(SaveInfo infoInSave)
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name);
        if (infoInSave.UpgradeInventory != null)
        {
            if (SaveManager.instance.LevelCounter < 8)
            {
                CurrentLevel = infoInSave.UpgradeLevel;
            }
           
            
        }
        UpgradeInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToUpgrade.ResourcesInfo.Count;

        if (infoInSave.UpgradeInventory!=null)
        {
            CreateInventory = infoInSave.CreateInventory;
            ResourceNeedInventory = infoInSave.ResourceNeedInventory;
            UpgradeInventory = infoInSave.UpgradeInventory;
            NeedResource = infoInSave.NeedResource;
        }

        Save(infoInSave);
    }

    private void Update()
    {
        CheckUpgrades();
        InventorySpaceCheck();
        WantToUpgrade = _upgradeManager.UpgradesData.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
        if (WantToUpgrade&&!_startResources.gameObject.activeSelf)
        {
            _canvas.SetActive(true);
            _upgradePanel.SetActive(true);
            CanUpgrade = UpgradesCountCheck.Length>0&&UpgradesCountCheck.All(x => x);
        }
        else
        {
            _canvas.SetActive(false);
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
