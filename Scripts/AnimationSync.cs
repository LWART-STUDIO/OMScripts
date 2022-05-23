using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSync : MonoBehaviour
{
    [SerializeField] private Animator Animsync;
    [SerializeField] private Animator Animation;
    
    private void Update()
    {
        Animation.Play(0, -1, Animsync.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
}
