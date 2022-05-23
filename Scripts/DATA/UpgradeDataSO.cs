using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "UpgaradeData", menuName = "ScriptableObjects/Upgarade Data", order = 1)]
public class UpgradeDataSO : ScriptableObject
{
    [ReorderableList]
    public List<UpgradeData> UpgradeDatas;
    public void OnValidate() {
        Debug.Log(Time.time);
    }
}
[System.Serializable]
public class UpgradeData
{
    public string Name;
    [ReorderableList]
    public List<UpgaradesGrade> UpgaradesGrades;
}
[System.Serializable]
public class UpgaradesGrade
{
    public InventoryInfo ResourcesToUpgrade;
    public InventoryInfo ResourcesForColect;
    public InventoryInfo ResourcesToCreate;
    public int MaxStorage;
    public int MaxInputStorage;
    public float CreationSpeed;
    public float CollectTime;
    public int NeedCount;
    [ReorderableList]
    public List<UpgradeInfo> UpgradeResourses;
}
