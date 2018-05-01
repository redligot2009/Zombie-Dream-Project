using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour {

    Slider slider;

	void Start () {
        slider = GetComponent<Slider>();
        slider.value = AudioListener.volume;
	}
	
	void Update () {
        AudioListener.volume = slider.value;
	}
}
