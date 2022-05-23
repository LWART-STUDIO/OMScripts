using System.Collections;
using UnityEngine;
[DefaultExecutionOrder(1)]
public class SaveCorotune : MonoBehaviour
{
    [SerializeField] private bool _wantSave=true;
    [SerializeField] private float _timeToSave = 2f;
    private void Start()
    {
        StartCoroutine(Save());
    }
    private IEnumerator Save()
    {
        while (true)
        {
            if (_wantSave)
            {
                SaveManager.instance.Save();
            }
            yield return new WaitForSecondsRealtime(_timeToSave);
        }
        
    }
}
