using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{
	[CreateAssetMenuAttribute(menuName = "Game/Special Ability/Power Attack")]
    public class PowerAttackConfig : SpecialAbilities
    {
       
        [SerializeField] Animation _attackAnimation;
		
        public override void AttachComponentTo(GameObject gameobjectToAttachTo)
        {
            _behaviour = gameobjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            _behaviour.SetConfig(this);
        }

      
    }

}
