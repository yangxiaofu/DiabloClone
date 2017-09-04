using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{
	public abstract class SpecialAbilitiesBehaviour : MonoBehaviour {

		public SpecialAbilities _config;
		protected Player _player;
		protected Animator _anim;

		public void SetConfig(SpecialAbilities config){
			_config = config;
		}
		
		public abstract void Use();
	}	

}

