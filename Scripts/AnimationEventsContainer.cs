using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsContainer : MonoBehaviour
{
    [SerializeField] List<UnityEvent> animationEvents;

    public void InvokeEvent(int index)
    {
        Debug.Log("Step 1");
        animationEvents[index].Invoke();
    }
}
