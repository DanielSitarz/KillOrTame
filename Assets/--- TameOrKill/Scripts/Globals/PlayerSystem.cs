namespace dieow.krakjam2022
{
    using UnityEngine;

    public sealed class PlayerSystem : MonoBehaviour
    {
        #region Public Variables
        public static Transform PlayerTr => _Instance._PlayerTr;
        public static Transform PlayerCamera => _Instance._PlayerCamera;
        #endregion Public Variables

        #region Public Methods

        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Transform _PlayerTr;
        [SerializeField] private Transform _PlayerCamera;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Instance = this;
        }
        #endregion Unity Methods

        #region Private Variables
        private static PlayerSystem _Instance;
        #endregion Private Variables

        #region Private Methods

        #endregion Private Methods
    }
}