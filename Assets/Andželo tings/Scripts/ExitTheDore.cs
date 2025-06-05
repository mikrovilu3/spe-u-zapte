using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTheDore : MonoBehaviour
{
    public int sizeNeededToExit;
    public MovementScaler scaler;
    bool isNearDore = false;
    private float scale1;
    private tooltipwarden Textuw;
    public string scene = "Pēteris_gulamistaba";

    void Start()
    {
        scaler = GameObject.Find("First Person Controller").GetComponent<MovementScaler>();
        Textuw = GameObject.Find("uii/tooltip").GetComponent<tooltipwarden>();
    }

    // Update is called once per frame
    void Update()
    {
        scale1 = scaler.scale;
        if (isNearDore && scale1 <= sizeNeededToExit && Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene("Pēteris_gulamistaba");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == scaler)
        {
            isNearDore = true;
            Textuw.avalableExit++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == scaler)
        {
            isNearDore = false;
            Textuw.avalableExit--;
        }
    }
}
