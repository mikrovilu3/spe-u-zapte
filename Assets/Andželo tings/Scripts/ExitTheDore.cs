using UnityEngine;

public class ExitTheDore : MonoBehaviour
{
    public int sizeNeededToExit;
    public MovementScaler scaler;
    bool isNearDore = false;
    private float scale1;

    void Start()
    {
        scaler = GameObject.Find("First Person Controller").GetComponent<MovementScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        scale1 = scaler.scale;
        if (isNearDore && sizeNeededToExit == scale1) { }
    }

    void OnCollisionEnter()
    {
        isNearDore = true;
    }

    void OnCollisionExit() 
    {
        isNearDore = false;
    }
}
