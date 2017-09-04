using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Game.CameraUI;
using Game.Items;
using System;

namespace Game.Characters{

    [RequireComponent(typeof(Animator))]
	public class Player : Character {
        [SerializeField] GameObject _projectilePrefab; //TODO: Refactor out to weapon later.
        [SerializeField] Transform _projectileSocket;

        
        [TooltipAttribute("Anything within this range will use a melee weapon for attacking.")]
        [SerializeField] float _meleeRange = 2f;
        CameraRaycaster _cameraRaycaster;
        GameObject weaponObject;
        Enemy _targetedEnemy;
        InventorySystem _inventory;

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
            ScanForAttack(enemy);
            ScanForDefensiveTrigger();
            ScanForSpecialAbility();
        }

        private void ScanForAttack(Enemy enemy)
        {
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                _targetedEnemy = enemy;
                FaceToward(_targetedEnemy.transform); //TODO: Consider changing later so that a rotation happens. 
                PerformAttack(enemy);
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
                GetComponent<SpecialAbilitiesSystem>().UseSpecialAbility();
            }
        }

        void Heal(float value)
        {
            _healthSystem.AddToCurrentHealth(value);
        }

        private void PerformAttack(Enemy enemy)
        {
            var distanceFromEnemy = Vector3.Distance(enemy.transform.position, this.transform.position);
            _animOC[DEFAULT_ATTACK] = _activeWeapon.GetAnimation();
            _anim.SetTrigger(ANIMATION_ATTACK);
            StartCoroutine(EndInAnimation(_activeWeapon.GetAnimation().length));
        }

        public IEnumerator EndInAnimation(float delay)
        {
            yield return new WaitForSeconds(delay);
            _inAnimation = false;
            yield return null;
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

        void BeginHit()
        { //Used as an event in the animator.
            if (weaponObject == null)
            {
                var hitArea = weaponObject.GetComponentInChildren<HitArea>();
                hitArea.Initialize(_targetedEnemy, this);
                hitArea.GetComponent<BoxCollider>().enabled = true;
            } 
            else if (weaponObject.GetComponent(typeof(MeleeWeapon)))
            {
                
                weaponObject.GetComponent<MeleeWeapon>().Initialize(_activeWeapon as MeleeWeaponConfig, _targetedEnemy, this);
                weaponObject.GetComponent<BoxCollider>().enabled = true;
            }    
		}

		void EndHit()
        {//Used as an event in the animator.
            if (weaponObject == null)
            {  // IF NO WEAPON EQUIPPED
                var hitArea = weaponObject.GetComponentInChildren<HitArea>();
                hitArea.GetComponent<BoxCollider>().enabled = false;
            } 
            else if (weaponObject.GetComponent(typeof(MeleeWeapon)))
            { 
                weaponObject.GetComponent<BoxCollider>().enabled = false;
            }
		}

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, _meleeRange);

        }
    }
}

