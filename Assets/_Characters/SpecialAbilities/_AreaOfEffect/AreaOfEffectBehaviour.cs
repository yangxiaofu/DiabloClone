using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{

    public class AreaOfEffectBehaviour : SpecialAbilitiesBehaviour
    {
        void Start() 
        {
            _player = GetComponent<PlayerControl>();
            _anim = GetComponent<Animator>();  
        }
        public override void Use()
        {
            ConsumeEnergy();
            PlayParticleSystem();
            SetupAttackAnimation();
            DealDamageToEnemies();
            StartCoroutine(_player.GetComponent<PlayerControl>().EndInAnimation(_config.GetAnimation().length));
        }

        private void ConsumeEnergy()
        {
            GetComponent<SpecialAbilitiesSystem>().ConsumeEnergy(_config.GetEnergyConsumption());
        }

        private void PlayParticleSystem()
        {
            var prefab = _config.GetParticleSystem();
            var particleSystemObject = Instantiate(prefab, this.transform.position, Quaternion.identity);
            var particleSystem = particleSystemObject.GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(particleSystem.gameObject, particleSystem.main.duration);
        }

        private void SetupAttackAnimation()
        {
            _player.GetComponent<Character>().animOC[_player.GetComponent<PlayerControl>().DEFAULT_ATTACK] = _config.GetAnimation();
            _anim.SetTrigger(_player.GetComponent<PlayerControl>().ANIMATION_ATTACK);
            StartCoroutine(_player.GetComponent<PlayerControl>().EndInAnimation(_config.GetAnimation().length));
        }

        private void DealDamageToEnemies()
        {
            var radius = (_config as AreaOfEffect).GetDamageRadius();
            RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, radius, Vector3.up, radius);
            for (int i = 0; i < hits.Length; i++)
            {
                var character = hits[i].collider.gameObject.GetComponent<Character>();
                if (character && character.GetComponent<EnemyControl>())
                {
                    character.GetComponent<HealthSystem>().TakeDamage(_config.GetAttackDamage());
                }
            }
        }
    }

}