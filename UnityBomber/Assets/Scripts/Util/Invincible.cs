using UnityEngine;
using System.Collections;

public class Invincible : Origin {

    private float nextTime;
    public float interval = 0.8f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Time.time > nextTime) {

            float alpha = GetComponent<CanvasRenderer>().GetAlpha();

            if (alpha == 1.0f){

                GetComponent<CanvasRenderer>().SetAlpha(0.0f);

            }else{

                GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            }

            nextTime += interval;
        }
	}
}
