namespace dieow.krakjam2022
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PauseHandler : MonoBehaviour
    {
        #region Public Methods
        public void ExitToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Quit()
        {
            Application.Quit();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private GameObject _PauseScreen;
        #endregion Inspector Variables

        #region Unity Methods
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var active = !_PauseScreen.activeSelf;
                _PauseScreen.SetActive(active);
                Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        #endregion Unity Methods
    }
}