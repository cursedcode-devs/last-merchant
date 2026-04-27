using UnityEngine;
using System.Collections;
using TMPro;

public class ResourcePickup : MonoBehaviour
{
    public TextMeshProUGUI pickupPrompt;

    private ResourceClusterSpawner spawner;

    private bool playerInRange = false;
    private bool collected = false;

    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (pickupPrompt != null)
            pickupPrompt.gameObject.SetActive(false);
    }

    public int resourceType = 0; // 0 = Fuel, 1 = Food, 2 = Parts

    public void Setup(ResourceClusterSpawner owner)
    {
        spawner = owner;
    }

    // Update method removed because we now collect automatically.

    void Collect()
    {
        if (collected) return;

        collected = true;
        col.enabled = false;

        if (pickupPrompt != null)
            pickupPrompt.gameObject.SetActive(false);

        Debug.Log("Collecting resource...");

        GiveRandomResource();

        StartCoroutine(FadeOut());
    }

    void GiveRandomResource()
    {
        int roll = resourceType;

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
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected pickup automatically.");
            Collect();
        }
    }
}