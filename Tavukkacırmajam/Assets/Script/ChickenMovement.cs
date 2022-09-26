using UnityEngine;

namespace Script
{
    public class ChickenMovement : MonoBehaviour
    {
        public enum TaskCycles
        {
            Patrol,
            Follow,
            Eating,
            Patrolspawm
        }

        [SerializeField] public float startWaitTime;
        [SerializeField] private TaskCycles taskCycle;
        [SerializeField] private float speed;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;


        public Transform moveSpot;
        public Transform spawnPoint;
        public bool isReached;
        private Transform _target;
        private float _waitTime;
        private float _patrolTimer;
        private static readonly int İsEating = Animator.StringToHash("isEating");

        void Start()
        {
            TargetManager.Instance.AddChicken(this);
            moveSpot.SetParent(null);
            spawnPoint.SetParent(null);
            _target = null;
            taskCycle = TaskCycles.Patrol;
            _waitTime = startWaitTime;
            PatrolPosition();
        }


        void Update()
        {
            FindSeed();


            if (_target == null)
            {
                taskCycle = TaskCycles.Patrol;
            }
            else if (_target != null && !isReached)
            {
                taskCycle = TaskCycles.Follow;
            }
            else
            {
                taskCycle = TaskCycles.Eating;
            }

            if (Vector2.Distance(transform.position, spawnPoint.position) < 0.2f)
            {
                taskCycle = TaskCycles.Patrolspawm;
            }


            switch (taskCycle)
            {
                case TaskCycles.Follow:

                    animator.SetBool(İsEating, false);
                    transform.position = Vector2.MoveTowards(transform.position,
                        _target.position, speed * Time.deltaTime);
                    if (transform.position == _target.position)
                    {
                        PatrolPosition();
                    }

                    FlipSprite(_target);
                    break;


                case TaskCycles.Patrol:

                    animator.SetBool(İsEating, false);
                    transform.position = Vector2.MoveTowards(transform.position,
                        moveSpot.position, speed * Time.deltaTime);
                    if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
                    {
                        if (_waitTime <= 0)
                        {
                            PatrolPosition();
                            _waitTime = startWaitTime;
                        }
                        else
                        {
                            _waitTime -= Time.deltaTime;
                        }
                    }

                    FlipSprite(moveSpot);
                    break;

                case TaskCycles.Eating:

                    animator.SetBool(İsEating, true);
                    break;

                case TaskCycles.Patrolspawm:
                    animator.SetBool(İsEating, false);
                    transform.position = Vector2.MoveTowards(transform.position,
                        moveSpot.position, speed * Time.deltaTime);
                    if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
                    {
                        if (_waitTime <= 0)
                        {
                            PatrolPosition();
                            _waitTime = startWaitTime;
                        }
                        else
                        {
                            _waitTime -= Time.deltaTime;
                        }
                    }

                    FlipSprite(moveSpot);
                    break;
            }
        }

        public void FindSeed()
        {
            _target = TargetManager.Instance.FindClosestSeedTarget(gameObject.transform.position, this);
        }

        public void PatrolPosition()
        {
            moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Trap"))
            {
                //todo:add point
                TargetManager.Instance.RemoveChicken(this);
                Destroy(gameObject);
            }

            if (other.CompareTag("Obstacle"))
            {
                moveSpot.position = spawnPoint.position;
            }
        }


        private void FlipSprite(Transform dest)
        {
            spriteRenderer.flipX = !(transform.position.x - dest.position.x < 0);
        }
    }
}