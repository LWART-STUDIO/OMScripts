using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAIBrain : MonoBehaviour
{
    [SerializeField] private List<MiniPumpBase> _miniPumps;
    [SerializeField] private List<PumpWorkerAI> _pumpWorkers;
    [SerializeField] private List<ProgresBarDrawer> _miniPumpProgressBarDrawers;
    [SerializeField] private List<GameObject> _pointToMove;
    [SerializeField] private OilPump _mainPump;
    private readonly int _work = Animator.StringToHash("Work");
    private readonly int _workSlow = Animator.StringToHash("WorkSlow");
    private readonly int _idle = Animator.StringToHash("Idle");
   
    

    private void Update()
    {
        if (_mainPump.CurrentLevel > 0)
        {
             for (int i = 0; i < _pumpWorkers.Count; i++)
        {
            PumpWorkerAI pumpWorker = _pumpWorkers[i];
            if (pumpWorker.Work == false&&pumpWorker.Tasks.Count<=0)
            {
                //pumpWorker.transform.SetParent(_miniPumps[i].transform);
                pumpWorker.SetTask("GoToMiniOilPump", _pointToMove[i].transform.position);
            }

            if (pumpWorker.Mover.IsMoving)
            {
                pumpWorker.AiAnimationControl.PumpRun();
            }
             if (pumpWorker.Work && !pumpWorker.Mover.IsMoving
                            && pumpWorker.Tasks.Count > 0
                            && _miniPumps[i].ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count >= 1
                            && _miniPumpProgressBarDrawers[i].CurrentValue > 0.5)
        {
            pumpWorker.transform.rotation = _pointToMove[i].transform.rotation;
            pumpWorker.AiAnimationControl.PumpWork();
            if (_mainPump.CurrentLevel < 4)
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 1].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, true);
                miniPumpAnimator.SetBool(_workSlow, false);
                miniPumpAnimator.SetBool(_idle, false);
            }
            else
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 2].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, true);
                miniPumpAnimator.SetBool(_workSlow, false);
                miniPumpAnimator.SetBool(_idle, false);
            }
            
            
        }
        else if (pumpWorker.Work && !pumpWorker.Mover.IsMoving
                                 && pumpWorker.Tasks.Count > 0
                                 && _miniPumps[i].ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count == 1
                                 && _miniPumpProgressBarDrawers[i].CurrentValue <= 0.9)
        {
            pumpWorker.transform.rotation = _pointToMove[i].transform.rotation;
            pumpWorker.AiAnimationControl.PumpWorkSlow();
            if (_mainPump.CurrentLevel < 4)
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 1].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, false);
                miniPumpAnimator.SetBool(_workSlow, true);
                miniPumpAnimator.SetBool(_idle, false);
            }
            else
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 2].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, false);
                miniPumpAnimator.SetBool(_workSlow, true);
                miniPumpAnimator.SetBool(_idle, false);
            }

        }
        else if (pumpWorker.Work && !pumpWorker.Mover.IsMoving
                                 && pumpWorker.Tasks.Count > 0
                                 && _miniPumps[i].ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count == 0
                                 && _miniPumpProgressBarDrawers[i].CurrentValue == 0)
        {
            pumpWorker.transform.rotation = _pointToMove[i].transform.rotation;
            pumpWorker.AiAnimationControl.PumpTired();
            if (_mainPump.CurrentLevel < 4)
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 1].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, false);
                miniPumpAnimator.SetBool(_workSlow, false);
                miniPumpAnimator.SetBool(_idle, true);
            }
            else
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 2].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, false);
                miniPumpAnimator.SetBool(_workSlow, false);
                miniPumpAnimator.SetBool(_idle, true);
            }

            
        }
        else if (!pumpWorker.Work && !pumpWorker.Mover.IsMoving
                                 && pumpWorker.Tasks.Count > 0
                                 && _miniPumps[i].ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count == 0
                                 && _miniPumpProgressBarDrawers[i].CurrentValue == 0)
        {
            pumpWorker.transform.rotation = _pointToMove[i].transform.rotation;
            pumpWorker.AiAnimationControl.PumpTired();
            if (_mainPump.CurrentLevel < 4)
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 1].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, false);
                miniPumpAnimator.SetBool(_workSlow, false);
                miniPumpAnimator.SetBool(_idle, true);
            }
            else
            {
                Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 2].GetComponent<Animator>();
                miniPumpAnimator.SetBool(_work, false);
                miniPumpAnimator.SetBool(_workSlow, false);
                miniPumpAnimator.SetBool(_idle, true);
            }

            
        }
        else
        {
            if (_mainPump.CurrentLevel > 0)
            {
                if (_mainPump.CurrentLevel < 4)
                {
                    Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 1].GetComponent<Animator>();
                    miniPumpAnimator.SetBool(_work, false);
                    miniPumpAnimator.SetBool(_workSlow, false);
                    miniPumpAnimator.SetBool(_idle, true);
                }
                else
                {
                    Animator miniPumpAnimator = _miniPumps[i].Models[_mainPump.CurrentLevel - 2].GetComponent<Animator>();
                    miniPumpAnimator.SetBool(_work, false);
                    miniPumpAnimator.SetBool(_workSlow, false);
                    miniPumpAnimator.SetBool(_idle, true);
                }


            }
                
            
            
        }
        }
        }
       
    }
    
}