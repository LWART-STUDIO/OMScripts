using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropCanvas : MonoBehaviour
{
    [SerializeField] private AllResousesSO _allResouses;
    [SerializeField] private GameObject _cloud;
    [SerializeField] private GameObject _resurcePanelPrefab;
    [SerializeField] private List<GameObject> _resoiurcePanels = new List<GameObject>() { };
    [SerializeField] private List<ResourceInfo> _resources = new List<ResourceInfo>() { };
    [SerializeField] private LootBoxResources _lootBoxResources;
    [SerializeField] private AirDrop _airDrop;
    
    
    private void GenerateResourcePanel(ResourceInfo info)
    {
        GameObject panel = Instantiate(_resurcePanelPrefab, _cloud.transform);
        _resoiurcePanels.Add(panel);
        ResourcePanel resourcePanel = panel.GetComponent<ResourcePanel>();
        _resources.Add(info);
        resourcePanel.Icon.sprite = _allResouses.VisualisationInfo.Find(x => x.Name.ID == info.ID).Icon;
    }

    private void Update()
    {
        for (int i = 0; i < _lootBoxResources.LootBoxGrades[_airDrop.CurrentIndex].Resources.Count; i++)
            
        {
            if (_resources.Exists(x => x.ID == _lootBoxResources.LootBoxGrades[_airDrop.CurrentIndex].Resources[i].ResourceInfo.ID))
            {
                int x = _resources.FindIndex(x => x.ID == _lootBoxResources.LootBoxGrades[_airDrop.CurrentIndex].Resources[i].ResourceInfo.ID);
                ResourcePanel resourcePanel = _resoiurcePanels[x].GetComponent<ResourcePanel>();
                resourcePanel.Count.text =
                    "" + _lootBoxResources.LootBoxGrades[_airDrop.CurrentIndex].Resources[i].Count;



            }
            else
            {
                GenerateResourcePanel(_lootBoxResources.LootBoxGrades[_airDrop.CurrentIndex].Resources[i].ResourceInfo);
            }
           
        }
    }
}
