using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeManagerData> UpgradesData;
    private void Start()
    {
        Load();
    }
    private void Update()
    {
        if (SaveManager.instance.UpgradeData != UpgradesData)
        {
            Save();
        }
    }
    private void Load()
    {
        if (SaveManager.instance.UpgradeData.Count > 0)
        {
            UpgradesData=SaveManager.instance.UpgradeData;
        }

        UpgradesData.Find(x => x.Name == "Tanker").LevelsBought = SaveManager.instance.CurrentLvl;
    }
    private void Save()
    {
        SaveManager.instance.UpgradeData= UpgradesData;
    }
}
