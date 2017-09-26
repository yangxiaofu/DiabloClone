using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;

namespace Game.Characters{
	[System.SerializableAttribute]
	public class InventorySystem : MonoBehaviour {
		[SerializeField] List<WeaponConfig> _weapons = new List<WeaponConfig>();
        [SerializeField] List<PotionConfig> _potions = new List<PotionConfig>();

		public List<PotionConfig> potions{
			get{return _potions;}
			set{_potions = value;}
		}
        public void AddToInventory(PotionConfig potion)
        {
            _potions.Add(potion);
        }

		public WeaponConfig GetWeapon(int index)
		{
			return _weapons[index];
		}

		public PotionConfig GetPotion(int index)
		{
			return _potions[index];
		}

		public int GetPotionCount()
		{
			return _potions.Count;
		}
	}

}

