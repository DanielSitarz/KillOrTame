namespace dieow.krakjam2022
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Tree : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private GameObject _BranchTemplate;
        [SerializeField] private Vector2 _BranchesCountRange;
        [SerializeField] private Vector2 _RandomDistance;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            _BranchTemplate.SetActive(false);

            _BranchesCount = Mathf.CeilToInt(Random.Range(_BranchesCountRange.x, _BranchesCountRange.y));

            SpawnBranches();
        }
        #endregion Unity Methods

        #region Private Variables
        private List<Transform> _Branches = new List<Transform>();
        private int _BranchesCount;
        #endregion Private Variables

        #region Private Methods
        private void SpawnBranches()
        {
            for (var i = 0; i < _BranchesCount; i++)
            {
                var branch = Instantiate(_BranchTemplate, _BranchTemplate.transform.parent);

                SetupBranch(branch.transform);
                _Branches.Add(branch.transform);

                branch.SetActive(true);
            }
        }

        private void SetupBranch(Transform branch)
        {
            var lastPos = _Branches.Count > 0 ? _Branches[^1].localPosition : _BranchTemplate.transform.localPosition;

            branch.transform.localPosition = new Vector3(lastPos.x, lastPos.y + Random.Range(_RandomDistance.x, _RandomDistance.y), lastPos.z);
            branch.transform.Rotate(Vector3.left, Random.Range(0, 180f));
            branch.transform.localScale = new Vector3(1f, Random.Range(0.5f, 2.5f) * (1 - (float) _Branches.Count / _BranchesCount), 1f);
        }
        #endregion Private Methods
    }
}