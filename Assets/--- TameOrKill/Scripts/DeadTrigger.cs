namespace dieow.krakjam2022
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class DeadTrigger : MonoBehaviour
    {
        #region Unity Methods
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            Game.Dead();
        }
        #endregion Unity Methods
    }
}