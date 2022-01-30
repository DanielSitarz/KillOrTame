namespace dieow.krakjam2022
{
    using TMPro;
    using UnityEngine;

    public sealed class DialogBox : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private TextMeshProUGUI _Content;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
            if (gameObject.activeSelf)
            {
                LookAtCameraAndFollow();
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private Transform _Target;
        #endregion Private Variables

        #region Private Methods
        public void Set(string msg, Transform worldTarget)
        {
            _Content.text = msg;
            _Target = worldTarget;
            LookAtCameraAndFollow();
        }

        private void LookAtCameraAndFollow()
        {
            transform.LookAt(PlayerSystem.PlayerCamera);
            transform.position = _Target.position + _Target.up * 1.7f;
        }
        #endregion Private Methods
    }
}