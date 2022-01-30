namespace dieow.krakjam2022
{
    using System;
    using UnityEngine;

    public class PlayerCharacter : MonoBehaviour
    {
        #region Public Methods
        public void GetHit()
        {
            _Health--;

            if (_Health <= 0)
            {
                Game.Dead();
            }

            WorldCommunicator.Communicate("Otrzymano 1 pkt. obrażeń.");

            UpdateLives();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private int _Health = 3;
        [SerializeField] private LayerMask _AnimalMask;
        [SerializeField] private ParticleSystem _TameEffect;
        [SerializeField] private ParticleSystem _AttackEffect;
        [SerializeField] private GameObject[] _Lives;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
            HandleInput();
        }
        #endregion Unity Methods

        #region Private Methods
        private void UpdateLives()
        {
            for (var i = 0; i < _Lives.Length; i++)
            {
                var life = _Lives[i];
                life.SetActive(_Health > i);
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoTame();
            }

            if (Input.GetMouseButtonDown(1))
            {
                DoHit();
            }
        }

        private void DoTame()
        {
            LookForAnimals(Tame, 7f);

            _TameEffect.Play();

            void Tame(Animal animal)
            {
                animal.Tame();
            }
        }

        private void DoHit()
        {
            LookForAnimals(Hit, 5f);

            _AttackEffect.Play();

            void Hit(Animal animal)
            {
                animal.Hit();
            }
        }

        private void LookForAnimals(Action<Animal> onHit, float range)
        {
            var hits = Physics.OverlapSphere(transform.position, range, _AnimalMask);

            foreach (var hit in hits)
            {
                var animal = hit.GetComponentInParent<Animal>();
                if (animal != null)
                {
                    onHit.Invoke(animal);
                }
            }
        }
        #endregion Private Methods
    }
}