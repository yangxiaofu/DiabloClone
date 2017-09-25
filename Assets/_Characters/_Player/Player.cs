using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Game.CameraUI;
using Game.Items;
using System;

namespace Game.Characters{

	public class Player : Character {
        [HeaderAttribute("Player Setup")]

        [HeaderAttribute("Player Variables")]
        [SerializeField] GameObject _projectilePrefab; //TODO: Refactor out to weapon later.
        [SerializeField] Transform _projectileSocket;
        
        [TooltipAttribute("Anything within this range will use a melee weapon for attacking.")]
        [SerializeField] float _meleeRange = 2f;
        [SerializeField] float _attackRadius = 2f;

        [HeaderAttribute("Animator")]
		[SerializeField] Avatar _avatar;
		[SerializeField] RuntimeAnimatorController _controller;
        
		[HeaderAttribute("Capsule Collider")]
		[SerializeField] float _radius = 0.3f;
		[SerializeField] float _height = 1.6f;
        CapsuleCollider m_Capsule;

        CameraRaycaster _cameraRaycaster;
        GameObject weaponObject;
        Enemy _targetedEnemy;
        InventorySystem _inventory;
        

        void Awake()
        {
            gameObject.AddComponent<AudioSource>();

            m_Capsule = gameObject.AddComponent<CapsuleCollider>();
			m_Capsule.radius = _radius;
			m_Capsule.height = _height;

            var m_Animator = gameObject.AddComponent<Animator>();
			m_Animator.runtimeAnimatorController = _controller;
			m_Animator.avatar = _avatar;
        }

        void Start()
        {
            GetCharacterComponents();
            GetPlayerComponents();
            PerformAssertions();
            SetupHitAreaBoxCollidersOnBodyParts(); //Needs setup for 
            RegisterToRaycaster();
            PutWeaponInHand(_inventory.GetWeapon(0));
            OverrideAnimatorController();
        }
        
        private void GetPlayerComponents()
        {
            _inventory = GetComponent<InventorySystem>();
        }

        void Update()
        {
            ScanForWeaponKeyInput();
        }

        public void AddItemToInventory(PotionConfig potion)
        {
            _inventory.AddToInventory(potion);
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

        private void ConsumePotion()
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

        private void PerformAssertions()
        {
            Assert.IsNotNull(_projectileSocket,"You need to add the projection game object as a child of the player");
        } 

        private void RegisterToRaycaster()
        {
            _cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            _cameraRaycaster.NotifyMouseOverEnemyObservers += OnMouseOverEnemy;
        }

        private void PutWeaponInHand(WeaponConfig weaponConfig)
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

        private void DestroyChildrenIn(Transform parent)
        {
            foreach(Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (_inAnimation) return;
            ScanForAttackMouseClick(enemy);
            ScanForDefensiveTrigger();
            ScanForSpecialAbility();
        }

        private void ScanForAttackMouseClick(Enemy enemy)
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
                        
                        GetComponent<PlayerMovement>().SetDestination(enemy.transform.position);
                        //TODO: Will it automatically hit the enemy or should it just go to the enemy. DO later if necessary. 
                    }
                } else if (_activeWeapon is RangeWeaponConfig){
                    _targetedEnemy = enemy;
                    FaceToward(_targetedEnemy.transform); //TODO: Consider changing later so that a rotation happens. 
                    PerformAttack(enemy);
                }
            }
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

        void Heal(float value)
        {
            _healthSystem.AddToCurrentHealth(value);
        }

        private void PerformAttack(Enemy enemy)
        {
            _inAnimation = true;
            _targetedEnemy = enemy;
            _animOC[DEFAULT_ATTACK] = _activeWeapon.GetAnimation();
            _anim.SetTrigger(ANIMATION_ATTACK);
            StartCoroutine(EndInAnimation(_activeWeapon.GetAnimation().length));
        }

        void Shoot() //Used for the animation event.
        {
            var projectileObject = Instantiate(
                _projectilePrefab.gameObject,
                weaponObject.GetComponentInChildren<ProjectileSocket>().transform.position,
                Quaternion.identity
            ) as GameObject;

            projectileObject.GetComponent<Projectile>().ShootProjectileAt(_targetedEnemy, this);
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
    }
}

