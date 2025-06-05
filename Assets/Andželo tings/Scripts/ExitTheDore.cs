using UnityEngine;

public class ExitTheDore : MonoBehaviour
{
    public int sizeNeededToExit;
    public MovementScaler scaler;

    void Start()
    {
        scaler = GameObject.Find("First Person Controller").GetComponent<MovementScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
