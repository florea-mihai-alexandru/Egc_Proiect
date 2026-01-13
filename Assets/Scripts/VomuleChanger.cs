using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VomuleChanger : MonoBehaviour
{
    public void VolumeChanger(float value)
    {
        AudioListener.volume = value;
    }
}
