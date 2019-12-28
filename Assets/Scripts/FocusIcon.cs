using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusIcon : MonoBehaviour
{
    private Canvas m_canvas;
    private GameObject m_trackingCore;
    public LayerMask m_layerMask;
    public GameObject m_ImageObj;
    public void Init(Canvas canvas, GameObject trackingCore)
    {
        m_canvas = canvas;
        m_trackingCore = trackingCore;
    }
    public void Update()
    {
        if(m_canvas != null && m_trackingCore != null)
        {
            Vector2 screenPos =  m_canvas.worldCamera.WorldToScreenPoint(m_trackingCore.transform.position);
            Vector2 finalPos;
            //Turns the screenspace coordinates into canvas space coordinates
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.gameObject.GetComponent<RectTransform>(), screenPos, m_canvas.worldCamera, out finalPos);
            this.transform.localPosition = finalPos;
            
        }
        //Check if gameobject is behind player
        Vector3 camPos = m_canvas.worldCamera.transform.position;
        Vector3 dir = (m_trackingCore.transform.position - camPos).normalized;
        float angle = Vector3.Angle(this.transform.forward, dir);

        if (Mathf.Abs(angle) > 90)
        {
            //Focus is behind camera, do not render image
            m_ImageObj.SetActive(false);
        }
        else
        {
            m_ImageObj.SetActive(true);
        }
    }
}
