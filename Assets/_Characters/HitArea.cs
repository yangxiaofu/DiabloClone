using System.Collections;
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
            if (other.gameObject.GetComponent<PlayerControl>() && _defender.GetComponent<PlayerControl>())
            {
                _defender.GetComponent<PlayerControl>()
                    .GetComponent<HealthSystem>()
                    .TakeDamage(_attacker.GetHitDamage());
            }
            else if (other.gameObject.GetComponent<EnemyControl>() && _defender.GetComponent<EnemyControl>())
            {
                _defender.GetComponent<HealthSystem>()
                    .TakeDamage(_attacker.GetHitDamage());
            }
        }
    }

}
