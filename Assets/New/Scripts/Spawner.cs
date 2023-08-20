using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FantasyRealm.AA
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField] private TextMesh text;
		public GameObject pinPrefab;


        private void Start()
        {
			UpdateText(GameManager.Instance.PinLeftToCompleteLevel);
        }

		public void UpdateText(int no) 
		{
			text.text = no.ToString();
		}

        void Update()
		{
			if (Input.GetMouseButtonDown(0) && GameManager.Instance.PinLeftToCompleteLevel > 0)
			{
				SpawnPin();
			}
		}

		void SpawnPin()
		{			
			var _go = Instantiate(pinPrefab, transform.position, transform.rotation);
            GameManager.Instance.TriggerPinFireSound();
            _go.GetComponent<Pin>().SetText(GameManager.Instance.PinLeftToCompleteLevel);
			UpdateText(GameManager.Instance.PinLeftToCompleteLevel-1);
            
        }

	}
}