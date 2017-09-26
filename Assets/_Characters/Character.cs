using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Game.Items;

namespace Game.Characters{
	public class Character : MonoBehaviour {
		[HeaderAttribute("Character Specific")]
		[SerializeField] protected float _hitDamage = 10f;
		[SerializeField] float _timeBeforeDestroying = 5f;
		[SerializeField] protected AnimatorOverrideController _animOC;
		[SerializeField] Avatar _avatar;
		public AnimatorOverrideController animOC{
			get{return _animOC;}
			set{_animOC = value;}
		}
		[SerializeField] protected AnimationClip _attackAnimation;

		[HeaderAttribute("Capsule Collider")]
		[SerializeField] float _radius = 0.3f;
		[SerializeField] float _height = 1.6f;

		const string DEFAULT_DEATH = "DEFAULT_DEATH";
		const string ANIMATION_DEATH = "Death";
		const string DEFAULT_ATTACK = "DEFAULT_ATTACK";
		[SerializeField] protected AnimationClip[] _deathAnimations;
		protected HealthSystem _healthSystem;
		public HealthSystem GetHealthSystem()
		{
			return _healthSystem;
		}
		protected BoxCollider[] _hitAreas;
		protected Animator _anim; 
		protected bool _killed = false;
		
		public bool killed
        {
            get{return _killed;}
        }

 		void Awake()
        {
            gameObject.AddComponent<AudioSource>();

			_anim = gameObject.AddComponent<Animator>();
			_anim.runtimeAnimatorController = _animOC;
			_anim.avatar = _avatar;
			_animOC[DEFAULT_DEATH] = _deathAnimations[
				UnityEngine.Random.Range(0, _deathAnimations.Length)
			];
			_animOC[DEFAULT_ATTACK] = _attackAnimation;

			var m_Capsule = gameObject.AddComponent<CapsuleCollider>();
			m_Capsule.radius = _radius;
			m_Capsule.height = _height;
			
        }

		void Start()
		{
			SetupHitAreaBoxCollidersOnBodyParts();
		}

		public IEnumerator KillCharacter()
        {
            TriggerDeathAnimation();
            yield return new WaitForSeconds(_timeBeforeDestroying);
            Destroy(this.gameObject);
        }

        private void DisableComponentsInCharacter()
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<CapsuleCollider>().enabled = false;
			
			if (GetComponent<EnemyControl>())
			{
				GetComponent<EnemyControl>().enabled = false;
			} 
			else if (GetComponent<PlayerControl>()) 
			{
				GetComponent<PlayerControl>().enabled = false;
			}
        }

        private void TriggerDeathAnimation()
        {
			var deathAnimationIndex = UnityEngine.Random.Range(0, _deathAnimations.Length);
            _animOC[DEFAULT_DEATH] = _deathAnimations[
				deathAnimationIndex
			];

			StartCoroutine(FinalizeKill(_deathAnimations[deathAnimationIndex].length));
            _anim.SetTrigger(ANIMATION_DEATH);
        }

		IEnumerator FinalizeKill(float delay)
		{
			yield return new WaitForSeconds(delay);
			DisableComponentsInCharacter();
			_killed = true;
			yield return null;
		}

        public float GetHitDamage()
        {
            return _hitDamage;
        }

        protected void SetupHitAreaBoxCollidersOnBodyParts()
        {
            _hitAreas = GetComponentsInChildren<BoxCollider>();
            for (int i = 0; i < _hitAreas.Length; i++)
            {
                _hitAreas[i].enabled = false;
            }
        }
	}

}
