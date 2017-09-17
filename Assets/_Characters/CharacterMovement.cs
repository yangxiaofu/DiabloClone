using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters{
	public class CharacterMovement : MonoBehaviour {
		protected Transform _target;
		protected NavMeshAgent agent;
		protected ThirdPersonCharacter character;
		protected void ProcessMovement()
        {
            print("remainingDistance " + agent.remainingDistance);
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }
	}
}


