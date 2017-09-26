using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;
using Game.CameraUI;
using Game.Items;


namespace Game.Characters{
	[SelectionBaseAttribute]
	public class PlayerControl : CharacterControl{

        [HeaderAttribute("Player Attack")]
		[SerializeField] float _meleeRange = 2f;
        [SerializeField] float _attackRadius = 2f;
		[SerializeField] GameObject _projectilePrefab; //TODO: Refactor out to weapon later.
		[HideInInspector] public string ANIMATION_ATTACK = "Attack";
		[HideInInspector] public string DEFAULT_ATTACK = "DEFAULT_ATTACK";
		protected bool _inAnimation = false;
		CameraRaycaster _cameraRaycaster;
		const string WALK_TARGET = "Walk Target";
		GameObject walkTargetObject;
		Character _character;
		InventorySystem _inventory;//TODO: Extract this out to aninventory 
		HealthSystem _healthSystem;
		EnemyControl _targetedEnemy;
		GameObject weaponObject;
        void Awake()
        {
			AddCharacterComponents();
        }

		// Use this for initialization
		void Start ()
        {
			_character = GetComponent<Character>();
			_healthSystem = GetComponent<HealthSystem>();
            _inventory = GetComponent<InventorySystem>();

			RegisterToRaycaster();
			GetCharacterComponents();
			PutWeaponInHand(_inventory.GetWeapon(0));	
        }
        
        void Update()
		{
			ScanForWeaponKeyInput();

			if (_target != null && _inAnimation == false)
			{
                agent.SetDestination(_target.position);
			}

			if (_inAnimation == true)
			{
				agent.SetDestination(this.transform.position);
			}

			ProcessMovement();  	
		}

        public void PutWeaponInHand(WeaponConfig weaponConfig) //TODO: For Weapon Extraction
        {
            _activeWeapon = weaponConfig;
            var gripTransform = GetComponentInChildren<WeaponGrip>().transform;
            DestroyChildrenIn(gripTransform);
            weaponObject = Instantiate(weaponConfig.GetItemPrefab());
            weaponObject.transform.SetParent(gripTransform);
            var weaponGripTransform = weaponConfig.GetGripTransform();
            weaponObject.transform.localPosition = weaponGripTransform.localPosition; 
            weaponObject.transform.localRotation = weaponGripTransform.localRotation;
        }
		
		private void RegisterToRaycaster()
        {
            _cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            _cameraRaycaster.NotifyMouseOverEnemyObservers += OnMouseOverEnemy;
			_cameraRaycaster.NotifyWalkTriggerObservers += OnMouseOverPotentiallyWalkable;
        }

		private void DestroyChildrenIn(Transform parent)
        {
            foreach(Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }

		void OnMouseOverEnemy(EnemyControl enemy)
        {
            if (_inAnimation) return;
            ScanForAttackMouseClick(enemy);
            ScanForDefensiveTrigger();
            ScanForSpecialAbility();
        }

        void Heal(float value)
        {
            _healthSystem.AddToCurrentHealth(value);
        }


		private void ScanForAttackMouseClick(EnemyControl enemy)
        {
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (_activeWeapon is MeleeWeaponConfig)
                {
                    var distanceFromEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (distanceFromEnemy < _attackRadius)
                    {
                        _targetedEnemy = enemy;
                        FaceToward(_targetedEnemy.transform); //TODO: Consider changing later so that a rotation happens. 
                        PerformAttack(enemy);
                    } else {
                        
                        GetComponent<PlayerControl>().SetDestination(enemy.transform.position);
                        //TODO: Will it automatically hit the enemy or should it just go to the enemy. DO later if necessary. 
                    }
                } else if (_activeWeapon is RangeWeaponConfig){
                    _targetedEnemy = enemy;
                    FaceToward(_targetedEnemy.transform); //TODO: Consider changing later so that a rotation happens. 
                    PerformAttack(enemy);
                }
            }
        }
		void Shoot() //Used for the animation event.
        {
            var projectileObject = Instantiate(
                _projectilePrefab.gameObject,
                weaponObject.GetComponentInChildren<ProjectileSocket>().transform.position,
                Quaternion.identity
            ) as GameObject;

            projectileObject.GetComponent<Projectile>().ShootProjectileAt(_targetedEnemy);
        }

		void FaceToward(Transform target)
        {
            this.transform.LookAt(target);
        }

        void BeginHit()//Used as an event in the animator.
        { 
            var meleeWeapon = _activeWeapon as MeleeWeaponConfig;
            _targetedEnemy.GetComponent<HealthSystem>().TakeDamage(meleeWeapon.GetAttackDamage());
		}

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, _meleeRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, _attackRadius);
        }

        private void PerformAttack(EnemyControl enemy)
        {
            _inAnimation = true;
            _targetedEnemy = enemy;
            _character.animOC[DEFAULT_ATTACK] = _activeWeapon.GetAnimation();
            _anim.SetTrigger(ANIMATION_ATTACK);
            StartCoroutine(EndInAnimation(_activeWeapon.GetAnimation().length));
        }

		public IEnumerator EndInAnimation(float delay)
        {
            yield return new WaitForSeconds(delay);
            _inAnimation = false;
            yield return null;
        }

        public void AddItemToInventory(PotionConfig potion)
        {
            _inventory.AddToInventory(potion);
        }

		private void ScanForDefensiveTrigger()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.LogWarning("Defensive Attack.  Needs implemented");
            }
        }

        private void ScanForSpecialAbility()
        {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
            {
                GetComponent<SpecialAbilitiesSystem>().AttemptSpecialAbility();
            }
        }

		private void ScanForWeaponKeyInput()
        {
            if (_inAnimation) {
                return;
            }
            
            if (Input.GetKeyDown("1"))
            {
                PutWeaponInHand(_inventory.GetWeapon(0));
            }
            
            if (Input.GetKeyDown("2"))
            {
                PutWeaponInHand(_inventory.GetWeapon(1));
            }

            if (Input.GetKeyDown("3"))
            {
                PutWeaponInHand(_inventory.GetWeapon(2));
            }

            if (Input.GetKeyDown("4"))
            {
                ConsumePotion();
            }
        }

		private void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
			{
				SetDestination(destination);
			}
        }

		public void ConsumePotion()
        {
            if (_healthSystem.GetCurrentHealth() >= _healthSystem.GetMaxHealth())
            {
                return;
            } 

            if (_inventory.GetPotionCount() > 0)
            {
                Heal(_inventory.GetPotion(0).GetRecoveryValue());
                _inventory.potions.RemoveAt(0);
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
