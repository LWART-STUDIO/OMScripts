using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelMaxUpgrade", menuName = "ScriptableObjects/LevelMaxUpgrade", order = 1)]
public class UpgradeManagerDataSO : ScriptableObject
{
    [ReorderableList]
    public List<UpgradeManagerData> LevelMaxUpgrade;
 
}
