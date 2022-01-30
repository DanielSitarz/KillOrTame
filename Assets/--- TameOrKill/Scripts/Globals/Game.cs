namespace dieow.krakjam2022
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Game : MonoBehaviour
    {
        #region Public Variables
        public static event Action AnimalsAreTakenToGoodPlace;

        public static List<Animal> TamedAnimals => _TamedAnimals;
        public static List<Animal> LivingAnimals => _LivingAnimals;

        public static int _AllTimeTamedAnimals;
        public static int _AllTimeKilledAnimals;
        public static int _AllTimeTakenByCostanza;
        public static int _AllTimeAnimalsTakenToGoodPlace;

        public static float _GameStartedTime;
        #endregion Public Variables

        #region Public Methods
        public static void Dead()
        {
            _Instance._DeadScreen.SetActive(true);
            PlayerSystem.PlayerTr.gameObject.SetActive(false);
        }

        public static void Win()
        {
            _Instance._WinScreen.SetActive(true);
        }

        public static void TameAnimal(Animal animal)
        {
            _TamedAnimals.Add(animal);
            _AllTimeTamedAnimals++;
        }

        public static void TakenByCostanza(Animal animal)
        {
            _TamedAnimals.Remove(animal);
            _AllTimeTakenByCostanza++;

            _LivingAnimals.Remove(animal);
        }

        public static void TakenToGoodPlace(Animal animal)
        {
            _TamedAnimals.Remove(animal);
            _AllTimeAnimalsTakenToGoodPlace++;

            _LivingAnimals.Remove(animal);
        }

        public static void TakeAnimalsToGoodPlace()
        {
            AnimalsAreTakenToGoodPlace?.Invoke();
        }

        public static void KillAnimal(Animal animal)
        {
            _KilledAnimals.Add(animal);
            _AllTimeKilledAnimals++;

            _TamedAnimals.Remove(animal);
            _LivingAnimals.Remove(animal);

            if (_LivingAnimals.Count == 0)
            {
                _Instance._NoMoreAnimalsScreen.SetActive(true);
            }
        }

        public static void AddAnimal(Animal animal)
        {
            _LivingAnimals.Add(animal);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private GameObject _DeadScreen;
        [SerializeField] private GameObject _WinScreen;
        [SerializeField] private GameObject _NoMoreAnimalsScreen;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Instance = this;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _TamedAnimals.Clear();
            _KilledAnimals.Clear();
            _LivingAnimals.Clear();

            _AllTimeAnimalsTakenToGoodPlace = 0;
            _AllTimeKilledAnimals = 0;
            _AllTimeTakenByCostanza = 0;
            _AllTimeTamedAnimals = 0;

            _GameStartedTime = Time.realtimeSinceStartup;
        }
        #endregion Unity Methods

        #region Private Variables
        private static List<Animal> _TamedAnimals = new List<Animal>();
        private static List<Animal> _KilledAnimals = new List<Animal>();
        private static List<Animal> _LivingAnimals = new List<Animal>();
        private static Game _Instance;
        #endregion Private Variables
    }
}