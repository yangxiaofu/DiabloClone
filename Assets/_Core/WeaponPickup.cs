using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using Game.Characters;

namespace Game.Core{
	[ExecuteInEditMode]
	public class WeaponPickup : MonoBehaviour {
		[SerializeField] ItemConfig _item;

		void Update()
        {
            DestroyChildren();
            if (!_item) return;
            InstantiateWeapon();   
        }

        private void InstantiateWeapon()
        {
            var itemPrefab = _item.GetItemPrefab();
            var itemObject = Instantiate(itemPrefab);
            itemObject.transform.SetParent(this.transform);
			itemObject.transform.localPosition = Vector3.zero;
        }

        private void DestroyChildren()
		{
			foreach(Transform child in transform)
			{
				DestroyImmediate(child.gameObject);
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				other.gameObject.GetComponent<InventorySystem>().AddItemToInventory(_item);
			}
		}
	}
}

