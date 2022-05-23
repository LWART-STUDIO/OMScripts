 using System.Collections.Generic;
using UnityEngine;

public class MechanicAIBrain : MonoBehaviour
{
    [SerializeField] private List<BuildingBase> _buildings;
    [SerializeField] private List<GameObject> _pointsToGo;
    public bool Work=false;
    [SerializeField] private MechanicWorkerAI _mechanic;
    [SerializeField] private GameObject _restPoint;
    [SerializeField] private MechanicSpawner _mechanicSpawner;
    public bool CanWork;

    private void Start()
    {
        CanWork = _mechanicSpawner.Worked;
        _mechanic.SetTask("Rest",_restPoint.transform.position);
        _mechanic.Timer = 0;
        Work = false;
    }

    private void Update()
    {
        
        CanWork = _mechanicSpawner.Worked;
        if (_mechanicSpawner.CurrentLevel > 0)
        {
            _mechanic.gameObject.SetActive(true);
            if (_mechanic.Repeir && _mechanic.Tasks.Count == 0)
            {
                _mechanic.SetTask("Rest",_restPoint.transform.position);
                _mechanic.Timer = 0;
                Work = false;
                _mechanic.Repeir = false;
            }
            if (!Work&&CanWork)
            {
                for (int i = 0; i < _buildings.Count; i++)
                {
                    BuildingBase building = _buildings[i];
                    if (building.BreakSpeed != 1)
                    {
                        if (_mechanic.Tasks.Count>0&&_mechanic.Tasks[0].Name == "Rest")
                        {
                            _mechanic.RemoveTask("Rest");
                        }
                        
                         Work = true;
                        _mechanic.SetTask(building.Name,_pointsToGo[i].transform.position,_pointsToGo[i].transform.rotation);
                        _mechanic.Timer = 0;
                        break;
                    }
                }
            }
            else if (_mechanic.WorkDone && _mechanic.Tasks.Count == 0)
            {
                
                _mechanic.SetTask("Rest",_restPoint.transform.position);
                _mechanic.Timer = 0;
                Work = false;
            }

        }
        else
        {
            _mechanic.gameObject.SetActive(false);
        }
    }
}
