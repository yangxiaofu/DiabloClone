using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters{
	public class EnemyMovement : CharacterMovement {

		[SerializeField] float _moveDistanceFromTarget = 10f;
		Player _player;
        bool _killTriggered = false;

		void Start()
        {
			_player = FindObjectOfType<Player>();
			agent = GetComponent<NavMeshAgent>();
			character = GetComponent<ThirdPersonCharacter>();
		}

        void Update()
        {
            if (_killTriggered) return;
            
            if (KillInProcess())
            {
                ProcessKill();
                return;
            }

            SetDestinationTargetForMovement();
            ProcessMovement();    
        }

        private bool KillInProcess()
        {
            return GetComponent<Enemy>().killed && _killTriggered == false;
        }

        private void ProcessKill()
        {
            GetComponent<Animator>().enabled = false;
            agent.SetDestination(this.transform.position);
            _killTriggered = true;
        }

        private void SetDestinationTargetForMovement()
        {
            var distanceFromPlayer = Vector3.Distance(this.transform.position, _player.transform.position);

            if (distanceFromPlayer < _moveDistanceFromTarget)
            {
                agent.SetDestination(_player.transform.position);
            }
            else
            {
                agent.SetDestination(this.transform.position);
            }
        }

        void OnDrawGizmos()
        {
			Gizmos.color = new Color(200/255, 150/255, 100/255, 1);
			Gizmos.DrawWireSphere(this.transform.position, _moveDistanceFromTarget);
		}
	}
}

