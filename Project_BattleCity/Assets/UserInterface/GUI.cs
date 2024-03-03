using UnityEngine;
using UnityEngine.UI;
using Core.Pattern;

public class GUIController : Singleton<GUIController> {

    private GameObject capBar;
    private Slider capBarSlider;
    //private TMP_Text capBarTimerText;

    //private float globalCapedTime;
    //private bool caped;
    public float maxCapTime;

    // Start is called before the first frame update
    void Start()  {
        capBar = this.gameObject.transform.Find("CapBar").gameObject;
        capBarSlider = capBar.GetComponent<Slider>();
        //capBarTimerText = capBar.transform.Find("Timer").GetComponent<TextMeshProUGUI>();

        //globalCapedTime = 0.0f;
        //caped = false;
        maxCapTime = 30.0f;
        capBarSlider.maxValue = maxCapTime;
        capBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
