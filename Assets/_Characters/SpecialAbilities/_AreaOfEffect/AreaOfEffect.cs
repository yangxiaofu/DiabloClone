using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{
	[CreateAssetMenuAttribute(menuName = "Game/Special Ability/Area of Effect")]
	public class AreaOfEffect : SpecialAbilities {
		[HeaderAttribute("Area of Effect Specific")]
		[SerializeField] float _damageRadius = 2f;
		[SerializeField] float _energyConsumed = 50f;

        public override void AttachComponentTo(GameObject gameobjectToAttachTo)
        {
			_behaviour = gameobjectToAttachTo.AddComponent<AreaOfEffectBehaviour>();
			_behaviour.SetConfig(this);
        }

		public float GetDamageRadius()
		{
			return _damageRadius;
		}

		public float GetEnergyConsume()
		{
			return _energyConsumed;
		}

    
	}

}
