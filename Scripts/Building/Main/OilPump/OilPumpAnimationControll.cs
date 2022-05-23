using UnityEngine;

public class OilPumpAnimationControll : MonoBehaviour
{
    [SerializeField] private OilPump _oilPump;
    [SerializeField] private bool _work;
    [SerializeField] private float _speed;
    private Animator _animator;

    private void Update()
    {
        _speed = (1/(_oilPump.BonusSpeed*_oilPump.BreakSpeed))-0.5f;
        _work = _oilPump.Work;
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
