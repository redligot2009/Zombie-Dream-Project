using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour {

    public Transform target;
    Health health;
    public Sprite[] heartSprites;
    public List<GameObject> hearts;
    public float offsetX = 0;
    public bool center = true;
    public int heartCount = 3;
    public Color heartTint;

    void UpdateHeart(int idx, float health)
    {
        int i = 0;
        for (float rem = 0; rem <= 1; rem += 0.25f, i++)
        {
            if (health >= Mathf.Max(1 - rem,0))
            {
                hearts[idx].GetComponent<SpriteRenderer>().sprite = heartSprites[i];
                break;
            }
        }
    }
    void Start () {
        health = target.GetComponent<Health>();
        float curr = ((health.health-1)%heartCount)+1;
        int idx = 0;
        float heartWidth = heartSprites[0].bounds.size.x;
        float totalWidth = 0;
        GameObject orig = new GameObject("heart", typeof(SpriteRenderer));
        while (idx < heartCount)
        {
            GameObject heart = Instantiate(orig, transform);
            SpriteRenderer spriteRenderer = heart.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 2;
            spriteRenderer.color = heartTint;
            totalWidth += heartWidth + offsetX;
            heart.transform.localPosition = new Vector3(idx * (heartWidth+offsetX), 0, 1);
            hearts.Add(heart);
            UpdateHeart(idx, Mathf.Max(curr,0));
            idx++;
            curr--;
        }
        Destroy(orig);
        if (center)
        {
            transform.localPosition += new Vector3((-totalWidth / 2f * transform.localScale.x) + heartWidth / 4, 0);
        }
	}
	
	void Update ()
    {
        if (!health.dead)
        {
            if (health.hitTimer > 0)
            {
                for (int i = 0; i < hearts.Count; i++)
                {
                    UpdateHeart(i, (((health.health - 1) % heartCount) + 1)-i);
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
