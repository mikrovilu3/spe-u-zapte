using UnityEngine;

public class ExitTheDore : MonoBehaviour
{
    public int sizeNeededToExit;
    public MovementScaler scaler;
    bool isNearDore = false;
    private float scale1;
    private tooltipwarden Textuw;
    public string scene;

    void Start()
    {
        scaler = GameObject.Find("First Person Controller").GetComponent<MovementScaler>();
        Textuw = GameObject.Find("uii/tooltip").GetComponent<tooltipwarden>();
    }

    // Update is called once per frame
    void Update()
    {
        scale1 = scaler.scale;
        if (isNearDore && sizeNeededToExit == scale1 && )
        {
            SceneManager.LoadScene(scene);
        }
    }

    void OnCollisionEnter(Collider other)
    {
        if (other.GameObject = scaler.GameObject)
        {
            isNearDore = true;
            Textuw.avalableExit ++
        }
    }

    void OnCollisionExit(Collider other)
    {
        if (other.GameObject = scaler.GameObject)
        {
            isNearDore = false;
            Textuw.avalableExit --
        }
    }
}
