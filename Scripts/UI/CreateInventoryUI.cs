using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateInventoryUI : MonoBehaviour
{
    [SerializeField] private AllResousesSO _allResouses;
    [SerializeField] private GameObject _cloud;
    [SerializeField] private GameObject _resurcePanelPrefab;
    [SerializeField] private BuildingBase _building;
    [SerializeField] private bool _createInventoryVisualisation;
    [SerializeField] private bool _needInventoryVisualisation;
    [SerializeField] private bool _upgradeInventoryVisualisation;
    private InventoryBase _createInventory;
    private InventoryBase _needInventory;
    private InventoryBase _upgradeInventory;
    [SerializeField] private List<GameObject> _resoiurcePanels = new List<GameObject>() { };
    [SerializeField] private List<ResourceInfo> _resources = new List<ResourceInfo>() { };
    [SerializeField] private bool _shopInventory = false;
    [SerializeField] private Image _shopImage;
    private Shop _shop;
    [SerializeField] private bool _useProgressBar;
    [SerializeField] private ProgresBarDrawer _progresBar;
    [SerializeField] private int _lastCount;
    private int _currentCount;
    private float _time;
    [SerializeField] private TMP_Text _levelText;
    private OilPump _oilPump;
    [SerializeField] private bool _oilPumpNeed;
    private HorizontalLayoutGroup _horizontalLayoutGroup;
    private void Start()
    {
        _horizontalLayoutGroup = _cloud.GetComponent<HorizontalLayoutGroup>();
        _oilPump = FindObjectOfType<OilPump>();
        if (_shopInventory)
        {
            _shop = _building.GetComponent<Shop>();
        }

        _horizontalLayoutGroup.padding.bottom = 26;
        _horizontalLayoutGroup.spacing = 5;
        _horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
    }

    private void Update()
    {
        _createInventory = _building.CreateInventory;
        _needInventory = _building.ResourceNeedInventory;
        _upgradeInventory = _building.UpgradeInventory;
        if (_createInventoryVisualisation)
        {
            CreateInventoryVisualisation();
        }
        else if (_needInventoryVisualisation)
        {
            NeedInventoryVisualisation();
        }
        else if (_upgradeInventoryVisualisation)
        {
            UpgradeInventoryVisualisation();
        }
        else
        {
            _cloud.SetActive(false);
        }
    }

    private void GenerateResourcePanel(ResourceInfo info)
    {
        GameObject panel = Instantiate(_resurcePanelPrefab, _cloud.transform);
        _resoiurcePanels.Add(panel);
        ResourcePanel resourcePanel = panel.GetComponent<ResourcePanel>();
        _resources.Add(info);
        resourcePanel.Icon.sprite = _allResouses.VisualisationInfo.Find(x => x.Name.ID == info.ID).Icon;
    }

    private void SetNeedPanel(ResourceInfo info)
    {
        if (_resoiurcePanels.Count <= 0)
        {
            GameObject panel = Instantiate(_resurcePanelPrefab, _cloud.transform);
            _resoiurcePanels.Add(panel);
            ResourcePanel resourcePanel = panel.GetComponent<ResourcePanel>();
            _resources.Add(info);
            resourcePanel.Icon.sprite = _allResouses.VisualisationInfo.Find(x => x.Name.ID == info.ID).Icon;
        }
    }

    private void CreateInventoryVisualisation()
    {
        if (_createInventory.InventoryInfo.ResourcesInfo.Count <= 0)
        {
            _cloud.SetActive(false);
        }
        else
        {
            _cloud.SetActive(true);
        }
        _horizontalLayoutGroup.enabled = false;
        _horizontalLayoutGroup.enabled = true;
        for (int i = 0; i < _createInventory.InventoryInfo.ResourcesInfo.Count; i++)
        {
            if (_resources.Exists(x => x.ID == _createInventory.InventoryInfo.ResourcesInfo[i].ID))
            {
                int x = _resources.FindIndex(x => x.ID == _createInventory.InventoryInfo.ResourcesInfo[i].ID);
                ResourcePanel resourcePanel = _resoiurcePanels[x].GetComponent<ResourcePanel>();
                resourcePanel.Count.text = "" + _createInventory.InventoryInfo.ResourcesInfo
                    .FindAll(x => x.ID == _createInventory.InventoryInfo.ResourcesInfo[i].ID).Count;
                
                
            }
            else
            {
                GenerateResourcePanel(_createInventory.InventoryInfo.ResourcesInfo[i]);
            }
           
        }
    }

    private void NeedInventoryVisualisation()
    {
        if (_building.NeedResource == null || _building.NeedResource.ID == null ||
            _building.CurrentLevel < 1 /*_needInventory.InventoryInfo.ResourcesInfo.Count <= 0*/)
        {
            _cloud.SetActive(false);
            _progresBar.gameObject.SetActive(false);
        }
        else
        {
            if (_useProgressBar)
            {
                _progresBar.gameObject.SetActive(true);
            }
            else
            {
                _cloud.SetActive(true);
            }
            
        }

        if (_needInventory.InventoryInfo.ResourcesInfo.Count > 0)
        {
            _horizontalLayoutGroup.enabled = false;
            _horizontalLayoutGroup.enabled = true;
            if (!_useProgressBar)
            {
                for (int i = 0; i < _needInventory.InventoryInfo.ResourcesInfo.Count; i++)
                {
                    if (_resources.Exists(x => x.ID == _needInventory.InventoryInfo.ResourcesInfo[i].ID))
                    {
                        int x = _resources.FindIndex(x => x.ID == _needInventory.InventoryInfo.ResourcesInfo[i].ID);
                        ResourcePanel resourcePanel = _resoiurcePanels[x].GetComponent<ResourcePanel>();
                        resourcePanel.Count.text =
                            _needInventory.InventoryInfo.ResourcesInfo
                                .FindAll(x => x.ID == _needInventory.InventoryInfo.ResourcesInfo[i].ID).Count + "/" +
                            _needInventory.MaxCount;
                    }
                    else
                    {
                        GenerateResourcePanel(_needInventory.InventoryInfo.ResourcesInfo[i]);
                    }
                
                }
            }
            else
            {
                _progresBar.MaximumValue = _building.ResourceNeedInventory.MaxCount;
                _currentCount = _building.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count;
                _progresBar.MinimumValue = 0;
                if (_lastCount != _currentCount)
                {
                    _lastCount = _currentCount;
                    _time = 0;
                }
                

               
            }
           
        }
        else
        {
            if (!_useProgressBar)
            {
                for (int i = 0; i < _resoiurcePanels.Count; i++)
                {
                    ResourcePanel resourcePanel = _resoiurcePanels[i].GetComponent<ResourcePanel>();
                    resourcePanel.Count.text =
                        _needInventory.InventoryInfo.ResourcesInfo
                            .FindAll(x => x.ID == _needInventory.InventoryInfo.ResourcesInfo[i].ID).Count + "/" +
                        _needInventory.MaxCount;
                }
            }
            else
            {
                _progresBar.MaximumValue = _building.ResourceNeedInventory.MaxCount;
                _currentCount = _building.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count;
                _progresBar.MinimumValue = 0;
                if (_lastCount != _currentCount)
                {
                    _lastCount = _currentCount;
                    _time = 0;
                }

                
            }
        }

        if (_building.NeedResource != null && _building.NeedResource.ID != null &&
            !_resources.Exists(x => x.ID == _building.NeedResource.ID))
        {
            SetNeedPanel(_building.NeedResource);
        }

        if (_useProgressBar)
        {
            if (_building.ResourceNeedInventory != null)
            {
                _time += Time.unscaledDeltaTime / _building.ResourceNeedInventory.MaxCount;
            }
            _progresBar.CurrentValue = Mathf.Lerp(_currentCount, 0, _time);
        }
        }
        

    private void UpgradeInventoryVisualisation()
    {
        
        if (_building.UpgradeBool)
        {
            _resources.Clear();
            for (int i = 0; i < _resoiurcePanels.Count; i++)
            {
                if (_resoiurcePanels[i] != null)
                {
                    Destroy(_resoiurcePanels[i]);
                }
            }

            _resoiurcePanels.Clear();
            _building.UpgradeBool = false;
        }

        if (!_oilPumpNeed)
        {
            if (_building.WantToUpgrade && !_building.CanUpgrade)
            {
                if (_levelText != null && _building.CurrentLevel > 0)
                {
                    _levelText.transform.parent.gameObject.SetActive(true);
                }
                else if (_levelText != null && _building.CurrentLevel <= 0)
                {
                    _levelText.transform.parent.gameObject.SetActive(false);
                }
            

            _cloud.SetActive(true);
            if (_levelText != null&&_building.CurrentLevel>0)
            {
                _levelText.text = "LEVEL " + (_building.CurrentLevel + 1);
            }
            var upgradeInfo = _building.UpgradeData.UpgradeDatas.Find(x => x.Name == _building.Name)
                .UpgaradesGrades[_building.CurrentLevel].UpgradeResourses;
            if (upgradeInfo.Count <= 0)
            {
                if (_levelText != null)
                {
                    _levelText.transform.parent.gameObject.SetActive(false);
                }
                _cloud.SetActive(false);
                return;
            }

            for (int i = 0; i < upgradeInfo.Count; i++)
            {
                if (_resources.Exists(x => x.ID == upgradeInfo[i].ResourceInfo.ID))
                {
                    if (!_building.UpgradesCountCheck[i])
                    {
                        _horizontalLayoutGroup.enabled = false;
                        _horizontalLayoutGroup.enabled = true;
                        _resoiurcePanels[i].SetActive(true);
                        int x = _resources.FindIndex(x => x.ID == upgradeInfo[i].ResourceInfo.ID);
                        ResourcePanel resourcePanel = _resoiurcePanels[x].GetComponent<ResourcePanel>();
                        if (_upgradeInventory != null && _upgradeInventory.InventoryInfo != null &&
                            _upgradeInventory.InventoryInfo.ResourcesInfo != null)
                        {
                            int count = upgradeInfo[i].Count -
                                         _upgradeInventory.InventoryInfo.ResourcesInfo
                                             .FindAll(x => x.ID == upgradeInfo[i].ResourceInfo.ID).Count;
                            if (count > 0)
                            {
                                resourcePanel.Count.text =""+ count;
                            }
                            else
                            {
                                resourcePanel.Count.text="" + 0;
                            }
                            
                        }
                    }
                    else
                    {
                        _resoiurcePanels[i].SetActive(false);
                    }
                }
                else
                {
                    GenerateResourcePanel(upgradeInfo[i].ResourceInfo);
                }
                
                
            }
            if (_shopInventory)
            {
                _shopImage.sprite = _shop.UpgradeSprite;
            }
        }
        else
        {
            _resources.Clear();
            for (int i = 0; i < _resoiurcePanels.Count; i++)
            {
                if (_resoiurcePanels[i] != null)
                {
                    Destroy(_resoiurcePanels[i]);
                }
            }

            _resoiurcePanels.Clear();
            _cloud.SetActive(false);
            if (_levelText != null)
            {
                _levelText.transform.parent.gameObject.SetActive(false);
            }
        }
        }
        else
        {
            if (_building.WantToUpgrade && !_building.CanUpgrade&&_oilPump.CurrentLevel>0)
        {
            if (_levelText != null&&_building.CurrentLevel>0)
            {
                _levelText.transform.parent.gameObject.SetActive(true);
            }
            else if(_levelText != null&&_building.CurrentLevel<=0)
            {
                _levelText.transform.parent.gameObject.SetActive(false);
            }
            
            _cloud.SetActive(true);
            if (_levelText != null&&_building.CurrentLevel>0)
            {
                _levelText.text = "LEVEL " + (_building.CurrentLevel + 1);
            }
            var upgradeInfo = _building.UpgradeData.UpgradeDatas.Find(x => x.Name == _building.Name)
                .UpgaradesGrades[_building.CurrentLevel].UpgradeResourses;
            if (upgradeInfo.Count <= 0)
            {
                if (_levelText != null)
                {
                    _levelText.transform.parent.gameObject.SetActive(false);
                }
                _cloud.SetActive(false);
                return;
            }

            for (int i = 0; i < upgradeInfo.Count; i++)
            {
                if (_resources.Exists(x => x.ID == upgradeInfo[i].ResourceInfo.ID))
                {
                    if (!_building.UpgradesCountCheck[i])
                    {
                        _resoiurcePanels[i].SetActive(true);
                        int x = _resources.FindIndex(x => x.ID == upgradeInfo[i].ResourceInfo.ID);
                        ResourcePanel resourcePanel = _resoiurcePanels[x].GetComponent<ResourcePanel>();
                        if (_upgradeInventory != null && _upgradeInventory.InventoryInfo != null &&
                            _upgradeInventory.InventoryInfo.ResourcesInfo != null)
                        {
                            int count = upgradeInfo[i].Count -
                                         _upgradeInventory.InventoryInfo.ResourcesInfo
                                             .FindAll(x => x.ID == upgradeInfo[i].ResourceInfo.ID).Count;
                            if (count > 0)
                            {
                                resourcePanel.Count.text =""+ count;
                            }
                            else
                            {
                                resourcePanel.Count.text="" + 0;
                            }
                            
                        }
                    }
                    else
                    {
                        _resoiurcePanels[i].SetActive(false);
                    }
                }
                else
                {
                    GenerateResourcePanel(upgradeInfo[i].ResourceInfo);
                }
                
                
            }
            if (_shopInventory)
            {
                _shopImage.sprite = _shop.UpgradeSprite;
            }
        }
        else
        {
            _resources.Clear();
            for (int i = 0; i < _resoiurcePanels.Count; i++)
            {
                if (_resoiurcePanels[i] != null)
                {
                    Destroy(_resoiurcePanels[i]);
                }
            }

            _resoiurcePanels.Clear();
            _cloud.SetActive(false);
            if (_levelText != null)
            {
                _levelText.transform.parent.gameObject.SetActive(false);
            }
        }
        }
        
    }
}