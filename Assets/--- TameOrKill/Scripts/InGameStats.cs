namespace dieow.krakjam2022
{
    using System;
    using TMPro;
    using UnityEngine;

    public class InGameStats : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private TextMeshProUGUI _GameplayTime;

        [SerializeField] private TextMeshProUGUI _AnimalsSavedText;
        [SerializeField] private TextMeshProUGUI _AnimalsTamedText;
        [SerializeField] private TextMeshProUGUI _AnimalsKilledText;
        [SerializeField] private TextMeshProUGUI _AnimalsCostanzedText;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnEnable()
        {
            InvokeRepeating(nameof(UpdateStats), 1f, 1f);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(UpdateStats));
        }
        #endregion Unity Methods

        #region Private Methods
        private void UpdateStats()
        {
            TimeSpan time = TimeSpan.FromSeconds(Time.realtimeSinceStartup - Game._GameStartedTime);
            string gameplayTimeText = time.ToString(@"mm\:ss");

            _GameplayTime.text = gameplayTimeText;

            _AnimalsCostanzedText.text = Game._AllTimeTakenByCostanza.ToString();
            _AnimalsKilledText.text = Game._AllTimeKilledAnimals.ToString();
            _AnimalsTamedText.text = Game._AllTimeTamedAnimals.ToString();

            _AnimalsSavedText.transform.parent.gameObject.SetActive(Game._AllTimeAnimalsTakenToGoodPlace > 0);
            _AnimalsSavedText.text = Game._AllTimeAnimalsTakenToGoodPlace.ToString();
        }
        #endregion Private Methods
    }
}