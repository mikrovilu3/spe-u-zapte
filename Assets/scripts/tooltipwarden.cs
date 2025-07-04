using TMPro;
using UnityEngine;

public class tooltipwarden : MonoBehaviour
{
    public int avalableBurgeres=0;
    public int avalablecoffy=0;
    public int avalableExit=0;
    public bool tutorial = false;
    private  TextMeshProUGUI Textu;
    public string goal;
    public bool ending = false;
    private void Start()
    {
        Textu= GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Textu != null)
        {
            if (tutorial)
            {
                if (Time.timeSinceLevelLoad < 3)
                {
                    Textu.text = "press tab to pause";
                }
                else if (avalableExit > 0)
                {
                    Textu.text = "press E to exit";
                }
                else if (avalableBurgeres > 0 && avalablecoffy > 0)
                {
                    Textu.text = "press E to eat and drink";
                }
                else if (avalableBurgeres > 0)
                {
                    Textu.text = "press E to eat";
                }
                else if (avalablecoffy > 0)
                {
                    Textu.text = "press E to drink";
                }
                else { Textu.text = ""; }
            }
            else if (!ending)
            {
                if (Time.timeSinceLevelLoad < 3)
                {

                }
                else if (avalableExit < 0)
                {
                    Textu.text = goal;
                }
                else
                {
                    Textu.text = "";
                }
            }
            else if (Time.timeSinceLevelLoad < 3)
            {
                Textu.text = "you can now escape, but you decide to enjoy jourself";
            }
            else
            {
                Textu.text = "";
            }
        }
        else { Debug.LogError("no text mesh in game object"); }
        
        
    }
}
