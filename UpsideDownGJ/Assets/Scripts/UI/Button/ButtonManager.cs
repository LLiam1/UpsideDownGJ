using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseHovering = false;
    private Animator anim;

    private void Start()
    {
        isMouseHovering = false;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (isMouseHovering)
        {
            anim.StopPlayback();
        } else
        {
            anim.Play(0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseHovering = false;
    }
}
