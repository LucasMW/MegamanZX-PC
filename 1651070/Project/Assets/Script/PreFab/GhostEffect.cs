using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public float GhostDelay;
    private float currentDelay;
    public GameObject AfterImage;
    public bool makeAfterimage = false;
    void Start()
    {
        currentDelay = GhostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeAfterimage)
        {
            if (currentDelay > 0)
            {
                currentDelay -= Time.deltaTime;
            }
            else
            {
                currentDelay = GhostDelay;
                GameObject currentAfterImage = Instantiate(AfterImage, transform.position, transform.rotation);
                currentAfterImage.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                currentAfterImage.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                currentAfterImage.GetComponent<PlayerControl>().enabled = false;
                currentAfterImage.GetComponent<DashEffect>().enabled = false;
            }
        }
    }
}
