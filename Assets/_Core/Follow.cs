using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Game.Characters;

namespace Game.Core{
	public class Follow : MonoBehaviour {

		Player _player;

		void Start(){
			_player = FindObjectOfType<Player>();

			Assert.IsNotNull(_player, "There is no player in the game scene.");
		}

		void Update(){
			this.transform.position = _player.transform.position;
		}

	}

}
