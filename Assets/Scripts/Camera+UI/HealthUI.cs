using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour {

    public Transform target;
    Health health;
    public Sprite[] heartSprites;
    public List<GameObject> hearts;
    public float offsetX = 0;

    void updateHeart(int idx, float health)
    {
        int i = 0;
        for (float rem = 0; rem <= 1; rem += 0.25f, i++)
        {
            if (health >= 1 - rem)
            {
                hearts[idx].GetComponent<SpriteRenderer>().sprite = heartSprites[i];
                break;
            }
        }
    }
    void Start () {
        health = target.GetComponent<Health>();
        //Instantiate(, transform);
        float curr = health.health;
        int idx = 0;
        float heartWidth = heartSprites[0].bounds.size.x;
        float totalWidth = 0;
        while(curr > 0)
        {
            GameObject heart = Instantiate(new GameObject(), transform);
            heart.AddComponent<SpriteRenderer>();
            SpriteRenderer spriteRenderer = heart.GetComponent<SpriteRenderer>();
            totalWidth += heartWidth + offsetX;
            heart.transform.localPosition = new Vector3(idx * (heartWidth+offsetX), 0, 1);
            hearts.Add(heart);
            updateHeart(idx, curr);
            idx++;
            curr--;
        }
        transform.localPosition += new Vector3((-totalWidth/2f * transform.localScale.x) + heartWidth/4, 0);
	}
	
	void Update ()
    {
        if (!health.dead)
        {
            if (health.hitTimer > 0)
            {
                for (int i = 0; i < hearts.Count; i++)
                {
                    updateHeart(i, health.health - i);
                }
            }
        }
        else
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                SpriteRenderer r = hearts[i].GetComponent<SpriteRenderer>();
                Color c = r.color;
                if (c.a > 0.01f)
                {
                    c.a = Mathf.Lerp(c.a, 0, Time.deltaTime * 5f);
                    r.color = c;
                }
            }
        }
	}
}
