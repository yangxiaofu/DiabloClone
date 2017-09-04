using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Core{
	public class Follow : MonoBehaviour {

		[SerializeField] GameObject _objectToFollow;

		void Start(){
			Assert.IsNotNull(_objectToFollow, "You need to reference an object to follow in the " + this.gameObject.name);
		}

		void Update(){
			this.transform.position = _objectToFollow.transform.position;
		}

	}

}
