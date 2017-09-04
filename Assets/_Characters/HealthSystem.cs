using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{
	public class HealthSystem : MonoBehaviour {
		[SerializeField] protected float _currentHealth = 100f;
		[SerializeField] protected float _maxHealth = 100f;
		[SerializeField] AudioClip[] _hitAudio;

		public float GetCurrentHealth()
		{
			return _currentHealth;
		}

		public void AddToCurrentHealth(float value)
		{
			_currentHealth += value;
			_currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
		}


		public float GetMaxHealth()
		{
			return _maxHealth;
		}
		public float healthAsPercentage
		{
			get{return _currentHealth / _maxHealth;}
		}

		public void TakeDamage(float damageToTake)
		{
			if (_currentHealth <= 0) return;

			_currentHealth -= damageToTake;
			PlayCharacterHitAudio();

			if (_currentHealth <= 0)
			{
				StartCoroutine(GetComponent<Character>().KillCharacter());
			}

			_currentHealth = Mathf.Clamp(_currentHealth, 0, _currentHealth);
        }

        private void PlayCharacterHitAudio()
        {
			if (_hitAudio.Length == 0) return;
			
            var audio = _hitAudio[UnityEngine.Random.Range(0, _hitAudio.Length)];

			if (!audio) return;
			
            GetComponent<AudioSource>().clip = audio;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
        }
    }

}
