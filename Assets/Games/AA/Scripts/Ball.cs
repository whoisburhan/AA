using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

namespace GS.AA
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        public TextMeshPro BallNo;

        public Spawner spawnFrom;
        public bool isCurrentBall = true;
        private bool isFired = false;
        private float speed = 300f;
        private Rigidbody2D rb2d;
        [SerializeField] private ParticleSystem particle;

        private void OnEnable()
        {
         //   GameManager.OnGameFinished += AutoDestroy;
            GameManager.OnGameOver += AutoDestroy;
            GameManager.OnLevelCompleted += AutoDestroy;
            GameManager.OnColorSet += SetParticleColor;
            UIManager.OnForceToDestroy += AutoDestroy;
        }

        private void OnDisable()
        {
            // GameManager.OnGameFinished -= AutoDestroy;
            GameManager.OnGameOver -= AutoDestroy;
            GameManager.OnLevelCompleted -= AutoDestroy;
            GameManager.OnColorSet -= SetParticleColor;
            UIManager.OnForceToDestroy -= AutoDestroy;
            
        }

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            SetParticleColor(GetComponent<SpriteRenderer>().color);
        }

        private void SetParticleColor(Color _color)
        {
            var _main = particle.main;
            _main.startColor = _color;
        }

        private void Update()
        {
            if (GameManager.Instance.IsPlay)
            {
                if (isCurrentBall && !isFired)
                {
#if UNITY_ANDROID || UNITY_WEBGL
                    if (Input.touchCount > 0)
                    {
                        Touch[] touches = Input.touches;
                        if (touches[0].phase == TouchPhase.Began)
                        {
                            AudioManager.Instance.Play(GameManager.Instance.BallFireClip);

                            isFired = true;
                            isCurrentBall = false;

                            Delay(GameManager.Instance.spawner[0].SpawnBall);
                            Delay(GameManager.Instance.spawner[1].SpawnBall);
                            // Prepare for next shot
                        }
                    }
#endif
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                    {
                        AudioManager.Instance.Play(GameManager.Instance.BallFireClip);
                        isFired = true;
                        isCurrentBall = false;

                        Delay(GameManager.Instance.spawner[0].SpawnBall);
                        Delay(GameManager.Instance.spawner[1].SpawnBall);
                        // Prepare for next shot
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (isFired)
            {
                rb2d.velocity = transform.up * speed * Time.fixedDeltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(TagsAndPlayerprefs.TARGET_TAG))
            {
                isFired = false;
                rb2d.velocity = Vector2.zero;
                this.transform.parent = col.transform;
                if (col.GetComponent<Circle>().TwoSideRotation)
                {
                    col.GetComponent<Rotator>().IsRotateRight = !col.GetComponent<Rotator>().IsRotateRight;
                }
                spawnFrom.SpawnBall();
                GameManager.Instance.AddBallInCount();
                
                if (particle.isPlaying)
                    particle.Stop();
                particle.Play();
                //Call Delegeate 
            }

            if (col.CompareTag(TagsAndPlayerprefs.BALL_TAG))
            {
                isFired = false;
                rb2d.velocity = Vector2.zero;
                // GameOver
                GameManager.Instance.GameOver();
                Destroy(this.gameObject);
            }
        }

        public void AutoDestroy()
        {
            Destroy(this.gameObject);
        }

        IEnumerator Delay(Action _action, float _delayTime = 0.5f)
        {
            yield return new WaitForSeconds(_delayTime);
            _action();
        }
    }
}