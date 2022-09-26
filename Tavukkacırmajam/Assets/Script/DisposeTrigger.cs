using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class DisposeTrigger : MonoBehaviour
    {
        [SerializeField] public GameObject seed;
        [SerializeField] private float seedCountDown = 2f;
        [SerializeField] private bool isStarted;
        private ChickenMovement _chickenMovement;
        public List<ChickenMovement> chickenMovements = new List<ChickenMovement>();

        public void Update()
        {
            if (isStarted)
            {
                seedCountDown -= Time.deltaTime;
                if (seedCountDown <= 0)
                {
                    foreach (var t in chickenMovements)
                    {
                        t.PatrolPosition();
                        t.isReached = false;
                    }

                    TargetManager.Instance.RemoveSeed(gameObject.transform);
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Chicken"))
            {
                isStarted = true;
                _chickenMovement = other.GetComponent<ChickenMovement>();
                _chickenMovement.isReached = true;
                chickenMovements.Add(_chickenMovement);
            }
        }

        private void RemoveSeed()
        {
        }
    }
}