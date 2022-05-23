using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAIBrain : AIMain
{
   [SerializeField] private OilPump _oilPump;
   [SerializeField] private GameObject _joystic;
   [SerializeField] private GameObject _movePoint;
   [SerializeField] private PlayerAnimationController _playerAnimationController;
   [SerializeField] private Animator _animator;
   [SerializeField] private Collider _collider;
   [SerializeField] private GameObject _monoWheel;
   [SerializeField] private GameObject _hoverboard1;
   [SerializeField] private GameObject _hoverboard2;
   [SerializeField] private NavMeshAgent _navMeshAgent;
   [SerializeField] private Transform _tankerPoint;
   [SerializeField] private PlayerInventory _playerInventory;
   [SerializeField] private Rigidbody _playerRigidbody;

   private void Start()
   {
      if (SaveManager.instance.OilPumpSaveInfo.UpgradeLevel <= 0)
      {
         Tasks.Clear();
         _navMeshAgent.enabled = true;
         _playerRigidbody.isKinematic = true;
         SetTask("Start",_movePoint.transform.position);
         _playerAnimationController.enabled = false;
         _joystic.SetActive(false);
         _animator.SetTrigger("Ship");
         _collider.enabled = false;
         _monoWheel.SetActive(false);
         _animator.SetBool("Mooving",true);
         
      }
      else
      {
         _navMeshAgent.enabled = false;
         transform.position = _movePoint.transform.position;

      }
   }

   private void Update()
   {
      DoSomeTask();
      if (Tasks.Count > 0)
      {

      }
   }

   public void EndLevel()
   {  
      _joystic.SetActive(false);
      _collider.enabled = false;
      _playerRigidbody.isKinematic = true;
      Tasks.Clear();
      _playerInventory.Clear();
      GetComponent<Collider>().enabled = false;
      StartCoroutine(Pause());
      
   }

   private IEnumerator Pause()
   {
      yield return null;
      yield return null;
      yield return null;
      yield return null;
      yield return null;
      _navMeshAgent.enabled = true;
      Mover.enabled = true;
      SetTask("GoToTanker",_tankerPoint.position);
      _playerAnimationController.enabled = false;
      _animator.SetBool("HoverBoard",false);
      _animator.SetBool("MonoWheel",false);
      _animator.SetTrigger("Ship");
      _monoWheel.SetActive(false);
      _hoverboard1.SetActive(false);
      _hoverboard2.SetActive(false);
      _animator.SetBool("Mooving",true);
   }
   public void StartLevel()
   {
      _playerRigidbody.isKinematic = false;
      _navMeshAgent.enabled = false;
      RemoveTask("Mooving");
      Mover.enabled = false;
      _playerAnimationController.enabled = true;
      _joystic.SetActive(true);
      _collider.enabled = true;
      this.enabled = false;
   }
}
