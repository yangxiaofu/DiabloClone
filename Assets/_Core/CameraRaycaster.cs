using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Characters;

namespace Game.CameraUI{
	public class CameraRaycaster : MonoBehaviour {
        [SerializeField] float _raycastDistance = 1000f;
		const string POTENTIALLY_WALKABLE_LAYER = "POTENTIALLY_WALKABLE";
        const int WALKABLE_LAYER_BIT = 8;
        const int ENEMY_LAYER = 10;

		public delegate void MouseOverPotentiallyWalkable(Vector3 destination);
		public event MouseOverPotentiallyWalkable NotifyWalkTriggerObservers;

        public delegate void MouseOverEnemy(EnemyControl enemy);
        public event MouseOverEnemy NotifyMouseOverEnemyObservers;

		void FixedUpdate()
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit, _raycastDistance, (1<<WALKABLE_LAYER_BIT)|(1<<ENEMY_LAYER)))
            {
                if(RaycastForPotentiallyWalkableLayer(hit)) {
                    return;
                }

                if(RaycastForEnemy(hit)){
                    return;
                } 
            }
        }

        bool RaycastForPotentiallyWalkableLayer(RaycastHit hit)
        {
            if (HitAWalkableLayer(hit))
            {
                NotifyWalkTriggerObservers(hit.point);
                return true;
            }    
			return false;
        }

        private bool RaycastForEnemy(RaycastHit hit)
        {
            if (HitAnEnemy(hit))
            {
                if(hit.transform.gameObject.GetComponent<EnemyControl>().enabled == false)
                {
                    return false;
                }
                
                NotifyMouseOverEnemyObservers(
                    hit.transform.gameObject.GetComponent<EnemyControl>()
                ); 
                return true;
            } 
            return false;
        }

        private bool HitAWalkableLayer(RaycastHit hit)
        {
            return hit.transform.gameObject.layer == LayerMask.NameToLayer(POTENTIALLY_WALKABLE_LAYER);
        }

        private bool HitAnEnemy(RaycastHit hit)
        {
            return hit.transform.gameObject.GetComponent<EnemyControl>();
        }


    }

}