using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public float GhostDelay;
    private float currentDelay;
    public bool makeAfterimage = false;
    private ObjectPooler objectPooler;
    public string Spawning;
    /*private Transform[] afterImageTransform = new Transform[2];
    private Vector3 lastPos, lastscale;
    private Sprite lastSprite;
    private float counter;
    private bool first;
        bool second;*/
    void Start()
    {
        /*first = true;
        second = false;
        counter = 0;*/
        currentDelay = GhostDelay;
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeAfterimage)
        {
            #region OriginalAfterimage
            /*AfterImage[0].SetActive(true);
            AfterImage[1].SetActive(true);
            if (first)
            {
                AfterImage[1].transform.position = AfterImage[0].transform.position = transform.position;
                AfterImage[1].transform.localScale = AfterImage[0].transform.localScale = transform.localScale;
                AfterImage[0].GetComponent<SpriteRenderer>().enabled = true;
                AfterImage[1].GetComponent<SpriteRenderer>().enabled = true;
                counter = 0;
                first = false;
            }
            else
            {
                switch (counter)
                {
                    case 0:
                        {
                            AfterImage[0].GetComponent<SpriteRenderer>().enabled = false;
                            AfterImage[1].GetComponent<SpriteRenderer>().enabled = false;
                            lastPos = transform.position;
                            lastscale = transform.localScale;
                            lastSprite = GetComponent<SpriteRenderer>().sprite;
                            counter++;
                            break;
                        }
                    case 3:
                        {
                            AfterImage[1].transform.position = AfterImage[0].transform.position;
                            AfterImage[1].transform.localScale = AfterImage[0].transform.localScale;
                            AfterImage[0].transform.position = lastPos;
                            AfterImage[0].transform.localScale = lastscale;

                            AfterImage[1].GetComponent<SpriteRenderer>().sprite = AfterImage[0].GetComponent<SpriteRenderer>().sprite = lastSprite;
                            
                            AfterImage[0].GetComponent<SpriteRenderer>().enabled = true;
                            AfterImage[1].GetComponent<SpriteRenderer>().enabled = true;
                            counter = 0;
                            if (second)
                            {
                                Debug.Break();
                            }
                            second = true;
                            break;
                        }
                    default:
                        {
                            counter++;
                            break;
                        }
                }
            }*/
            #endregion
            if (currentDelay > 0)
            {
                currentDelay -= Time.deltaTime;
            }
            else
            {
                currentDelay = GhostDelay;
                GameObject currentAfterImage = objectPooler.SpawnFromPool(Spawning, transform.position, transform.rotation);  //Instantiate(AfterImage, transform.position, transform.rotation);
                currentAfterImage.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                currentAfterImage.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
