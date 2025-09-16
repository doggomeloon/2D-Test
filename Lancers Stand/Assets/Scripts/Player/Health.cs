using UnityEngine;

public class Health : MonoBehaviour
{

    public GameObject HealthBackground;

    public GameObject HeartTemplate;

    private int initHealth; // The current number of initialized health objects in HealthBackground

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        initHealth = HealthBackground.transform.childCount;
        if (initHealth < GlobalVariables.health) //When hearts are added
        {
            GameObject heart = Instantiate(HeartTemplate); // Duplicate template
            heart.transform.SetParent(HealthBackground.transform); // Move it to the correct location
            heart.name = "heart"; // Renaming it so it looks better
        }
        else if (initHealth > GlobalVariables.health && initHealth > 0) //When hearts are removed
        {
            Transform firstChild = HealthBackground.transform.GetChild(0); //Gets the first child
            Destroy(firstChild.gameObject); // Destroys it (wow thats brutal)
        }

        if (Input.GetKeyDown(KeyCode.L)) // Debug add or remove health
        {
            GlobalVariables.health--;
        } else if (Input.GetKeyDown(KeyCode.P)) {
            GlobalVariables.health++;
        }
    }
}
