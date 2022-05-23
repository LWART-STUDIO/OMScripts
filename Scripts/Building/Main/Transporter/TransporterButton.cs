using UnityEngine;

public class TransporterButton : MonoBehaviour
{
    [SerializeField] private Transporter _transporter;
    [SerializeField] private GameObject _greenButton;
    [SerializeField] private GameObject _redButton;
    [SerializeField] private Animator _buttonAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMover playerMover))
        {
            if (_transporter.Work)
            {
                _buttonAnimator.SetTrigger("Click");
                _transporter.StopWork();
                _greenButton.SetActive(false);
                _redButton.SetActive(true);
                
            }
            else
            {
                _buttonAnimator.SetTrigger("Click");
                _transporter.StartWork();
                _greenButton.SetActive(true);
                _redButton.SetActive(false);
            }
        }
    }

    private void Start()
    {
        if (_transporter.Work)
        {
            _greenButton.SetActive(true);
            _redButton.SetActive(false);
        }
        else
        {
            _greenButton.SetActive(false);
            _redButton.SetActive(true);
        }
    }
}
