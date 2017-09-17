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
        [SerializeField] float _attackDamage = 10f;
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
            if (_killed) return;
            DetermineAttackBehaviour();
        }

        private void FindPlayer()
        {
            _player = FindObjectOfType<Player>();

            Assert.IsNotNull(_player,
                "You need to drag in a player prefab in the game scene.  Check to make sure the player game object has a player script attached to it."
            );
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

        private bool IsInAttackRadius(float distanceFromPlayer)
        {   
            return distanceFromPlayer <= _attackDistance && _isAttacking == false;
        }

        private bool OutsideOfAttackRadius(float distanceFromPlayer)
        {
            return distanceFromPlayer > _attackDistance;
        }

		private IEnumerator AttackPlayer()
        {
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
            FindObjectOfType<Player>()
                .GetComponent<HealthSystem>()
                .TakeDamage(_attackDamage);
		}

        void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, _attackDistance);
		}


	}

}
