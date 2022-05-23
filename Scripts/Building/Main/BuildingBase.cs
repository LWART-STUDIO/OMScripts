
using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BuildingBase : MonoBehaviour
{
    public InventoryBase UpgradeInventory;
    public InventoryBase ResourceNeedInventory;
    public InventoryBase CreateInventory;
    public ResourceInfo NeedResource;
    public UpgradeDataSO UpgradeData;
    public string Name;
    public int CurrentLevel;
    [ReorderableList]
    public List<UpgradeInfo> CurrentUpgradesResources = new List<UpgradeInfo>
    {
        Capacity = 0
    };
    public bool WantToUpgrade;
    public bool CanUpgrade;
    public bool[] UpgradesCountCheck = new bool[] { };
    public float BreakSpeed=1;
    public ParticleSystem BreakParticals;
    public ParticleSystem UpgradeParticals;
    public bool UpgradeBool;
    public GameObject BrokenCanvas;
    public Canvas MaxStorageCanvas;

    
    public void InventorySpaceCheck()
    {
        if (CreateInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            CreateInventory.CurrentSpace = CreateInventory.MaxCount - CreateInventory.InventoryInfo.ResourcesInfo.Count;
            if (CreateInventory.CurrentSpace < 0)
            {
                CreateInventory.RemoveLast();
            }

            if (CreateInventory.CurrentSpace == 0)
            {
                if (MaxStorageCanvas != null)
                {
                    MaxStorageCanvas.gameObject.SetActive(true);
                }
            }
            else
            {
                if (MaxStorageCanvas != null)
                {
                    MaxStorageCanvas.gameObject.SetActive(false);
                }
            }
        }

        if (ResourceNeedInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            ResourceNeedInventory.CurrentSpace = ResourceNeedInventory.MaxCount - ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count;
            if (ResourceNeedInventory.CurrentSpace < 0)
            {
                ResourceNeedInventory.RemoveLast();
            }
        }

        if (UpgradeInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            UpgradeInventory.CurrentSpace =
                UpgradeInventory.MaxCount - UpgradeInventory.InventoryInfo.ResourcesInfo.Count;
        }
    }
    public void CheckUpgrades()
    {
        List<UpgradeInfo> upgradeInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == Name).UpgaradesGrades[CurrentLevel].UpgradeResourses;
        
        if (UpgradesCountCheck.Length != upgradeInfo.Count)
        {
            UpgradesCountCheck = new bool[upgradeInfo.Count];
        }
        if (CurrentUpgradesResources.Count < upgradeInfo.Count)
        {
            CurrentUpgradesResources.Add(new UpgradeInfo());
        }
        else if (CurrentUpgradesResources.Count > upgradeInfo.Count)
        {
            CurrentUpgradesResources.RemoveAt(CurrentUpgradesResources.Count-1);
        }
        if (UpgradeInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            for (int i = 0; i < CurrentUpgradesResources.Count; i++)
            {

                CurrentUpgradesResources[i].ResourceInfo = upgradeInfo[i].ResourceInfo;
                CurrentUpgradesResources[i].Count = UpgradeInventory.InventoryInfo.ResourcesInfo.FindAll(x => x.ID == upgradeInfo[i].ResourceInfo.ID).Count;
                if (CurrentUpgradesResources[i].Count >= upgradeInfo[i].Count)
                {

                    UpgradesCountCheck[i] = true;
                }
                else
                {
                    UpgradesCountCheck[i] = false;

                }
            }
        }
        
        
    }

    public void CheckBreak()
    {
        if (BreakSpeed != 1)
        {
            BrokenCanvas.SetActive(true);
            if (!BreakParticals.isPlaying)
            {
                BreakParticals.Play();
            }
            
        }
       
    }
    public void Repear()
    {
        if (BreakSpeed != 1)
        {
            BreakSpeed = 1;
            BreakParticals.Stop();
            BrokenCanvas.SetActive(false);
        }
    }

    public void RunUpgradeParticals()
    {
        UpgradeParticals.Play();
    }
}
