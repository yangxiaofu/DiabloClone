﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Game.Characters;

namespace Game.Items{
	///Add this to a child element of the hit area. 
	[RequireComponent(typeof(BoxCollider))]
	public class HitArea : MonoBehaviour { 
		[SerializeField] float _hitDamage = 10f;
		Character _defender;
		Character _attacker; 
		public void Initialize(Character defender, Character attacker){
			_defender = defender;
			_attacker = attacker;
		}

		void OnTriggerEnter(Collider other)
        {
            Assert.IsNotNull(_defender,
                "The defender is null.  Looks like you should initalize the defender first before it enters the trigger."
            );

            DealDamageToDefender(other);
        }

        private void DealDamageToDefender(Collider other)
        {
            if (other.gameObject.GetComponent<Player>() && _defender.GetComponent<Player>())
            {
                _defender.GetComponent<Player>()
                    .GetHealthSystem()
                    .TakeDamage(_attacker.GetHitDamage());
            }
            else if (other.gameObject.GetComponent<Enemy>() && _defender.GetComponent<Enemy>())
            {
                _defender.GetComponent<Enemy>()
                    .GetHealthSystem()
                    .TakeDamage(_attacker.GetHitDamage());
            }
        }
    }

}
