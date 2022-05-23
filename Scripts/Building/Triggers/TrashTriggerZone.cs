using System.Collections.Generic;
using UnityEngine;

public class TrashTriggerZone : MonoBehaviour
{
    [SerializeField] private Transform _pointToMove;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            _animator.SetBool("Open",true);
            playerInventory.ResourceOutOfPlayer += PlaySound;
            playerInventory.WantToWorkWithInventory = true;
            playerInventory.RemoveResorceFromInventoryToTrash(_pointToMove);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            _animator.SetBool("Open",false);
            playerInventory.ResourceOutOfPlayer -= PlaySound;
            playerInventory.WantToWorkWithInventory = false;

        }
    }
    public void PlaySound()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }
}
