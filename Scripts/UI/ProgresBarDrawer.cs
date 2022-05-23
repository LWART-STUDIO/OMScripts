using UnityEngine;
using UnityEngine.UI;

public class ProgresBarDrawer : MonoBehaviour
{
    public float MinimumValue;
    public float MaximumValue;
    public float CurrentValue;
    [SerializeField] private Image _mask;


    private void Update()
    {
        SetCurrentFill();
    }
    private void SetCurrentFill()
    {
        float currentOffset = CurrentValue - MinimumValue;
        float maximumOffset= MaximumValue - MinimumValue;
        float fillAmount = currentOffset / maximumOffset;
        _mask.fillAmount = fillAmount;
    }
}
