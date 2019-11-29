using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimpointController : MonoBehaviour
{
    private float m_max_dist = 1000000;
    private float m_radius = 3;
    public LayerMask m_layer_mask;
    public GameObject m_crosshair_ref; //used for getting forward vector
    public Sprite m_no_target;
    public Sprite m_target;
    void Start()
    {
        InvokeRepeating("UpdateAimpoint", 0f, 0.2f);
    }
    public GameObject UpdateAimpoint()
    {
        //Returns a gameobject if the raycast encounters a player core or any targets, otherwise null
        //resets the z-value each time aimpoint needs updated
        GameObject return_val = null;
        transform.localPosition = new Vector3(0, 1.45f, 0);
        Ray cast = new Ray(transform.position, m_crosshair_ref.transform.forward);
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, m_radius, transform.forward, out hit, m_max_dist, m_layer_mask))
        {
            GameObject obj = hit.transform.gameObject;
            if (obj.tag == "Player" || obj.tag == "Target")
            {
                return_val = obj;
                m_crosshair_ref.GetComponent<Image>().sprite = m_target;
                transform.position = obj.transform.position;
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, m_max_dist);
                m_crosshair_ref.GetComponent<Image>().sprite = m_no_target;
            }
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, m_max_dist);
            m_crosshair_ref.GetComponent<Image>().sprite = m_no_target;
        }
        return return_val;
    }
}
