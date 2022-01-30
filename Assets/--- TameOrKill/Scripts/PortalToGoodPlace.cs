namespace dieow.krakjam2022
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PortalToGoodPlace : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Transform _GoodPlace;
        #endregion Inspector Variables

        #region Unity Methods
        void Update()
        {
            if (_IsTakingAnimalsToGoodPlace) { return; }

            if (Vector3.Distance(transform.position, PlayerSystem.PlayerTr.position) < 8f)
            {
                if (Game.TamedAnimals.Count > 0)
                {
                    TakeAnimalsToGoodPlace();
                }
                else
                {
                    WorldCommunicator.Communicate("Czujesz dziwną energię...");
                }
            }
        }

        private void TakeAnimalsToGoodPlace()
        {
            Game.TakeAnimalsToGoodPlace();
            _IsTakingAnimalsToGoodPlace = true;

            WorldCommunicator.Communicate("Twoje zwierzaczki odchodzą do lepszego miejsca");

            List<Animal> animalsToBeTaken = new List<Animal>();
            animalsToBeTaken.AddRange(Game.TamedAnimals);

            foreach (var animal in animalsToBeTaken)
            {
                animal.GoToGoodPlace(_GoodPlace);
                animal.ReachedPlaceToGo += OnReachedGoodPlace;
            }
        }

        private void OnReachedGoodPlace(Animal animal)
        {
            Game.TakenToGoodPlace(animal);
            WorldCommunicator.Communicate($"{animal.Name} trafił do lepszego miejsca");
            Destroy(animal.gameObject);
        }
        #endregion Unity Methods

        #region Private Variables
        private bool _IsTakingAnimalsToGoodPlace;
        #endregion Private Variables
    }
}