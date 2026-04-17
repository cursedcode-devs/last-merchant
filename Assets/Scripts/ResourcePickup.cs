// ResourcePickup.cs
// FULL DEBUG VERSION
// Press F while inside trigger range.

using UnityEngine;
using System.Collections;

public class ResourcePickup : MonoBehaviour
{
    private ResourceClusterSpawner spawner;

    private bool playerInRange = false;
    private bool collected = false;

    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Setup(ResourceClusterSpawner owner)
    {
        spawner = owner;
    }

    void Update()
    {
        if (playerInRange)
        {
            Debug.Log("Player in pickup range.");

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F pressed on pickup.");
                Collect();
            }
        }
    }

    void Collect()
    {
        if (collected) return;

        collected = true;
        col.enabled = false;

        Debug.Log("Collecting resource...");

        GiveRandomResource();

        StartCoroutine(FadeOut());
    }

    void GiveRandomResource()
    {
        int roll = Random.Range(0, 3);

        if (roll == 0)
        {
            PlayerManager.Instance.AddFuel(10);
            Debug.Log("Gained +10 Fuel");
        }
        else if (roll == 1)
        {
            PlayerManager.Instance.AddFood(10);
            Debug.Log("Gained +10 Food");
        }
        else
        {
            PlayerManager.Instance.AddParts(10);
            Debug.Log("Gained +10 Parts");
        }

        Debug.Log(
            "Totals => Fuel: " + PlayerManager.Instance.GetFuel() +
            " Food: " + PlayerManager.Instance.GetFood() +
            " Parts: " + PlayerManager.Instance.GetParts()
        );
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = sr.color;

        while (t < 0.4f)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / 0.4f);
            sr.color = c;
            yield return null;
        }

        spawner.Collected(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered pickup range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left pickup range.");
        }
    }
}