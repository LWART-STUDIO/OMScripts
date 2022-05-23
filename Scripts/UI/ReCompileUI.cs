using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReCompileUI : MonoBehaviour
{
    private HorizontalLayoutGroup _horizontalLayoutGroup;

    private void Start()
    {
        _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        StartCoroutine(CustomUpdate());
    }

    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            _horizontalLayoutGroup.enabled = false;
            _horizontalLayoutGroup.enabled = true;
        }
        
    }
}
