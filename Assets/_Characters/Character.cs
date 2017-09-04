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
		public AnimatorOverrideController animOC{
			get{return _animOC;}
			set{_animOC = value;}
		}

		[SerializeField] protected WeaponConfig _activeWeapon;
		[SerializeField] protected AnimationClip _attackAnimation;
		[SerializeField] protected AnimationClip[] _deathAnimations;
		[SerializeField] AudioClip[] _hitAudio;
        public AudioClip[] GetHitAudio()
        {
            return _hitAudio;
        }

		protected HealthSystem _healthSystem;
		protected AudioSource _audioSource;
		public HealthSystem GetHealthSystem()
		{
			return _healthSystem;
		}

		protected BoxCollider[] _hitAreas;
		[HideInInspector] public string DEFAULT_ATTACK = "DEFAULT_ATTACK";
		protected const string DEFAULT_DEATH = "DEFAULT_DEATH";
		protected const string ANIMATION_DEATH = "Death";
		[HideInInspector] public string ANIMATION_ATTACK = "Attack";
		protected Animator _anim; 
		protected bool _killed = false;
		public bool killed
        {
            get{return _killed;}
        }

        protected bool _inAnimation = false;
        public bool inAnimation{get{return _inAnimation;}}

		public IEnumerator KillCharacter()
        {
            TriggerDeathAnimation();
            yield return new WaitForSeconds(_timeBeforeDestroying);
            Destroy(this.gameObject);
        }

		protected void GetCharacterComponents()
		{
			_healthSystem = GetComponent<HealthSystem>();
			_audioSource = GetComponent<AudioSource>();
		}

        private void DisableComponentsInCharacter()
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            GetComponent<CapsuleCollider>().enabled = false;
			
			if (GetComponent<Enemy>())
			{
				GetComponent<Enemy>().enabled = false;
			} 
			else if (GetComponent<Player>()) 
			{
				GetComponent<Player>().enabled = false;
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
		protected void PlayAudio() //Used as a placholder for weapon audio in animation events for weapons. 
        {
			if (_audioSource == null) return;

			if (_activeWeapon != null)
			{
				Assert.IsNotNull(_activeWeapon.GetAudioClip(), "There is no audio attached to the active weapon");
				_audioSource.clip = _activeWeapon.GetAudioClip();
				_audioSource.loop = false;
				_audioSource.Play();
			}
			
        }

        public float GetHitDamage()
        {
            return _hitDamage;
        }

		
		///Used to help keep the player from moving while string. 
        protected void BeginningOfAnimation()//Callback to animation events
        { 
            _inAnimation = true;
        }

		///Used to continue player movement.
		protected void EndOfAnimation()//Callback to animation events.  This helps with continuing movement after the animation is complete. 
        { 
            _inAnimation = false;
        }

		protected void OverrideAnimatorController()
        {
            _anim = GetComponent<Animator>();
            _anim.runtimeAnimatorController = _animOC;
			_animOC[DEFAULT_DEATH] = _deathAnimations[
				UnityEngine.Random.Range(0, _deathAnimations.Length)
			];
			_animOC[DEFAULT_ATTACK] = _attackAnimation;
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
