using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[ExecuteInEditMode]
public class Cutscene : MonoBehaviour {

    public List<Sprite> frames;
    public int curr = 0;
    Image image;
    public bool autoplay = false;
    public bool fadeTransition = true;

    public string nextScene = "";

    IEnumerator Fade(float duration = 1f, float alpha=0)
    {
        float start = Time.time;
        float elapsed = 0;
        Color endC = image.color;
        endC.a = alpha;
        while(elapsed < duration)
        {
            float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
            elapsed += Time.deltaTime;
            image.color = Color.Lerp(image.color,endC,normalisedTime);
            yield return null;
        }
    }
    public bool finished = true;
    IEnumerator Wait(float seconds = 0, bool next = true)
    {
        int newFrame = (next ? Mathf.Min(curr + 1, frames.Count - 1) : Mathf.Max(curr - 1, 0));
        finished = false;
        if (newFrame != curr)
        {
            if(fadeTransition)
                StartCoroutine(Fade(seconds / 2, 0));
            yield return new WaitForSeconds(seconds / 2);
            curr = newFrame;
            if(fadeTransition)
                StartCoroutine(Fade(seconds / 2, 1));
            yield return new WaitForSeconds(seconds / 2);
        }
        else
        {
            if(next)
            {
                if(SceneManager.GetSceneByName(nextScene)!=null)
                {
                    SceneManager.LoadScene(nextScene);
                }
            }
        }
        yield return null;
        finished = true;
    }

    public void NextFrame(float duration = 1)
    {
        StartCoroutine(Wait(duration,true));
    }
    public void PrevFrame(float duration = 1)
    {
        StartCoroutine(Wait(duration, false));
    }

    void Start () {
        image = GetComponent<Image>();
	}
	
	void Update () {
        //control frames
        if (!autoplay)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && finished)
            {
                PrevFrame();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && finished)
            {
                NextFrame();
            }
        }
        else
        {
            if (Application.isPlaying)
            {
                if (finished)
                {
                    NextFrame(2);
                }
            }
        }
        if (frames.Count > 0)
        {
            image.sprite = frames[curr];
        }
	}
}
