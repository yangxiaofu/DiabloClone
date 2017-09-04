using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items{
	[CreateAssetMenuAttribute(menuName = "Game/Potions/Potion")]
	public class PotionConfig : ItemConfig {
		[SerializeField] float _recoveryValue = 10f;

		public float GetRecoveryValue()
		{
			return _recoveryValue;
		}
	}
}

