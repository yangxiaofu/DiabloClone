using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters{
	public class EnemyControl : CharacterControl {

        [HeaderAttribute("Enemy Specfic")]
		[SerializeField] float _attackDistance = .2f;
		[SerializeField] float _timeBetweenAttacks = 2f;
		[SerializeField] float _moveDistanceFromTarget = 10f;
        [SerializeField] AudioClip[] _zombieAudio;

        const string ANIMATION_ATTACK = "Attack";
		PlayerControl _player;
        bool _killTriggered = false;
        bool _isAttacking = false;

        void Awake()
        {
            AddCharacterComponents();
        }

		void Start()
        {
			_player = FindObjectOfType<PlayerControl>();	
            GetCharacterComponents();
		}

        void Update()
        {
            if (GetComponent<Character>().killed) return;

            if (_killTriggered) return;
            
            if (KillInProcess())
            {
                ProcessKill();
                return;
            }

            DetermineAttackBehaviour();
            SetDestinationTargetForMovement();
            ProcessMovement();    
        }

        private void DetermineAttackBehaviour()
        {
            var distanceFromPlayer = Vector3.Distance( this.transform.position, _player.transform.position);

            if (IsInAttackRadius(distanceFromPlayer)) //IF  can attack player.
            {
                StartCoroutine(AttackPlayer());
            } 
            else if (OutsideOfAttackRadius(distanceFromPlayer))
            {
                _isAttacking = false;
                StopCoroutine(AttackPlayer());
            } 
        }

		private IEnumerator AttackPlayer()
        {
			_isAttacking = true;
			this.transform.LookAt(_player.transform); //TODO: Remove thsi and put in another coroutine to turn faster. 
			while (_isAttacking && !GetComponent<Character>().killed)
            {
                GetComponent<NavMeshAgent>().isStopped = true;
				_anim.SetTrigger(ANIMATION_ATTACK);
				yield return new WaitForSeconds(_timeBetweenAttacks);	
			}

            GetComponent<NavMeshAgent>().isStopped = false;
            yield return null;
		}

        private bool IsInAttackRadius(float distanceFromPlayer)
        {   
            return distanceFromPlayer <= _attackDistance && _isAttacking == false;
        }

        private bool KillInProcess()
        {
            return GetComponent<Character>().killed && _killTriggered == false;
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

        void BeginHit() //Used as an event in the animator.
        { 
            if (GetComponent<Character>().killed) return;
            if (OutsideOfAttackRadius(Vector3.Distance( this.transform.position, _player.transform.position))) return;
            _player.GetComponent<HealthSystem>().TakeDamage(GetComponent<Character>().GetHitDamage());
		}

         private bool OutsideOfAttackRadius(float distanceFromPlayer)
        {
            return distanceFromPlayer > _attackDistance;
        }

        void OnDrawGizmos()
        {
			Gizmos.color = new Color(200/255, 150/255, 100/255, 1);
			Gizmos.DrawWireSphere(this.transform.position, _moveDistanceFromTarget);

            Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, _attackDistance);
		}
	}
}

