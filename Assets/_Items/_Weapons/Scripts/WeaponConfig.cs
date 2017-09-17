using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items{

	[CreateAssetMenuAttribute(menuName = "Game/Weapon")]
	public class WeaponConfig : ItemConfig{
		[SerializeField] Transform _gripTransform;
		[SerializeField] AnimationClip _weaponAnimation;
		[SerializeField] AudioClip _audioClip;
		public Transform GetGripTransform()
		{
			return _gripTransform;
		}

		public AnimationClip GetAnimation()
		{
			return _weaponAnimation;
		}

		public AudioClip GetAudioClip()
		{
			return _audioClip;
		}
	}

}
