using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerColorController : NetworkBehaviour
{
    private NetworkVariable<UnityEngine.Color> Colour = new NetworkVariable<UnityEngine.Color>(writePerm:NetworkVariableWritePermission.Owner, readPerm:NetworkVariableReadPermission.Everyone);
    SpriteRenderer m_SpriteRenderer;
    Color m_NewColor;

    float m_Red = 1, m_Blue = 1, m_Green = 1;

    void Start()
    {
        m_SpriteRenderer = Helper.FindChildWithTag(gameObject, "Entity").GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.white;
    }
    void Update()
    {
        m_SpriteRenderer.color = Colour.Value;
    }

    void OnGUI()
    {
        if (!IsOwner) return;
        //Use the Sliders to manipulate the RGB component of Color
        //Use the Label to identify the Slider
        GUI.Label(new Rect(Screen.width - 200, 30, 50, 30), "Red: ");
        //Use the Slider to change amount of red in the Color
        m_Red = GUI.HorizontalSlider(new Rect(Screen.width - 210, 25, 200, 30), m_Red, 0.7f, 1);

        //The Slider manipulates the amount of green in the GameObject
        GUI.Label(new Rect(Screen.width - 200, 70, 50, 30), "Green: ");
        m_Green = GUI.HorizontalSlider(new Rect(Screen.width - 210, 60, 200, 30), m_Green, 0.7f, 1);

        //This Slider decides the amount of blue in the GameObject
        GUI.Label(new Rect(Screen.width - 200, 105, 50, 30), "Blue: ");
        m_Blue = GUI.HorizontalSlider(new Rect(Screen.width - 210, 95, 200, 30), m_Blue, 0.7f, 1);

        //Set the Color to the values gained from the Sliders
        m_NewColor = new Color(m_Red, m_Green, m_Blue);

        //Set the SpriteRenderer to the Color defined by the Sliders
        m_SpriteRenderer.color = m_NewColor;
        Colour.Value = m_NewColor;
    }
}
