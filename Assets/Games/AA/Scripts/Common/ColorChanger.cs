using System;
using UnityEngine;
using UnityEngine.UI;

namespace GS.AA
{
    public class ColorChanger : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.OnColorSet += SetColorInObject;
        }

        private void OnDisable()
        {
            GameManager.OnColorSet -= SetColorInObject;
        }

        public void SetColorInObject(Color _color)
        {
            try
            {
                SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = _color;
                }
            } catch (Exception e) { }

            try
            {
                Image img = this.GetComponent<Image>();
                if(img != null)
                {
                    img.color = _color;
                }
            }catch(Exception e) { }

            try
            {
                Text txt = this.GetComponent<Text>();
                if(txt != null)
                {
                    txt.color = _color;
                }
            }catch(Exception e) { }
        }
    }
}