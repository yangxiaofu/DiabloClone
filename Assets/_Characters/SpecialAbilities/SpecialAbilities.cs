using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{
	public abstract class SpecialAbilities : ScriptableObject {
		[SerializeField] float _attackDamage = 10f;
		[SerializeField] AnimationClip _animation;
		[SerializeField] AudioClip _audioClip;
		[SerializeField] ParticleSystem _particleSystem;
		[SerializeField] float _energyConsumed = 50f;

		public float GetEnergyConsumption()
		{
			return _energyConsumed;
		}
		public abstract void AttachComponentTo(GameObject gameobjectToAttachTo);
		protected SpecialAbilitiesBehaviour _behaviour;
		public void Use(){
			_behaviour.Use();
		}

		public float GetAttackDamage(){
            return _attackDamage;
        }

		public AnimationClip GetAnimation()
		{
			return _animation;
		}
		public AudioClip GetAudioClip()
		{
			return _audioClip;
		}

		public ParticleSystem GetParticleSystem()
		{
			return _particleSystem;
		}
	}
}

