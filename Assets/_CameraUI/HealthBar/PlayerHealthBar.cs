using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Characters{
    public class PlayerHealthBar : MonoBehaviour
    {
        Image _image;
        Player player;

        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<Player>();
            _image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            _image.fillAmount = player.GetHealthSystem().healthAsPercentage;   
        }
    }

}
