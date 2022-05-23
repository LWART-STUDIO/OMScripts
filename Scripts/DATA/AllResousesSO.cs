using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ResourcesData", menuName = "ScriptableObjects/All Resources Data", order = 1)]
public class AllResousesSO : ScriptableObject
{
    [ReorderableList]
    public List<VisualisationInfo> VisualisationInfo;
}
[System.Serializable]
public class VisualisationInfo
{
    public ResourceInfo Name;
    public Sprite Icon;
    public GameObject gameObject;

}
