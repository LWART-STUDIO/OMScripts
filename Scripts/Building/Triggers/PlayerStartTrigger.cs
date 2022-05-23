using UnityEngine;

public class PlayerStartTrigger : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.TryGetComponent(out PlayerAIBrain playerAIBrain))
      {
         playerAIBrain.StartLevel();
         gameObject.SetActive(false);
      }
   }
}
