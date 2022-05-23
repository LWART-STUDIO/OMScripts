using UnityEngine;

public class TransporterPoint : MonoBehaviour
{
   public bool Empty;
   public ResourceMover OilResourceMover;

   public void GetResource(ResourceMover resource)
   {
      OilResourceMover = resource;
      Empty = false;
   }

   /*public void GiveResource(Transform pointToMove, Transform parentObject, bool remove = false)
   {
      OilResourceMover.MoveResource(pointToMove, parentObject, remove);
      OilResourceMover = null;
      Empty = true;
      

   }*/
   public GameObject GiveResource(Transform pointToMove, Transform parentObject,bool remove=false)
   {
      GameObject resource = OilResourceMover.gameObject;
      ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
      resourceMover.MoveResource(pointToMove, parentObject,remove);
      Empty = true;
      OilResourceMover = null;
      return resource;
   }
}
