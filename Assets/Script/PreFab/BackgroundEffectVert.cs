using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEffectVert : MonoBehaviour
{
    public GameObject bottom;
    public float length, height;
    private Vector2 startpos;
    public GameObject camera;
    public float parralaxEffectx;
    public float parralaxEffecty;
    public float minHeight;
    public float prevpos;
    // Start is called before the first frame update
    void Start()
    {
        startpos.x = transform.position.x;
        startpos.y = transform.position.y;
        prevpos = minHeight;
        length = bottom.GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float tempx = (camera.transform.position.x * (1 - parralaxEffectx));
        float distx = (camera.transform.position.x * parralaxEffectx);
        float disty = 0;
        float tempy = 0;
        if (camera.transform.position.y > minHeight) {
            tempy = prevpos + (camera.transform.position.y - prevpos) *(1 - parralaxEffecty);
            disty = (camera.transform.position.y - prevpos) * parralaxEffecty;
            transform.position = new Vector3(startpos.x + distx, startpos.y + disty, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(startpos.x + distx, minHeight, transform.position.z);
        }
        
        if (tempx > startpos.x + length) { startpos.x += length; }
        else if (tempx < startpos.x - length)
        {
            startpos.x -= length;
        }
        if (camera.transform.position.y > minHeight)
        {
            if (tempy > startpos.y + height) { startpos.y += height; prevpos = startpos.y; }
            else if (tempy < startpos.y - height)
            {
                startpos.y -= height;
                prevpos = startpos.y;
            }
        }
    }
}
