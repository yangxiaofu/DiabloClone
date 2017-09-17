using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.CameraUI;

namespace Game.Characters{
	[RequireComponent(typeof(Player))]
	public class PlayerMovement : CharacterMovement{
		CameraRaycaster _cameraRaycaster;
		Player _player;
		const string WALK_TARGET = "Walk Target";
		GameObject walkTargetObject;

		// Use this for initialization
		void Start () 
		{
			agent = GetComponent<NavMeshAgent>();
			character = GetComponent<ThirdPersonCharacter>();
			_player = GetComponent<Player>();
			_cameraRaycaster = FindObjectOfType<CameraRaycaster>();
			_cameraRaycaster.NotifyWalkTriggerObservers += OnMouseOverPotentiallyWalkable;
		}

		void Update()
		{
			
			if (_target != null && _player.inAnimation == false)
			{
                agent.SetDestination(_target.position);
			}

			if (_player.inAnimation == true)
			{
				agent.SetDestination(this.transform.position);
			}

			ProcessMovement();  	
		}

		private void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
			{
				SetDestination(destination);
			}
        }

		private void PositionWalkTarget(Vector3 destination)
        {
            walkTargetObject = GameObject.Find(WALK_TARGET);

            if (!walkTargetObject)
            {
                walkTargetObject = new GameObject(WALK_TARGET);				
            }

			walkTargetObject.transform.position = destination;
        }

		public void SetDestination(Vector3 destination)
		{
			PositionWalkTarget(destination);
			SetTarget(walkTargetObject.transform);
		}

		private void SetTarget(Transform target)
		{
			_target = target;
		}
	}
}
