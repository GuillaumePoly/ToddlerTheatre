using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReceiver : MonoBehaviour
{
    public static bool m_ClickedPrevious;
    public static bool m_Clicked;
    public static Vector2 m_PointerPos;
    public static Vector2 m_WorldPointerPos;

    private Camera cam;

    private void OnEnable()
    {
        cam = Camera.main;
    }

    public void OnPointer(InputAction.CallbackContext ctx)
    {
        m_ClickedPrevious = m_Clicked;
        m_Clicked = ctx.ReadValue<float>() > 0;
    }

    public void OnPointerPos(InputAction.CallbackContext ctx)
    {
        m_PointerPos = ctx.ReadValue<Vector2>();
        m_WorldPointerPos = cam.ScreenToWorldPoint(m_PointerPos);
    }
}
