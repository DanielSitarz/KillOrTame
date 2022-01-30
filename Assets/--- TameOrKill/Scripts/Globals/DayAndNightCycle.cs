namespace dieow.krakjam2022
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.HighDefinition;

    public class DayAndNightCycle : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private int _DayDuration = 60;
        [SerializeField] private int _NightDuration = 30;

        [SerializeField] private float _DayLux = 7000;
        [SerializeField] private float _NightLux = 200;

        [SerializeField] private Light _Light;
        [SerializeField] private VolumeProfile _VolumeProfile;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            _VolumeProfile.TryGet(out _Sky);

            MakeDay();
        }
        #endregion Unity Methods

        #region Private Variables
        private HDRISky _Sky;
        private bool _IsDay;
        private int _DaysCount;
        #endregion Private Variables

        #region Private Methods
        private void MakeDay()
        {
            _DaysCount++;

            WorldCommunicator.Communicate($"Dzień #{_DaysCount}");

            _IsDay = true;

            UpdateWorld();

            Invoke(nameof(MakeNight), _DayDuration);
        }

        private void MakeNight()
        {
            _IsDay = false;

            UpdateWorld();

            Invoke(nameof(MakeDay), _NightDuration);
        }

        private void UpdateWorld()
        {
            _Light.gameObject.SetActive(_IsDay);

            _Sky.desiredLuxValue.value = _IsDay ? _DayLux : _NightLux;
        }
        #endregion Private Methods
    }
}