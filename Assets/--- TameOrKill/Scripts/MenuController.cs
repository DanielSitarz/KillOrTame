namespace dieow.krakjam2022
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MenuController : MonoBehaviour
    {
        #region Public Methods
        public void StartGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void Exit()
        {
            Application.Quit();
        }
        #endregion Public Methods
    }
}