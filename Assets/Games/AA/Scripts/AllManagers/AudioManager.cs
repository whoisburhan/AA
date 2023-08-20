using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.AA
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource audioSource;

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Play(AudioClip _audioClip)
        {
            audioSource.PlayOneShot(_audioClip);
        }
    }
}