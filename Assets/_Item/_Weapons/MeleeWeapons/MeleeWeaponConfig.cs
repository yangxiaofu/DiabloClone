using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items{
	[CreateAssetMenuAttribute(menuName = "Game/Melee Weapon")]
	public class MeleeWeaponConfig : WeaponConfig
	{
		[HeaderAttribute("Melee Weapon Config Specific")]
		[SerializeField] float _attackDamage = 10f;
		public float GetAttackDamage()
		{
			return _attackDamage;
		}
	}
}


