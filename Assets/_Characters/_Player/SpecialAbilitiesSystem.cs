using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{
	public class SpecialAbilitiesSystem : MonoBehaviour {
		[SerializeField] float _currentEnergy = 100f;
        [SerializeField] float _maxEnergy = 100f;
		[SerializeField] float _regenPerSecond = 1f;
		[SerializeField] SpecialAbilities _ability;

		void Start()
		{
			 _ability.AttachComponentTo(this.gameObject);
		}


		void Update()
        {
            RegenerateEnergy();
        }

		public void AttemptSpecialAbility()
		{
			if (_ability.GetEnergyConsumption() <= GetCurrentEnergy())
			{
				_ability.Use();
			}
		}

		private void RegenerateEnergy()
        {
            _currentEnergy += _regenPerSecond * Time.deltaTime;
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        }

        public float energyAsPercentage{
			get{
				return _currentEnergy / _maxEnergy;
			}
		}

		public void ConsumeEnergy(float energyToConsume)
		{
			_currentEnergy -= energyToConsume;
			_currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
		}

		private float GetCurrentEnergy()
		{
			return _currentEnergy;
		}
	}
}

