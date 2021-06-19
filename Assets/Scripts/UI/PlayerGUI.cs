using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    //Handles the GUI aspects of a player: health, throttle, and minimap
    public GameObject minimap_ref;
    public GameObject target_template;
    private List<GameObject> target_list;
    private List<GameObject> icon_list;
    void Start()
    {
        icon_list = new List<GameObject>();
    }

    void FindTargets()
    {
        target_list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
    }

    void ClearMap()
    {
        List <GameObject> destroy_list = new List<GameObject>();
        foreach (GameObject obj in icon_list)
        {
            destroy_list.Add(obj);
        }
        
        foreach (GameObject obj in destroy_list)
        {
            Destroy(obj);
        }
    }

    void RenderTargets()
    {
        foreach (GameObject obj in target_list)
        {
            if (obj == this.gameObject)
            {
                // TODO
            }
            else
            {
                Vector3 delta = obj.transform.position - transform.position;
                Vector3 dir = delta.normalized;
                GameObject icon_clone = Instantiate(target_template, minimap_ref.transform);
                icon_clone.SetActive(true);
                //Debug.Log("Object Forward: " + forward_angle + "Object Right: " + right_angle + "Object Up: " + up_angle);
                icon_list.Add(icon_clone);

                // Angle meanings:
                // forward/right/up: 0 to 90 mean offset ahead, over 90 means behind
                float forward_angle = ConvertToDegrees(FindAngle(transform.forward, dir));
                float right_angle = ConvertToDegrees(FindAngle(transform.right, dir));
                float up_angle = ConvertToDegrees(FindAngle(transform.up, dir));

                Vector3 t_vec = Vector3.zero;
                
                if (float.IsNaN(forward_angle))
                    forward_angle = 0;
                if (float.IsNaN(right_angle))
                    right_angle = 0;
                if (float.IsNaN(up_angle))
                    up_angle = 0;

                Debug.Log("1: " + forward_angle + " 2: " + right_angle + " 3: " + up_angle);

                t_vec += forward_angle * transform.forward;
                t_vec += right_angle * transform.right;
                t_vec += up_angle * transform.up;
                icon_clone.transform.position += t_vec / 2f;

            }
        }
    }
    float ConvertToDegrees(float radians)
    {
        return radians * 180 / Mathf.PI;
    }
    float FindAngle(Vector3 a, Vector3 b)
    {
        //Note: Returns angle in radians
        float angle = 0;
        if (Vector3.Dot(a, b) == 0)
        {
            return 90 * Mathf.PI / 180;
        }
        else
        {
            float a_val = a.magnitude;
            float b_val = b.magnitude;
            if (a_val * b_val != 0)
            {
                angle = Mathf.Acos((Vector3.Dot(a, b) / (a_val * b_val)));
                return angle;
            }
            return angle;
            
        }
    }
    void UpdateTracker()
    {
        //Update tracking list and represent positions on gui.
        ClearMap();
        FindTargets();
        RenderTargets();

    }

    void Update()
    {
        UpdateTracker();
    }
}
