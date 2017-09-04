using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Game.Characters;

namespace Game.Items{
	public class Potion : MonoBehaviour {
		[SerializeField] PotionConfig _potionConfig;

		void Start()
		{
			Assert.IsNotNull(_potionConfig);
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.GetComponent<Player>())
			{
				var player = other.gameObject.GetComponent<Player>();
				player.AddItemToInventory(_potionConfig);
				Destroy(this.gameObject);
			}
		}

	}

}
