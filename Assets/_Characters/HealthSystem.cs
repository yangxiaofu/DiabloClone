using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{
	public class HealthSystem : MonoBehaviour {
		[SerializeField] protected float _currentHealth = 100f;
		public float GetCurrentHealth()
		{
			return _currentHealth;
		}

		public void AddToCurrentHealth(float value)
		{
			_currentHealth += value;
			_currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
		}

		[SerializeField] protected float _maxHealth = 100f;
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
            var character = GetComponent(typeof(Character)) as Character;
            var audio = character.GetHitAudio()[UnityEngine.Random.Range(0, character.GetHitAudio().Length)];

			if (!audio) return;
			
            GetComponent<AudioSource>().clip = audio;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
        }

    }

}
