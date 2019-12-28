using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Targeting : MonoBehaviour
{
    public Sprite m_noTarget;
    public Sprite m_Target;
    public MeshCollider m_ownCore;
    public LayerMask m_layermask;
    public GameObject copy;
    private float m_radius = 5f;
    private float m_maxDist = 10000f;
    private List<GameObject> testList = new List<GameObject>();
    private Image m_imageRef;
    private Vector3 m_convergePoint;
    private GameObject m_targetRef;
    private void Start()
    {
        m_imageRef = this.gameObject.GetComponent<Image>();
    }
    private void Update()
    {
        m_targetRef = null;
        m_convergePoint = transform.position + transform.forward * m_maxDist;  //Default converge point if no target is found
        Ray ray = new Ray(transform.position, transform.forward);
        List<RaycastHit> hitList = new List<RaycastHit>(Physics.SphereCastAll(ray, m_radius, m_maxDist, m_layermask.value));
        foreach (RaycastHit hit in hitList)
        {
            if (hit.collider != m_ownCore)
            {
                m_imageRef.sprite = m_Target;
                m_convergePoint = hit.point;
                m_targetRef = hit.collider.gameObject;
                //SphereCastVisualizer(hit);
            }
        }
        if (hitList.Count == 0)
        {
            m_imageRef.sprite = m_noTarget;
        }
        
    }
    public GameObject GetTargetRef()
    {
        ///Returns a reference to the current target
        return m_targetRef;
    }
    public Vector3 GetConvergePoint()
    {
        ///Returns a deep copy of the converge point, to prevent any issues with it changing in future
        return new Vector3(m_convergePoint.x, m_convergePoint.y, m_convergePoint.z);
    }
    private void SphereCastVisualizer(RaycastHit hit)
    {
        ///Debugging function used to visualize where the raycast is actually hitting an object
        GameObject obj = Instantiate(copy);
        obj.transform.localScale = new Vector3(m_radius, m_radius, m_radius);
        obj.transform.position = hit.point;
        testList.Add(obj);
        if (testList.Count >= 50)
        {
            foreach (GameObject tmp in testList)
            {
                Destroy(tmp);
            }
            testList.Clear();
        }
    }
}
