using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Characters{
	public class EnergyBar : MonoBehaviour {

	    RawImage healthBarRawImage;
        SpecialAbilitiesSystem _energySystem;

        // Use this for initialization
        void Start()
        {
            _energySystem = FindObjectOfType<Player>().GetComponent<SpecialAbilitiesSystem>();
            healthBarRawImage = GetComponent<RawImage>();
        }

        // Update is called once per frame
        void Update()
        {
            float xValue = -(_energySystem.energyAsPercentage / 2f) - 0.5f;
            healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
	}
}

