using System.Collections;
using UnityEngine;

public class TankerAnimationControll : MonoBehaviour
{
   [SerializeField] private Animator _tankerAnimator;

   public void OpenDorForSec(float time)
   {
      StopAllCoroutines();
      _tankerAnimator.SetBool("OpenDoor",true);
      StartCoroutine(DoorClose(time));
   }

   private IEnumerator DoorClose(float time)
   {
      yield return new WaitForSecondsRealtime(time);
      _tankerAnimator.SetBool("OpenDoor",false);
   }
}
