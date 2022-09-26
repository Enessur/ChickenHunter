using System.Collections.Generic;
using UnityEngine;


namespace Script
{
    public class TargetManager : Singleton<TargetManager>
    {
        public List<Transform> seedTransforms = new List<Transform>();
        public List<ChickenMovement> chickenMovement = new List<ChickenMovement>();
        private Vector3 _offset;
        private float _currentDistance;
        private float _closestDistance;
        public GameObject closestTarget;
        public float detectionDistance = 3f;


        public void AddSeed(Transform tr)
        {
            seedTransforms.Add(tr);
        }

        public void RemoveSeed(Transform tr)
        {
            seedTransforms.Remove(tr);
        }

        public void AddChicken(ChickenMovement cm)
        {
            chickenMovement.Add(cm);
        }

        public void RemoveChicken(ChickenMovement cm)
        {
            chickenMovement.Remove(cm);
            CheckWinCondition();
            SoundManager.Instance.PlaySound("chickenDie");
        }

        public void CheckSeedForChickens()
        {
            for (int i = 0; i < chickenMovement.Count; i++)
            {
                chickenMovement[i].FindSeed();
            }
        }


        public Transform FindClosestSeedTarget(Vector3 chickenPosition, ChickenMovement chickenMovement)
        {
            if (seedTransforms.Count != 0)
            {
                _closestDistance = 100000f;
                foreach (var t in seedTransforms)
                {
                    {
                        _offset = chickenPosition - t.gameObject.transform.position;
                        _currentDistance = Vector3.Magnitude(_offset);

                        if (_closestDistance > _currentDistance)
                        {
                            _closestDistance = _currentDistance;
                            closestTarget = t.gameObject;
                        }
                    }
                }

                if (_closestDistance < detectionDistance)
                {
                    return closestTarget.transform;
                }

                return null;
            }

            return null;
        }

        public bool IsChickenEmpty()
        {
            // if (chickenMovement.Count == 0)
            // {
            //     return true;
            // }
            //
            // return false;

             return chickenMovement.Count == 0;
        }

        public void CheckWinCondition()
        {
            if (IsChickenEmpty())
            {
                SceneManager.Instance.WinGame();
            }
        }
    }
}