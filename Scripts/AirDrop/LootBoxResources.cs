using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxResources : BuildingBase
{
    
    [SerializeField] private BuildingCreateInventoryVisulizer _inventoryCreateVisualizer;
    public List<LootBoxGrade> LootBoxGrades;
    [SerializeField] private GameObject _setPanel;

   

    public void SpawnResources(int index=0 )
    {
        StartCoroutine(ResourceSpawn(index));
    }

    private IEnumerator ResourceSpawn(int index=0)
    {
        foreach (var t in LootBoxGrades[index].Resources)
        {
            for (int a = 0; a < t.Count;a++)
            {
                CreateInventory.Add(t.ResourceInfo);
                _inventoryCreateVisualizer.CreateResource(t.ResourceInfo.ID);
                yield return new WaitForSecondsRealtime(0.1f);
            }
            
        }
        
    }

    private void Update()
    {
        if (CreateInventory.InventoryInfo.ResourcesInfo.Count <= 0)
        {
            _setPanel.SetActive(false);
        }
        else
        {
            _setPanel.SetActive(true);
        }
    }
}

[Serializable]
public class LootBoxGrade
{
    public List<UpgradeInfo> Resources;
}
