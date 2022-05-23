using UnityEngine;

public class BurAnimationControll : MonoBehaviour
{
    [SerializeField] private Bur _bur;
    [SerializeField] private bool _work;
    [SerializeField] private float _speed;
    private Animator _animator;
    [SerializeField] private ParticleSystem _particle;

    private void Update()
    {
        _speed = 1 / (_bur.InventorySpeed*_bur.BreakSpeed) + 0.5f;
        _work = _bur.Work;
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        else
        {
            _animator.SetBool("Work", true);
            _animator.SetFloat("Speed", _speed);
        }

     
        
    }
}
