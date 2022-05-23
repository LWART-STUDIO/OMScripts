using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropTrigger : MonoBehaviour
{
    [SerializeField] private AirDrop _airDrop;
    [SerializeField]private bool _canShow;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        AdsManager.UpdatingStateReward += CanOpen;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent(out PlayerInventory playerInventory))
        {
            _animator.SetTrigger("Click");
            GiveResources();

        }
    }

    public void GiveResources()
    {
        AdsManager.ShowRewarded((bool valey) =>
        {
            TimerAds.Reset();
            _airDrop.Open();
            //��� ������ ������
        }, "give_resources");
    }
    private void CanOpen()
    {
        if (AdsManager.IsReadyReward)
        {
            _canShow = true;
        }
        else
        {
            _canShow = false;
            AdsManager.UpdatingStateReward -= CanOpen;
            
        } 
    }
}
