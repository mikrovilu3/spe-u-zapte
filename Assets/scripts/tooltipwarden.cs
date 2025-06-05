using TMPro;
using UnityEngine;

public class tooltipwarden : MonoBehaviour
{
    public int avalableBurgeres=0;
    public int avalablecoffy=0;
    public int avalableExit=1;
    private  TextMeshProUGUI Textu;
    private void Start()
    {
        Textu= GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Textu != null)
        {
            if (avalableBurgeres > 0 && avalablecoffy > 0)
            {
                Textu.text = "press E to eat and drink";
            }
            else if (avalableBurgeres > 0)
            {
                Textu.text = "press E to eat";
            }
            else if(avalablecoffy > 0)
            {
                Textu.text = "press E to drink";
            }
            else { Textu.text = ""; }
        }
        else { Debug.LogError("no text mesh in game object"); }
        if(avalableBurgeres > 1|| avalablecoffy > 1)
        {
            Debug.LogWarning("Multiple food items avalable at the same time");
        }
    }
}
