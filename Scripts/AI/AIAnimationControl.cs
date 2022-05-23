using UnityEngine;

public class AIAnimationControl : MonoBehaviour
{
    [SerializeField] private Animator _animator;


    public void HoldingBox()
    {
        _animator.SetBool("BoxHands", true);
        _animator.SetBool("IdleHands",false);
        _animator.SetBool("RunHands",false );
    }
    public void IdleHands()
    {
        _animator.SetBool("BoxHands", false);
        _animator.SetBool("IdleHands", true);
        _animator.SetBool("RunHands", false);
    }
    public void RunHands()
    {
        _animator.SetBool("BoxHands", false);
        _animator.SetBool("IdleHands", false);
        _animator.SetBool("RunHands", true);
    }
    public void Idle()
    {
        _animator.SetBool("IdleLegs", true);
        _animator.SetBool("RunLegs", false);
        if (_animator.GetBool("BoxHands") == false)
        {
            IdleHands();
        }

    }
    public void Run()
    {
        _animator.SetBool("IdleLegs", false);
        _animator.SetBool("RunLegs", true);
        if (_animator.GetBool("BoxHands") == false)
        {
            RunHands();
        }
    }
    public void ResetHands()
    {
        _animator.SetBool("BoxHands", false);
        _animator.SetBool("IdleHands", false);
        _animator.SetBool("RunHands", false);
    }

    public void PumpRun()
    {
        _animator.SetBool("Run",true);
        _animator.SetBool("Tired",false);
        _animator.SetBool("WorkSlow",false);
        _animator.SetBool("Work",false);
    }

    public void PumpWork()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Tired", false);
        _animator.SetBool("WorkSlow", false);
        _animator.SetBool("Work", true);
    }

    public void PumpWorkSlow()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Tired", false);
        _animator.SetBool("WorkSlow", true);
        _animator.SetBool("Work", false);
    }

    public void PumpTired()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Tired", true);
        _animator.SetBool("WorkSlow", false);
        _animator.SetBool("Work", false);
    }

    public void MechanicWork()
    {
        _animator.SetBool("Work",true);
        _animator.SetBool("Idle",false);
        _animator.SetBool("Run",false);
    }

    public void MechanicIdle()
    {
        _animator.SetBool("Work",false);
        _animator.SetBool("Idle",true);
        _animator.SetBool("Run",false);
    }

    public void MechanicRun()
    {
        _animator.SetBool("Work",false);
        _animator.SetBool("Idle",false);
        _animator.SetBool("Run",true);
    }
}
