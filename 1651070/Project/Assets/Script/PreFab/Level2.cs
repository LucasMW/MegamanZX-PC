using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace UnityStandardAssets.Cameras
{
    public class Level2 : MonoBehaviour
    {
        public bool passed = false;
        public GameObject CameraRig;
        public float Yminbefore, Yminafter;
        public GameObject AreaText;
        public string before, after;
        TextMeshProUGUI textbox;
        // Start is called before the first frame update
        void Start()
        {
            textbox = AreaText.GetComponent<TextMeshProUGUI>();
            AreaText.SetActive(false);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (passed == false)
                {
                    textbox.SetText(after);
                    AreaText.SetActive(true);
                    AreaText.GetComponent<Animator>().Play("AreaText");
                    CameraRig.GetComponent<AutoCam>().yMin = Yminafter;
                    passed = true;
                }
                else if (passed)
                {

                    textbox.SetText(before);
                    passed = false;
                    AreaText.SetActive(true);
                    AreaText.GetComponent<Animator>().Play("AreaText");
                    CameraRig.GetComponent<AutoCam>().yMin = Yminbefore;
                }
            }
        }
    }
}