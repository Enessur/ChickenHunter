using Unity.Mathematics;
using UnityEngine;

namespace Script
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject seedPrefab;
        [SerializeField] private float seedInterval = 1.5f;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        private float _seedTimer;
        private bool _canProduce = true;
        private Vector2 _movement;
        private static readonly int isFoxRunning = Animator.StringToHash("isFoxRunning");

        void Update()
        {
            _seedTimer += Time.deltaTime;
            if (_seedTimer >= seedInterval)
            {
                _seedTimer = 0;
                _canProduce = true;
            }

            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool(isFoxRunning, true);
                spriteRenderer.flipX = true;
            }

            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetBool(isFoxRunning, true);
                spriteRenderer.flipX = false;
            }
            else
            {
                animator.SetBool(isFoxRunning, false);
            }
            


            if (Input.GetKey(KeyCode.Space))
            {
                SpawnSeed();
            }
        }
        

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
        }

        private void SpawnSeed()
        {
            if (_canProduce)
            {
                SoundManager.Instance.PlaySound("seed");
                var mySeed = Instantiate(seedPrefab, transform.position, quaternion.identity);
                TargetManager.Instance.AddSeed(mySeed.transform);
                TargetManager.Instance.CheckSeedForChickens();
                _canProduce = false;
            }

          
        }
    }
}