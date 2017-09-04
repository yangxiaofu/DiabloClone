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
			
			if (other.gameObject.GetComponent(typeof(Character))) //TODO: Make changes later if it is a human hitting opposing enemy.
			{  
				Assert.IsNotNull(_defender,
					"You have not targeted a defender."
				);

				_defender.GetHealthSystem().TakeDamage(_attacker.GetHitDamage());
			}

		}
	}

}
