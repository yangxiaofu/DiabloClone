using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters{

    public class AreaOfEffectBehaviour : SpecialAbilitiesBehaviour
    {
        void Start() 
        {
            _player = GetComponent<Player>();
            _anim = GetComponent<Animator>();
            
        }

        public override void Use()
        {
            ConsumeEnergy();
            PlayParticleSystem();
            SetupAttackAnimation();
            DealDamageToEnemies();
            StartCoroutine(_player.EndInAnimation(_config.GetAnimation().length));
        }

        private void ConsumeEnergy()
        {
            GetComponent<EnergySystem>().ConsumeEnergy(_config.GetEnergyConsumption());
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
            _player.animOC[_player.DEFAULT_ATTACK] = _config.GetAnimation();
            _anim.SetTrigger(_player.ANIMATION_ATTACK);
            StartCoroutine(_player.EndInAnimation(_config.GetAnimation().length));
        }

        private void DealDamageToEnemies()
        {
            var radius = (_config as AreaOfEffect).GetDamageRadius();
            RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, radius, Vector3.up, radius);
            for (int i = 0; i < hits.Length; i++)
            {
                var character = hits[i].collider.gameObject.GetComponent<Character>();
                if (character && character is Enemy)
                {
                    character.GetComponent<Enemy>().GetHealthSystem().TakeDamage(_config.GetAttackDamage());
                }
            }
        }
    }

}