using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIconLocator : MonoBehaviour
{
    public GameObject m_ImageObj;
    private Canvas m_canvas;
    private GameObject m_trackingCore;
    public void Init(Canvas canvas, GameObject trackingCore)
    {
        m_canvas = canvas;
        m_trackingCore = trackingCore;
    }
    void Update()
    {
        //TODO Fix 
        if (m_canvas != null && m_trackingCore != null)
        {
            //Find out if obj is behind player or not, use different ruleset if so
            Vector3 camPos = m_canvas.worldCamera.transform.position;
            Vector3 worldDir = (m_trackingCore.transform.position - camPos).normalized;
            float worldAngle = Vector3.Angle(this.transform.forward, worldDir);
            Debug.Log(worldAngle);
            if (Mathf.Abs(worldAngle) < 90)
            {
                //Obj is directly behind camera, do not update the angle of the marker
            }
            else
            {
                Vector2 screenPos = m_canvas.worldCamera.WorldToScreenPoint(m_trackingCore.transform.position);
                Vector2 finalPos;
                RectTransform canvasRect = m_canvas.gameObject.GetComponent<RectTransform>();
                //Turns the screenspace coordinates into canvas space coordinates
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, m_canvas.worldCamera, out finalPos);
                Vector3 finalPos3 = new Vector3(finalPos.x, finalPos.y, 0);
                Vector3 dir = (finalPos3 - this.transform.localPosition).normalized;
                float reverse = Vector3.Dot(m_canvas.worldCamera.transform.forward, dir);
                float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                angle += 180;
                this.transform.localEulerAngles = new Vector3(0, 180, angle); // Note 180 in y slot is to account for unity default drop down arrow which points down by default
            }
            if (Mathf.Abs(worldAngle) > 160)
            {
                //Target is within view of the player, no need to render off-screen arrow
                m_ImageObj.SetActive(false);
            }
            else
            {
                m_ImageObj.SetActive(true);
            }
            
        }
    }
}
