using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Game.Items;

namespace Game.Characters{
	public class Enemy : Character {
        [HeaderAttribute("Enemy Specfic")]
		[SerializeField] float _attackDistance = .2f;
		[SerializeField] float _timeBetweenAttacks = 2f;
        Player _player;
        bool _isAttacking = false;

		void Start()
        {
            GetCharacterComponents();
            FindPlayer();
            SetupHitAreaBoxCollidersOnBodyParts();
            OverrideAnimatorController();
        }
        void Update()
        {			
            if (_killed)
            {
                DisableHitAreas();
                return;
            }

            DetermineAttackBehaviour();
        }

        private void FindPlayer()
        {
            _player = FindObjectOfType<Player>();

            Assert.IsNotNull(_player,
                "YOu need to drag in a player prefab in the game scene.  Check to make sure the player game object has a player script attached to it."
            );
        }

        private void DetermineAttackBehaviour()
        {
            var distanceFromPlayer = Vector3.Distance( this.transform.position, _player.transform.position);
            if (distanceFromPlayer <= _attackDistance && _isAttacking == false)
            {
                StartCoroutine(AttackPlayer());
            }
            else if (distanceFromPlayer > _attackDistance)
            {
                _isAttacking = false;
                StopCoroutine(AttackPlayer());
            } 

        }

		private IEnumerator AttackPlayer()
        {
            Invoke("DisableHitAreas", _attackAnimation.length);
			_isAttacking = true;
			this.transform.LookAt(_player.transform); //TODO: Remove thsi and put in another coroutine to turn faster. 
			while (_isAttacking && !_killed)
            {
                GetComponent<NavMeshAgent>().isStopped = true;
				_anim.SetTrigger(ANIMATION_ATTACK);
				yield return new WaitForSeconds(_timeBetweenAttacks);	
			}

            GetComponent<NavMeshAgent>().isStopped = false;
            yield return null;
		}

        void BeginHit() //Used as an event in the animator.
        { 
			for(int i = 0; i < _hitAreas.Length; i++)
            {
				_hitAreas[i].gameObject.SetActive(true);
                _hitAreas[i].gameObject.GetComponent<BoxCollider>().enabled = true;
				_hitAreas[i].GetComponent<HitArea>().Initialize(_player, this);
			}
		}

		void EndHit()//Used as an event in the animator.
        {
            DisableHitAreas();
        }

        private void DisableHitAreas() //Used on an invoke method.
        {
            for (int i = 0; i < _hitAreas.Length; i++)
            {
                _hitAreas[i].gameObject.GetComponent<BoxCollider>().enabled = false;
                _hitAreas[i].gameObject.SetActive(false);
            }
        }

        void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, _attackDistance);
		}


	}

}
