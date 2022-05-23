using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMain : MonoBehaviour
{
    [SerializeField] private InventoryBase _inventory;
    public InventoryBase Inventory => _inventory;
    [SerializeField] private BackPackVisulizer _backPackVisulizer;
    public AIAnimationControl AiAnimationControl;
    [SerializeField] private AIMover _mover;
    public AIMover Mover => _mover;
    [SerializeField] private bool _canCarryItrems;
    [SerializeField] private string _nameOfWork;
    [SerializeField] private List<AiTask> _tasks;
    public List<AiTask> Tasks => _tasks;
    [SerializeField] private bool _work = false;
    public bool Work => _work;
    public float Timer;
    public string NameOfWork => _nameOfWork;
    private Vector3 _pointOfWork;

    public void CheckSpace()
    {
        _inventory.CurrentSpace = _inventory.MaxCount - _inventory.InventoryInfo.ResourcesInfo.Count;
    }
    public void FoodWork()
    {
        if (Inventory.InventoryInfo.ResourcesInfo.Count > 0)
        {
            RemoveTask("FoodCreator");
        }
        else
        {
            if (_tasks.Exists(x => x.Name != "FoodCreator"))
            {
                string name = _tasks.Find(x => x.Name != "FoodCreator").Name;
                RemoveTask(name);
            }
        }
    }
    public void WaterWork()
    {
        if (Inventory.InventoryInfo.ResourcesInfo.Count > 0)
        {
            RemoveTask("WaterPump");
        }
        else
        {
            if (_tasks.Exists(x => x.Name != "WaterPump"))
            {
                string name = _tasks.Find(x => x.Name != "WaterPump").Name;
                RemoveTask(name);
            }
        }
    }

    public void DoSomeTask()
    {
        if (_tasks.Count > 0)
        {
            DoTask(_tasks[0].Name);
        }
    }

    public void TimerCounter()
    {
        Timer += Time.unscaledDeltaTime;
    }

    public void FoodAnimationConrol()
    {
        if (_canCarryItrems)
        {
            if (_inventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                AiAnimationControl.HoldingBox();
            }
            else if ((_inventory.InventoryInfo.ResourcesInfo.Count == 0))
            {
                AiAnimationControl.ResetHands();
            }

            if (_mover.IsMoving)
            {
                AiAnimationControl.Run();
            }
            else if (_mover.IsMoving == false)
            {
                AiAnimationControl.Idle(); 
                //AiAnimationControl.IdleHands();
            }
        }

        if (_mover.IsMoving)
        {
            AiAnimationControl.Run();
        }
        else
        {
            AiAnimationControl.Idle();
        }
    }

    public void SetTask(string nameOfWork, Vector3 position, Quaternion rotation = default )
    {
        _tasks.Add(new AiTask() {Name = nameOfWork, Position = position,Rotation = rotation});
        //throw new System.NotImplementedException();
        
    }

    public void RemoveTask(string name)
    {
        _tasks.Remove(_tasks.Find(x => x.Name == name));
        _work = false;
    }

    public void DoTask(string name)
    {
        AiTask task = _tasks.Find(x => x.Name == name);
        SetWork(task.Name, task.Position);
        _work = true;
    }

    private void SetWork(string name, Vector3 position)
    {
        _nameOfWork = name;
        _pointOfWork = position;
        _mover.SetPoint(position);
    }

    public void AddResource(ResourceInfo resourceInfo,
        BuildingCreateInventoryVisulizer buildingInventoryVisulizer = null, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null)
    {
        StartCoroutine(AddResourceWork(resourceInfo, buildingInventoryVisulizer, inventoryToRemove, inventoryToAdd));
    }

    public void RemoveResource(ResourceInfo resourceInfo, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null, BuildingInventoryVisulizer visulizer = null)
    {
        
        StartCoroutine(RemoveResourceWork(resourceInfo, inventoryToRemove, inventoryToAdd, visulizer));
    }

    private IEnumerator AddResourceWork(ResourceInfo resourceInfo,
        BuildingCreateInventoryVisulizer buildingInventoryVisulizer = null, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null)
    {
        while (_inventory.InventoryInfo.ResourcesInfo.Count == 0)
        {
            if (_inventory.CurrentSpace > 0)
            {
                if (resourceInfo.ID != null)
                {
                    if (inventoryToRemove != null && inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                    {
                        if (buildingInventoryVisulizer != null)
                        {
                            if (buildingInventoryVisulizer.ResourcecsInInventory.Count > 0)
                            {
                                GameObject resource = buildingInventoryVisulizer.GetResource(
                                    _backPackVisulizer.ResourcePoints[_inventory.InventoryInfo.ResourcesInfo.Count],
                                    _backPackVisulizer.transform);
                                _backPackVisulizer.ResourcecsInInventory.Add(resource.transform);
                                if (inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x =>
                                        x.ID == resourceInfo.ID))
                                {
                                    inventoryToRemove.Remove(resourceInfo);
                                }

                                if (inventoryToAdd != null)
                                {
                                    inventoryToAdd.Add(resourceInfo);
                                }

                                _inventory.Add(resourceInfo);
                            }
                        }
                        else
                        {
                            if (inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                            {
                                inventoryToRemove.Remove(resourceInfo);
                            }

                            if (inventoryToAdd != null)
                            {
                                inventoryToAdd.Add(resourceInfo);
                            }

                            _inventory.Add(resourceInfo);
                        }
                    }
                }
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private IEnumerator RemoveResourceWork(ResourceInfo resourceInfo, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null, BuildingInventoryVisulizer visulizer = null)
    {
        while (_inventory.InventoryInfo.ResourcesInfo.Count > 0)
        {
            
            if (_inventory.CurrentSpace < _inventory.MaxCount)
            {
                if (_inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                {
                    if (inventoryToRemove != null)
                    {
                        if (inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                        {
                            
                            inventoryToRemove.Remove(resourceInfo);
                        }
                    }
                    
                    if (inventoryToAdd != null)
                    {
                        
                        if (inventoryToAdd.CurrentSpace > 0)
                        {
                            
                            if (visulizer != null)
                            {
                                
                                if (_backPackVisulizer.ResourcecsInInventory.Exists((x =>
                                        x.GetComponent<ResourceMover>().ID == resourceInfo.ID)))
                                {
                                    
                                    GameObject res = _backPackVisulizer.GetResource(resourceInfo.ID,
                                        visulizer.ResourcePoints[visulizer.ResourcecsInInventory.Count],
                                        visulizer.InventoryParent.transform);

                                    visulizer.ResourcecsInInventory.Add(res.transform);
                                }
                            }
                            
                            inventoryToAdd.Add(resourceInfo);
                            _inventory.Remove(resourceInfo);

                            //_backPackVisulizer.GetResource(resourceInfo.ID,)
                        }
                    }
                    else
                    {
                        if (visulizer != null)
                        {
                            if (_backPackVisulizer.ResourcecsInInventory.Exists((x =>
                                    x.GetComponent<ResourceMover>().ID == resourceInfo.ID)))
                            {
                                GameObject res = _backPackVisulizer.GetResource(resourceInfo.ID,
                                    visulizer.ResourcePoints[visulizer.ResourcecsInInventory.Count],
                                    visulizer.InventoryParent.transform);
                                visulizer.ResourcecsInInventory.Add(res.transform);
                            }
                        }

                        _inventory.Remove(resourceInfo);
                    }
                }
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }
}

[System.Serializable]
public class AiTask
{
    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
}