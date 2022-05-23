using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCanvas : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private ResourceInfo _oilResourceInfo;
    [SerializeField] private ResourceInfo _waterResourceInfo;
    [SerializeField] private ResourceInfo _plasticResourceInfo; 
    [SerializeField] private ResourceInfo _metallResourceInfo;

    public void AddOil()
    {
        _playerInventory.GenerateResource(_oilResourceInfo);
    }
    public void AddWater()
    {
        _playerInventory.GenerateResource(_waterResourceInfo);
    }
    public void AddPlastic()
    {
        _playerInventory.GenerateResource(_plasticResourceInfo);
        
    }
    public void AddMetall()
    {
        _playerInventory.GenerateResource(_metallResourceInfo);
    }
}
