namespace dieow.krakjam2022
{
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.AI;

    public sealed class MrCostanza : MonoBehaviour
    {
        #region Public Methods
        public void DropDead()
        {
            DialogSystem.HideMessage(_Id);
            Destroy(gameObject);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private readonly string _Id = "MrCostanza";

        [SerializeField] private Transform _HisHouse;
        [SerializeField] private Transform _DropPos;

        [SerializeField] private int _MinimalCountOfAnimalsToTake = 5;

        [SerializeField] private string _WelcomeMsg;

        [SerializeField] private string _WithAnimalsMsg;
        [SerializeField] private string _WithoutAnimalsMsg;
        [SerializeField] private string _ToLittleAnimalsMsg;
        [SerializeField] private string _AfterGoodPlaceMsg;

        [SerializeField] private MeshRenderer _MeshRenderer;
        [SerializeField] private NavMeshAgent _NavMeshAgent;

        [SerializeField] private Transform _DialogHolder;

        // Demonic
        [SerializeField] private Material _DemonicMaterial;
        [SerializeField] private float _AttackRange;
        [SerializeField] private float _AttackCooldown;
        [SerializeField] private ParticleSystem _AttackEffect;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            DialogSystem.ShowMessage(_Id, transform, _WelcomeMsg);
            Game.AnimalsAreTakenToGoodPlace += TransformIntoDemon;
        }

        private void Update()
        {
            if (_IsTakingAnimals) { return; }

            HandleWelcomeDialog();

            if (_ShowingWelcomeMsg) { return; }

            if (_IsDemon)
            {
                HandleDemonicBehaviour();
            }
            else
            {
                HandleAnimalsDialogs();
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private bool _ShowingWelcomeMsg = true;
        private bool _IsTakingAnimals;
        private bool _IsDemon;
        private float _NextAttackTime;
        #endregion Private Variables

        #region Private Methods
        private void HandleWelcomeDialog()
        {
            if (_ShowingWelcomeMsg && Vector3.Distance(transform.position, PlayerSystem.PlayerTr.position) > 20f)
            {
                _ShowingWelcomeMsg = false;
                DialogSystem.HideMessage(_Id);
            }
        }

        private void HandleAnimalsDialogs()
        {
            if (Vector3.Distance(transform.position, PlayerSystem.PlayerTr.position) < 5f)
            {
                if (Game.TamedAnimals.Count >= _MinimalCountOfAnimalsToTake)
                {
                    TakeAnimals();
                }
                else
                {
                    WaitForMoreAnimals();
                }
            }
            else
            {
                DialogSystem.HideMessage(_Id);
            }
        }

        private void WaitForMoreAnimals()
        {
            if (Game.TamedAnimals.Count > 0 && Game.TamedAnimals.Count < _MinimalCountOfAnimalsToTake)
            {
                DialogSystem.ShowMessage(_Id, _DialogHolder, _ToLittleAnimalsMsg);
            }
            else
            {
                DialogSystem.ShowMessage(_Id, _DialogHolder, _WithoutAnimalsMsg);
            }
        }

        private void TakeAnimals()
        {
            _IsTakingAnimals = true;

            DialogSystem.ShowMessage(_Id, _DialogHolder, _WithAnimalsMsg);

            List<Animal> animalsToBeTaken = new List<Animal>();
            animalsToBeTaken.AddRange(Game.TamedAnimals);

            foreach (var animal in animalsToBeTaken)
            {
                animal.TakenByCostanza(_HisHouse);
                animal.ReachedPlaceToGo += OnAnimalReachedHouse;
            }
        }

        private void OnAnimalReachedHouse(Animal animal)
        {
            animal.gameObject.transform.DOMove(_DropPos.position, 1f).OnComplete(() =>
            {
                animal.gameObject.AddComponent<Rigidbody>();
            });

            _IsTakingAnimals = false;
        }

        private void HandleDemonicBehaviour()
        {
            DialogSystem.ShowMessage(_Id, _DialogHolder, _AfterGoodPlaceMsg);

            _NavMeshAgent.SetDestination(PlayerSystem.PlayerTr.position);

            TryAttack();

            if (!_IsDemon) { TransformIntoDemon(); }
        }

        private void TryAttack()
        {
            if (Time.time < _NextAttackTime) { return; }

            var hits = Physics.OverlapSphere(transform.position, _AttackRange);

            foreach (var hit in hits)
            {
                var animal = hit.GetComponentInParent<Animal>();
                if (animal != null)
                {
                    animal.Hit();
                }

                var player = hit.GetComponentInParent<PlayerCharacter>();
                if (player != null)
                {
                    player.GetHit();
                }
            }

            _AttackEffect.Play();
            _NextAttackTime = Time.time + _AttackCooldown;
        }

        private void TransformIntoDemon()
        {
            transform.localScale *= 2f;
            _MeshRenderer.material = _DemonicMaterial;

            _IsDemon = true;
        }
        #endregion Private Methods
    }
}