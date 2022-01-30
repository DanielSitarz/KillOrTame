
namespace dieow.krakjam2022
{
    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.AI;
    using Random = UnityEngine.Random;

    public class Animal : MonoBehaviour
    {
        #region Public Types
        public enum Status
        {
            Wild,
            Tamed,
            Dead,
            Costanzed,
            GoodPlaced
        }
        #endregion Public Types

        #region Public Variables
        public string Name => _Name;
        public event Action<Animal> ReachedPlaceToGo;
        #endregion Public Variables

        #region Public Methods
        public void Tame()
        {
            if (_Status != Status.Wild) { return; }
            if (_Aggressive) { return; }

            _TameTries++;

            WorldCommunicator.Communicate($"{_Name} oswaja się");

            if (_TameTries < 3) { return; }

            BecomeTamed();
        }

        public void Hit()
        {
            if (_Status == Status.Dead) { return; }

            _Health--;
            MakeAggressive();
            _TameParticles.SetActive(false);

            WorldCommunicator.Communicate($"{_Name} oberwał! (Życie: {_Health})");

            if (_Health > 0) { return; }

            DOVirtual.DelayedCall(1f, () => _AggresionParticles.SetActive(false));
            DialogSystem.HideMessage(_Id);
            DoDeadStuff();
        }

        public void TakenByCostanza(Transform hisHouse)
        {
            Game.TakenByCostanza(this);
            _PlaceToGo = hisHouse;
            _Status = Status.Costanzed;
        }

        public void GoToGoodPlace(Transform goodPlace)
        {
            _PlaceToGo = goodPlace;
            _Status = Status.GoodPlaced;
            _GoodPlaceParticles.SetActive(true);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private AnimationCurve _AttackSpeedToHealth;
        [SerializeField] private AnimationCurve _FleeSpeedToTameValue;
        [SerializeField] private float _WanderSpeed = 5f;
        [SerializeField] private float _AttackDmgRange = 3f;
        [SerializeField] private float _AttackDmgCooldown = 1f;

        [SerializeField] private Vector2 _SmellRange;
        [SerializeField, Range(0.01f, 1f)] private float _AggresionLevel;
        [SerializeField] private NavMeshAgent _NavMeshAgent;

        [SerializeField] private GameObject _AggresionParticles;
        [SerializeField] private GameObject _TameParticles;
        [SerializeField] private GameObject _GoodPlaceParticles;
        [SerializeField] private ParticleSystem _AttackEffect;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            Game.AddAnimal(this);

            _Name = Names.GetRandomName();

            _PlayerDetectionRange = Random.Range(_SmellRange.x, _SmellRange.y) * (1f + _AggresionLevel);

            _AggresionParticles.SetActive(false);
            _TameParticles.SetActive(false);
            _GoodPlaceParticles.SetActive(false);

            if (Random.value < _AggresionLevel)
            {
                MakeAggressive();
            }
        }

        private void Update()
        {
            if (_Status == Status.Dead) { return; }

            _SmellsPlayer = _DistanceToPLayer < _PlayerDetectionRange;

            switch (_Status)
            {
                case Status.Wild:
                    DoWildStuff();
                    break;
                case Status.Tamed:
                    DoTamedStuff();
                    break;
                case Status.Costanzed:
                    DoCostanzaStuff();
                    break;
                case Status.GoodPlaced:
                    DoGoodPlace();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private string _Name;
        private Status _Status = Status.Wild;

        private int _Health = 3;
        private int _TameTries = 0;

        private float _PlayerDetectionRange;
        private float _NextAttackTime;

        private bool _Aggressive;
        private bool _SmellsPlayer;

        private float _DistanceToPLayer => Vector3.Distance(transform.position, PlayerSystem.PlayerTr.position);

        private Transform _PlaceToGo;
        private string _Id = Guid.NewGuid().ToString();
        #endregion Private Variables

        #region Private Methods
        private void MakeAggressive()
        {
            _Aggressive = true;
            gameObject.tag = "AggressiveAnimal";
            _AggresionParticles.SetActive(true);
        }

        private void DoTamedStuff()
        {
            _NavMeshAgent.SetDestination(PlayerSystem.PlayerTr.position);
        }

        private void DoWildStuff()
        {
            if (_SmellsPlayer)
            {
                if (_Aggressive && _TameTries == 0)
                {
                    Attack();
                }
                else
                {
                    Flee();
                }
            }
            else
            {
                WanderAround();
            }
        }

        private void DoDeadStuff()
        {
            _Status = Status.Dead;
            WorldCommunicator.Communicate($"{_Name} umarł");
            Game.KillAnimal(this);

            _NavMeshAgent.enabled = false;
            transform.localPosition -= transform.up * 0.1f;
            transform.Rotate(transform.forward, 90f);

            _AggresionParticles.SetActive(false);
            _TameParticles.SetActive(false);
        }
        private void DoCostanzaStuff()
        {
            _NavMeshAgent.speed = 2.5f;
            MoveToPlace();
        }

        private void DoGoodPlace()
        {
            _NavMeshAgent.speed = 4f;
            MoveToPlace();
        }

        private void MoveToPlace()
        {
            _TameParticles.SetActive(false);
            _NavMeshAgent.SetDestination(_PlaceToGo.position);

            if (enabled && Vector3.Distance(transform.position, _PlaceToGo.position) < 1f)
            {
                _NavMeshAgent.enabled = false;
                enabled = false;

                ReachedPlaceToGo?.Invoke(this);
            }
        }

        private void Attack()
        {
            _NavMeshAgent.speed = _AttackSpeedToHealth.Evaluate(_Health);
            _NavMeshAgent.SetDestination(PlayerSystem.PlayerTr.position);

            TryHitSomebody();
        }

        private void TryHitSomebody()
        {
            if (_NavMeshAgent.remainingDistance < _AttackDmgRange && Time.time > _NextAttackTime)
            {
                var hits = Physics.OverlapSphere(transform.position, _AttackDmgRange);

                foreach (var hit in hits)
                {
                    var animal = hit.GetComponentInParent<Animal>();
                    if (animal != null && animal != this)
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
                _NextAttackTime = Time.time + _AttackDmgCooldown;
            }
        }

        private void Flee()
        {
            _NavMeshAgent.speed = _FleeSpeedToTameValue.Evaluate(_TameTries);

            var posAway = transform.position + (transform.position - PlayerSystem.PlayerTr.position).normalized * 8f;
            _NavMeshAgent.SetDestination(posAway);
        }

        private void WanderAround()
        {
            if (Vector3.Distance(transform.position, _NavMeshAgent.destination) > 1f)
            {
                return;
            }

            _NavMeshAgent.speed = _WanderSpeed;

            var randomPosAround = transform.position + Random.insideUnitSphere * 10f;
            _NavMeshAgent.SetDestination(randomPosAround);
        }

        private void BecomeTamed()
        {
            WorldCommunicator.Communicate($"{_Name} lubi cię!");

            _Status = Status.Tamed;

            Game.TameAnimal(this);

            _TameParticles.SetActive(true);

            DialogSystem.ShowMessage(_Id, transform, _Name);
        }
        #endregion Private Methods
    }
}