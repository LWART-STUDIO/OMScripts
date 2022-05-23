using UnityEngine;
using DG.Tweening;

public class FrucktScaler : MonoBehaviour
{
    [SerializeField] private bool _canGrow;
    private void OnEnable()
    {
        _canGrow = true;
        transform.DOScale(1f, 2f);

    }

    private void OnDisable()
    {
        _canGrow = false;
        transform.localScale = new Vector3(0, 0, 0);
    }
}
