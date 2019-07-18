using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 光锥扫描效果
/// </summary>
public class ConeOfSight : MonoBehaviour
{

    public float SightAngle;
    public float MaxDistance;
    public const int BUFFER_SIZE = 256;

    private float[] m_aDepthBuffer;
    [SerializeField]
    private Material m_ConeOfSightMat;
    [SerializeField]
    private float rotSpeed;
    // Use this for initialization
    void Start()
    {

        m_aDepthBuffer = new float[BUFFER_SIZE];
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.fixedDeltaTime);
        UpdateViewDepthBuffer();
    }

    private void UpdateViewDepthBuffer()
    {
        float tempAngleStep = SightAngle / BUFFER_SIZE;
        float tempViewAngle = transform.eulerAngles.y;
        int tempBufferIndex = 0;

        for (int i = 0; i < BUFFER_SIZE; i++)
        {
            float tempAngle = tempAngleStep * i + (tempViewAngle - SightAngle / 2.0f);
            Vector3 tempDest = GetVector(-tempAngle * Mathf.Deg2Rad, MaxDistance);
            Ray tempRay = new Ray(transform.position, tempDest);
            RaycastHit tempHit = new RaycastHit();
            if (Physics.Raycast(tempRay, out tempHit))
            {
                m_aDepthBuffer[tempBufferIndex] = (tempHit.distance / MaxDistance);
                // Debug.DrawRay(transform.position, tempHit.point, Color.red);
            }
            else
            {
                m_aDepthBuffer[tempBufferIndex] = -1;
                //Debug.DrawRay(transform.position, tempDest);
            }
            tempBufferIndex++;

        }
        m_ConeOfSightMat.SetFloatArray("_SightDepthBuffer", m_aDepthBuffer);
    }
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.localPosition, transform.up, Vector3.right, 360, MaxDistance);

        float halfangle = SightAngle * Mathf.Deg2Rad / 2.0f;
        float viewangle = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        Vector3 p1 = GetVector(-halfangle - viewangle, MaxDistance);
        Vector3 p2 = GetVector(halfangle - viewangle, MaxDistance);

        // Handles.DrawLine(myObj.transform.position, p1);
        //  Handles.DrawLine(myObj.transform.position, p2);
        Debug.DrawRay(transform.position, p1);
        Debug.DrawRay(transform.position, p2);
    }
    public Vector3 GetVector(float angle, float dist)
    {
        float x = Mathf.Cos(angle) * dist;
        float z = Mathf.Sin(angle) * dist;
        return new Vector3(x, 0, z);

    }
}
