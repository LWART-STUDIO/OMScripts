using UnityEngine;

public class PlasticCreatorAnimationControll : MonoBehaviour
{
    [SerializeField] private PlasticCreator _plasticCreator;
    [SerializeField] private bool _work;
    [SerializeField] private float _speed;
    private Animator _animator;

    private void Update()
    {
        _speed = (1/(_plasticCreator.InventorySpeed*_plasticCreator.BreakSpeed))+0.5f;
        _work = _plasticCreator.Work;
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        else
        {
            _animator.SetBool("Work",true);
            _animator.SetFloat("Speed",_speed);
        }
    }

}
