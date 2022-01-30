namespace dieow.krakjam2022
{
    using UnityEngine;

    public class KillCostanza : MonoBehaviour
    {
        #region Unity Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Costanza"))
            {
                other.GetComponentInParent<MrCostanza>().DropDead();
                Game.Win();
            }
        }
        #endregion Unity Methods
    }
}
