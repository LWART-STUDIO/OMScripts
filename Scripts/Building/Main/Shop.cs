using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : BuildingBase
{
    [SerializeField] private UpgradeData _upgradeInfo;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private UpgradeManagerDataSO _levelUpgrades;
    [SerializeField] private UpgradeManagerDataSO _levelMaxUpgrades;
    [SerializeField] private int _counter = 1;
    public int Counter => _counter;
    [AssetPreview()] public Sprite UpgradeSprite;

    [AssetPreview()] [ReorderableList] [SerializeField]
    private List<Sprite> _upgradeSprites;

    [SerializeField] private CameraControllManager _cameraControllManager;
    [SerializeField] private Animator _traderAnimator;
    [SerializeField] private AudioSource _upgradeSound;
    [SerializeField] private Animator _shopAnimator;
    private OilPump _oilPump;
    [SerializeField] private UpgradeTriggerZone _upgradeTriggerZone;
    [SerializeField] private GameObject _upgradeCanvas;
    private void Start()
    {
        _oilPump = FindObjectOfType<OilPump>();
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name);
        if (SaveManager.instance.ShopSaveInfo.UpgradeInventory.InventoryInfo.ResourcesInfo.Count != 0 ||
            SaveManager.instance.ShopCounter > 0)
        {
            _counter = SaveManager.instance.ShopCounter;
            UpgradeInventory = SaveManager.instance.ShopSaveInfo.UpgradeInventory;
            CurrentLevel = SaveManager.instance.ShopSaveInfo.UpgradeLevel;
        }

        if (SaveManager.instance.LevelCounter > 7)
        {
            _levelUpgrades = _levelMaxUpgrades;
        }
        if (CurrentLevel == _levelUpgrades.LevelMaxUpgrade[0].LevelsBought)
        {
            _shopAnimator.SetBool("Close",true);
            _upgradeTriggerZone.gameObject.SetActive(false);
            _upgradeCanvas.SetActive(false);
        }

        if (_oilPump?.CurrentLevel <= 0)
        {
            _shopAnimator.SetBool("Close",true);
            _upgradeTriggerZone.gameObject.SetActive(false);
            _upgradeCanvas.SetActive(false);
        }
    }

    private void Upgrade()
    {
        RunUpgradeParticals();
        _upgradeSound.Play();
        _traderAnimator.SetTrigger("Trade");
        UpgradeInventory.InventoryInfo.ResourcesInfo.Clear();
        CurrentLevel++;
        _counter++;
        if (CurrentLevel == _levelUpgrades.LevelMaxUpgrade[0].LevelsBought)
        {
            _shopAnimator.SetBool("Close",true);
            _upgradeTriggerZone.gameObject.SetActive(false);
            _upgradeCanvas.SetActive(false);
        }
        
        _cameraControllManager.SwitchCamera(_levelUpgrades.LevelMaxUpgrade[_counter].Name);
        if (_levelUpgrades.LevelMaxUpgrade[_counter].Name == "AILevel")
        {
            if (_levelUpgrades.LevelMaxUpgrade[_counter].LevelsBought == 1)
            {
                _cameraControllManager.SwitchCamera("FoodWorker1");
            }
            else if (_levelUpgrades.LevelMaxUpgrade[_counter].LevelsBought == 2)
            {
                _cameraControllManager.SwitchCamera("WaterWorker");
            }
            else if (_levelUpgrades.LevelMaxUpgrade[_counter].LevelsBought == 3)
            {
                _cameraControllManager.SwitchCamera("FoodWorker2");
            }
        
    }
        _upgradeManager.UpgradesData.Find(x => x.Name == _levelUpgrades.LevelMaxUpgrade[_counter].Name).LevelsBought =
            _levelUpgrades.LevelMaxUpgrade[_counter].LevelsBought;
        Save(SaveManager.instance.ShopSaveInfo);
        Synchronize(SaveManager.instance.ShopSaveInfo);
        
        UpgradeBool = true;
    }
    private void Update()
    {
        InventorySpaceCheck();
        WantToUpgrade = _levelUpgrades.LevelMaxUpgrade.Find(x => x.Name == Name).LevelsBought > CurrentLevel;
        UpgradeSprite = _upgradeSprites[_counter + 1];
        if (_oilPump == null)
        {
            _oilPump = FindObjectOfType<OilPump>();
        }
        if (_oilPump?.CurrentLevel > 0&&CurrentLevel < _levelUpgrades.LevelMaxUpgrade[0].LevelsBought)
        {
            _shopAnimator.SetBool("Close",false);
            _upgradeTriggerZone.gameObject.SetActive(true);
            _upgradeCanvas.SetActive(true);
        }
        if (WantToUpgrade)
        {
            
            if (UpgradesCountCheck.Length > 0 && UpgradesCountCheck.All(x => x) &&
                UpgradeInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                Debug.Log(UpgradesCountCheck.All(x => !x));

                CanUpgrade = true;
            }
            else
            {
                CanUpgrade = false;
            }
        }

        if (CanUpgrade && WantToUpgrade)
        {
            if (UpgradeInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                Upgrade();
                CanUpgrade = false;
            }
        }

        Save(SaveManager.instance.ShopSaveInfo);
        CheckUpgrades();
    }

    private void Synchronize(SaveInfo infoInSave)
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name);
        UpgradeInventory.InventoryInfo = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToUpgrade;
        UpgradeInventory.MaxCount = _upgradeInfo.UpgaradesGrades[CurrentLevel].ResourcesToUpgrade.ResourcesInfo.Count;
        if (infoInSave.NeedResource.ID != null)
        {
            CurrentLevel = infoInSave.UpgradeLevel;
        }
    }

    private void Save(SaveInfo infoInSave)
    {
        SaveManager.instance.ShopCounter = _counter;
        infoInSave.UpgradeInventory = UpgradeInventory;
        infoInSave.UpgradeLevel = CurrentLevel;
    }
}