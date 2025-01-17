using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRend : MonoBehaviour
{

    [SerializeField]
    private int pointCount;
    [SerializeField]
    private float radius;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount + 1;
        lineRenderer.widthMultiplier = 0.1f;
    }

    public void Draw(float circleRadius)
    {
        radius = circleRadius;
        lineRenderer.enabled = true;
        float angle_between_points = 360f / pointCount;

        for (int i = 0; i <= pointCount; i++)
        {
            float angle = i * angle_between_points * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0.15f, Mathf.Cos(angle));
            Vector3 position = direction * radius;

            lineRenderer.SetPosition(i, position);
        }
    }

    public void Hide()
    {
        lineRenderer.enabled = false;
    }

    public void ChangeColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
