using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Game.Characters;

namespace Game.Items{
	public class Projectile : MonoBehaviour {

		[SerializeField] float _projectileSpeed = 50f;
		[SerializeField] float _projectileDamage = 10f;
		[SerializeField] float _timeBeforeDestroying = 2f;
		Collider _collider;
		void Start(){
			_collider = GetComponent<Collider>();
			Assert.IsNotNull(_collider, "You do not have a box collider on the projectile");
		}

		public float GetProjectileSpeed(){
			return _projectileSpeed;
		}

		public void ShootProjectileAt(EnemyControl enemy)
		{
			if (enemy != null){
				var direction = (enemy.transform.position - this.transform.position).normalized;
				GetComponent<Rigidbody>().velocity = direction * _projectileSpeed;
				StartCoroutine(DestroyProjectile());
			}
		}

		IEnumerator DestroyProjectile(){
			yield return new WaitForSeconds(_timeBeforeDestroying);
			Destroy(this.gameObject);
		}


		void OnTriggerEnter(Collider other)
		{

			if (other.GetComponent<EnemyControl>()){
				other.GetComponent<HealthSystem>().TakeDamage(_projectileDamage);
			}

			Destroy(this.gameObject);
		}


	}

}
