using UnityEngine;
using System.Collections;

public class LoadManager : Origin {

    // Use this for initialization
    void Start () {

        FrameCount = 0;
	}
	
	// Update is called once per frame
	void Update () {

        FrameCount++;

        if(FrameCount == 120) {

            FadeManager.Instance.LoadLevel(TITLE_SCENE,1.0f);
        }
	}
}
