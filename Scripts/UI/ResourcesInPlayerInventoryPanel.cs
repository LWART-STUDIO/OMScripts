using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourcesInPlayerInventoryPanel : MonoBehaviour
{
   [SerializeField] private TMP_Text _foodText;
   [SerializeField] private TMP_Text _metallText;
   [SerializeField] private TMP_Text _plasticText;
   [SerializeField] private TMP_Text _oilText;
   [SerializeField] private TMP_Text _waterText;
   private PlayerInventory _playerInventory;

   private void Start()
   {
     _foodText.transform.parent.gameObject.SetActive(false);
     _metallText.transform.parent.gameObject.SetActive(false);
     _plasticText.transform.parent.gameObject.SetActive(false);
     _oilText.transform.parent.gameObject.SetActive(false);
     _waterText.transform.parent.gameObject.SetActive(false);
   }

   private void Update()
   {
      if (_playerInventory == null)
      {
         _playerInventory = FindObjectOfType<PlayerInventory>();
         return;
      }
      var f= _playerInventory.Inventory.InventoryInfo.ResourcesInfo.FindAll(x => x.ID == "Food").Count;
      if (f == 0)_foodText.transform.parent.gameObject.SetActive(false);else{_foodText.transform.parent.gameObject.SetActive(true);
         _foodText.text = f.ToString();}
      
      var m= _playerInventory.Inventory.InventoryInfo.ResourcesInfo.FindAll(x => x.ID == "Metall").Count;
      if (m == 0)_metallText.transform.parent.gameObject.SetActive(false);else{_metallText.transform.parent.gameObject.SetActive(true);
         _metallText.text = m.ToString();}
      
      var p = _playerInventory.Inventory.InventoryInfo.ResourcesInfo.FindAll(x => x.ID == "Plastic").Count;
      if (p == 0)_plasticText.transform.parent.gameObject.SetActive(false);else{_plasticText.transform.parent.gameObject.SetActive(true);
         _plasticText.text = p.ToString();}
      
      var o= _playerInventory.Inventory.InventoryInfo.ResourcesInfo.FindAll(x => x.ID == "Oil").Count;
      if (o == 0)_oilText.transform.parent.gameObject.SetActive(false);else{_oilText.transform.parent.gameObject.SetActive(true);
         _oilText.text = o.ToString();}
      
      var w= _playerInventory.Inventory.InventoryInfo.ResourcesInfo.FindAll(x => x.ID == "Water").Count;
      if (w == 0)_waterText.transform.parent.gameObject.SetActive(false);else{_waterText.transform.parent.gameObject.SetActive(true);
         _waterText.text = w.ToString();
      }
      
   }
}
