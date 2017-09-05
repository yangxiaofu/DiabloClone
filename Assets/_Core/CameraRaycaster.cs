using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Characters;

namespace Game.CameraUI{
	public class CameraRaycaster : MonoBehaviour {
		const string POTENTIALLY_WALKABLE_LAYER = "POTENTIALLY_WALKABLE";
		public delegate void MouseOverPotentiallyWalkable(Vector3 destination);
		public event MouseOverPotentiallyWalkable NotifyWalkTriggerObservers;

        public delegate void MouseOverEnemy(Enemy enemy);
        public event MouseOverEnemy NotifyMouseOverEnemyObservers;

        

		void FixedUpdate()
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit, 20000, 1<<8))
            {
                Debug.Log("Hit something " + hit.point);
                if(RaycastForPotentiallyWalkableLayer(hit)) return;
            }
        }

        bool RaycastForPotentiallyWalkableLayer(RaycastHit hit)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(POTENTIALLY_WALKABLE_LAYER))
            {
                NotifyWalkTriggerObservers(hit.point);
                return true;
              
            } 

            if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                if(hit.transform.gameObject.GetComponent<Enemy>().enabled == false)
                {
                    return false;
                }
                
                NotifyMouseOverEnemyObservers(
                    hit.transform.gameObject.GetComponent<Enemy>()
                ); 
            } 
			return false;
        }


    }

}