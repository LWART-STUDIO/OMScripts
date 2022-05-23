using System.Collections.Generic;
using UnityEngine;

public class FoodCreatorFruitSpawner : MonoBehaviour
{
    [SerializeField] private FoodCreator _foodCreator;
    [SerializeField] private List<GameObject> _firstLevelFruits;
    [SerializeField] private List<GameObject> _secondLevelFruits;
    [SerializeField] private List<GameObject> _tirdLevelFruits;
    [SerializeField] private int _currentFruitMustBeActive;

    private void Update()
    {
        int coefficient=  _foodCreator.CreateInventory.MaxCount / _firstLevelFruits.Count;
        if (_foodCreator.CurrentLevel ==1)
        {
            
            _currentFruitMustBeActive = _foodCreator.CreateInventory.InventoryInfo.ResourcesInfo.Count * coefficient;
            Debug.Log(_currentFruitMustBeActive);
            for (int i=0;i<_firstLevelFruits.Count;i++)
            {
                if (i <= _currentFruitMustBeActive-1)
                {
                    if (!_firstLevelFruits[i].activeSelf)
                    {
                        _firstLevelFruits[i].SetActive(true);
                    }
                    
                }
                else
                {
                    _firstLevelFruits[i].SetActive(false);
                }
            }
            
            
        }
        else if (_foodCreator.CurrentLevel == 2)
        {
            _currentFruitMustBeActive = _foodCreator.CreateInventory.InventoryInfo.ResourcesInfo.Count * coefficient;
            for (int i=0;i<_secondLevelFruits.Count;i++)
            {
                if (i <= _currentFruitMustBeActive-1)
                {
                    _secondLevelFruits[i].SetActive(true);
                }
                else
                {
                    _secondLevelFruits[i].SetActive(false);
                }
            }
        }
        else if (_foodCreator.CurrentLevel == 3)
        {
            _currentFruitMustBeActive = _foodCreator.CreateInventory.InventoryInfo.ResourcesInfo.Count * coefficient;
            for (int i=0;i<_tirdLevelFruits.Count;i++)
            {
                if (i <= _currentFruitMustBeActive-1)
                {
                    _tirdLevelFruits[i].SetActive(true);
                }
                else
                {
                    _tirdLevelFruits[i].SetActive(false);
                }
            }
        }
    }
}
