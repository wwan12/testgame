using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    public Slider slider;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        slider.onValueChanged.AddListener(ValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ValueChange(float value) {
        text.text = value.ToString();
        if (gameObject.name.Equals("AmiNum"))
        {
            Messenger.Broadcast(EventCode.AUDIO_AMBIENT_VOLUME,value);
        }
        if (gameObject.name.Equals("MusicNum"))
        {
            Messenger.Broadcast(EventCode.AUDIO_EFFECT_VOLUME, value);
        }

    }

}
