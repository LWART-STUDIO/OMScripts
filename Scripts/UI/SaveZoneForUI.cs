using UnityEngine;

public class SaveZoneForUI : MonoBehaviour
{
    private RectTransform _rectTransform;
    Rect _saveArea;
    Vector2 _minAnchor;
    Vector2 _maxAnchor;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _saveArea = Screen.safeArea;
        _minAnchor = _saveArea.position;
        _maxAnchor=_minAnchor + _saveArea.size;

        _minAnchor.x /=Screen.width;
        _minAnchor.y /=Screen.height;
        _maxAnchor.x /=Screen.width;
        _maxAnchor.y /=Screen.height;

        _rectTransform.anchorMin = _minAnchor;
        _rectTransform .anchorMax = _maxAnchor;
    }
}
