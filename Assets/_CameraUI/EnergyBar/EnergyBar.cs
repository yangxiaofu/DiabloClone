using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Characters{
	public class EnergyBar : MonoBehaviour {

	    Image _orb;
        SpecialAbilitiesSystem _energySystem;

        // Use this for initialization
        void Start()
        {
            _energySystem = FindObjectOfType<Player>().GetComponent<SpecialAbilitiesSystem>();
            _orb = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            _orb.fillAmount = _energySystem.energyAsPercentage;
        }
	}
}

