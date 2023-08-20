using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FantasyRealm.AA
{
	public class Pin : MonoBehaviour
	{
        [SerializeField] private ParticleSystem particle;
		[SerializeField] private TextMesh text;

        private bool isPinned = false;

		public float speed = 20f;
		public Rigidbody2D rb;

		void Update()
		{
			if (!isPinned)
				rb.MovePosition(rb.position + Vector2.up * speed * Time.deltaTime);
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.tag == "Rotator")
			{
				transform.SetParent(col.transform);
				//Score.PinCount++;
				GameManager.Instance.PinLeftToCompleteLevel--;
				isPinned = true;

                if (particle.isPlaying)
                    particle.Stop();
                particle.Play();
            }
			else if (col.tag == "Pin")
			{
                FindObjectOfType<GameManager>().EndGame();
			}
		}


		public void SetText(int no) 
		{
			text.text = no.ToString();
		}
	}
}