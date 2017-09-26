using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Characters{
    public class PlayerHealthBar : MonoBehaviour
    {
        Image _image;
        PlayerControl playerControl;

        // Use this for initialization
        void Start()
        {
            playerControl = FindObjectOfType<PlayerControl>();
            _image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            _image.fillAmount = playerControl.GetComponent<HealthSystem>().healthAsPercentage;
        }
    }

}
