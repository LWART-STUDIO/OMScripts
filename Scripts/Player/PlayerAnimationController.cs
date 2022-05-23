using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private GameObject _monoWheel;
    [SerializeField] private GameObject _hoverBoard1;
    [SerializeField] private GameObject _hoverBoard2;
    [SerializeField] private int _currentLvl;
    [SerializeField] private GameObject _walkEffect;

    private void Update()
    {
        _currentLvl = SaveManager.instance.PlayerSaveInfo.UpgradeLevel;
        if(_currentLvl < 1)
        {
            _walkEffect.SetActive(true);
            _playerAnimator.SetBool("Legs",true);
            _playerAnimator.SetBool("MonoWheel", false);
            _playerAnimator.SetBool("HoverBoard",false);
            _monoWheel.SetActive(false);
            _hoverBoard1.SetActive(false);
            _hoverBoard2.SetActive(false);
            _playerAnimator.gameObject.transform.localPosition = new Vector3(_playerAnimator.gameObject.transform.localPosition.x, -0.436f, _playerAnimator.gameObject.transform.localPosition.z);


        }
        else if(_currentLvl == 1)
        {
            _walkEffect.SetActive(false);
            _playerAnimator.SetBool("Legs", false);
            _playerAnimator.SetBool("MonoWheel", true);
            _playerAnimator.SetBool("HoverBoard", false);
            _monoWheel.SetActive(true);
            _hoverBoard1.SetActive(false);
            _hoverBoard2.SetActive(false);
            _playerAnimator.gameObject.transform.localPosition=new Vector3(_playerAnimator.gameObject.transform.localPosition.x,-0.247f,_playerAnimator.gameObject.transform.localPosition.z);

        } 
        else if(_currentLvl==2)
        {
            _walkEffect.SetActive(false);
            _playerAnimator.SetBool("Legs", false);
            _playerAnimator.SetBool("MonoWheel", false);
            _playerAnimator.SetBool("HoverBoard", true);
            _monoWheel.SetActive(false);
            _hoverBoard1.SetActive(true);
            _hoverBoard2.SetActive(false);
            _playerAnimator.gameObject.transform.localPosition = new Vector3(_playerAnimator.gameObject.transform.localPosition.x, -0.127f, _playerAnimator.gameObject.transform.localPosition.z);
        }
        else if (_currentLvl >2)
        {
            _walkEffect.SetActive(false);
            
            _playerAnimator.SetBool("Legs", false);
            _playerAnimator.SetBool("MonoWheel", false);
            _playerAnimator.SetBool("HoverBoard", true);
            _monoWheel.SetActive(false);
            _hoverBoard1.SetActive(false);
            _hoverBoard2.SetActive(true);
            _playerAnimator.gameObject.transform.localPosition = new Vector3(_playerAnimator.gameObject.transform.localPosition.x, -0.127f, _playerAnimator.gameObject.transform.localPosition.z);
        }
        if (_playerAnimator.enabled)
        {
            MovingOnLegs();
        }
        else
        {
            _walkEffect.SetActive(false);
        }
    }
    private void MovingOnLegs()
    {
        if (_playerMover.MovementVector.magnitude > 0)
        {
            
            _playerAnimator.SetBool("Mooving", true);

            ChangeBlend(_playerMover.MovementVectorMagnitude * 1.5f, _playerAnimator);
            _playerAnimator.SetFloat("WalkSpeed", _playerMover.MovementVector.magnitude * 2f);
        }
        else
        {
            
            _playerAnimator.SetBool("Mooving", false);
        }

    }
    private void ChangeBlend(float parameter, Animator anim)
    {
        anim.SetFloat("Blend", parameter);
        
    }
}
