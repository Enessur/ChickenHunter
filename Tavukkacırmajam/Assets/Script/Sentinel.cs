using UnityEngine;

namespace Script
{
    public class Sentinel : MonoBehaviour
    {
        public enum TaskCycleDog
        {
            Chase,
            Patrol,
            Stop
        }

        [SerializeField] private TaskCycleDog taskCycleDog;
        [SerializeField] private float speed;
        [SerializeField] private float chaseSpeed;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        [SerializeField] public float startWaitTime = 1f;
        [SerializeField] private float chasingDistance;
        [SerializeField] private float killDistance;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool oneTime;
        public Transform moveSpot;
        private Transform _target;
        private float _patrolTimer;


        void Start()
        {
            oneTime = false;
            moveSpot.SetParent(null);
            _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        void Update()
        {
            if (Vector2.Distance(transform.position, _target.position) < chasingDistance)
            {
                taskCycleDog = TaskCycleDog.Chase;
            }
            else
            {
                taskCycleDog = TaskCycleDog.Patrol;
            }

            if (Vector2.Distance(transform.position, _target.position) < killDistance)
            {
                taskCycleDog = TaskCycleDog.Stop;
            }

            switch (taskCycleDog)
            {
                case TaskCycleDog.Chase:
                    transform.position =
                        Vector2.MoveTowards(transform.position, _target.position, chaseSpeed * Time.deltaTime);
                    FlipSprite(_target);
                    break;
                case TaskCycleDog.Patrol:
                    PatrolPosition();
                    transform.position =
                        Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
                    FlipSprite(moveSpot);
                    break;
                case TaskCycleDog.Stop:
                    GameOver();

                    break;
            }
        }

        private void PatrolPosition()
        {
            _patrolTimer += Time.deltaTime;

            if (!(_patrolTimer >= startWaitTime)) return;
            _patrolTimer = 0;

            moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        }

        private void GameOver()
        {
            if (oneTime) return;
            SoundManager.Instance.PlaySound("dogBark");
            SceneManager.Instance.LoseGame();
            oneTime = true;
        }

        public void FlipSprite(Transform dest)
        {
            spriteRenderer.flipX = !(transform.position.x - dest.position.x < 0);
        }
    }
}