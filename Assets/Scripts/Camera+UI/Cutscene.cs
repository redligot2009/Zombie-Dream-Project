using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Cutscene : MonoBehaviour {

    public List<Sprite> frames;
    public int curr = 0;
    Image image;

    public void NextFrame()
    {
        curr = Mathf.Min(curr + 1, frames.Count - 1);
    }
    public void PrevFrame()
    {
        curr = Mathf.Max(0, curr - 1);
    }

    void Start () {
        image = GetComponent<Image>();
	}
	
	void Update () {
        //control frames
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            PrevFrame();
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            NextFrame();
        }
        if (frames.Count > 0)
        {
            image.sprite = frames[curr];
        }
	}
}
