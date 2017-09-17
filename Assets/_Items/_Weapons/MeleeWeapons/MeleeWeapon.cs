using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Characters;

namespace Game.Items{
	public class MeleeWeapon : Weapon {
		MeleeWeaponConfig _config;
		Character _defender;
		Character _attacker;
		public void Initialize(MeleeWeaponConfig config, Character defender, Character attacker)
		{
			_defender = defender;
			_attacker = attacker;
			_config = config;
		}

		void OnTriggerEnter(Collider other)
		{
			if (_defender && other.gameObject.GetComponent<Character>())
			{
				if (!_attacker.killed)
				{
					_defender.GetHealthSystem().TakeDamage(_config.GetAttackDamage());				
				}
			} 
		}
	}
}

