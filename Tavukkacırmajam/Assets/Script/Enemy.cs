using System;
using UnityEngine;

namespace Script
{
    public class Enemy : MonoBehaviour
    {
        public float speed;

        [SerializeField]private Transform _target;

        // private void Start()
        // {
        //     _target = GameObject.FindGameObjectWithTag("Seed").GetComponent<Transform>();
        // }

        void Update()
        {
            if (_target != null)
            { transform.position = Vector2.MoveTowards(transform.position,
                                    _target.position, speed * Time.deltaTime);
            }
            
            if (_target == null)
            {
                _target = GameObject.FindGameObjectWithTag("Seed").GetComponent<Transform>();
              
            }
        }
    }
}