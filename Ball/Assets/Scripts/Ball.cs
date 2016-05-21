using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

    public float maxStretch = 3.0f;
    public LineRenderer leftLine;
    public LineRenderer rightLine;

    private SpringJoint2D spring;
    private Transform center;
    private Ray leftRay;
    private Ray rightRay;
    private Ray centerRay;
    private float maxStretchSqr;
    private float circleRadius;
    private bool clickedOn;
    private Vector2 prevVelocity;

    void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        center = spring.connectedBody.transform;
    }

    // Use this for initialization
    void Start()
    {
        LineRendererSetup();
        leftRay = new Ray(leftLine.transform.position, this.transform.position);
        rightRay = new Ray(rightLine.transform.position, this.transform.position);
        centerRay = new Ray(center.position, this.transform.position);
        maxStretchSqr = maxStretch * maxStretch;
        CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
        circleRadius = circle.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (clickedOn)
            Dragging();

        if (spring != null)
        {
            if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude)
            {
                Destroy(spring);
                GetComponent<Rigidbody2D>().velocity = prevVelocity;
            }

            if (!clickedOn)
            {
                prevVelocity = GetComponent<Rigidbody2D>().velocity;
            }

            LineRendererUpdate();

        }
        else
        {
            leftLine.enabled = false;
            rightLine.enabled = false;
        }
    }

    void LineRendererSetup()
    {
        leftLine.SetPosition(0, leftLine.transform.position);
        rightLine.SetPosition(0, rightLine.transform.position);

        leftLine.sortingLayerName = "Foreground";
        rightLine.sortingLayerName = "Foreground";

        leftLine.sortingOrder = 1;
        rightLine.sortingOrder = 1;
    }

    void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
    }

    void OnMouseUp()
    {
        spring.enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        clickedOn = false;
    }

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - center.position;

        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            centerRay.direction = catapultToMouse;
            mouseWorldPoint = centerRay.GetPoint(maxStretch);
        }

        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }

    void LineRendererUpdate()
    {
        Vector2 leftToProjectile = transform.position - leftLine.transform.position;
        Vector2 rightToProjectile = transform.position - rightLine.transform.position;
        leftRay.direction = leftToProjectile;
        rightRay.direction = rightToProjectile;
        //Vector3 leftPoint = leftRay.GetPoint(leftToProjectile.magnitude + circleRadius);
        //Vector3 rightPoint = rightRay.GetPoint(rightToProjectile.magnitude + circleRadius);

        Vector3 leftPoint = leftRay.GetPoint(leftToProjectile.magnitude);
        Vector3 rightPoint = rightRay.GetPoint(rightToProjectile.magnitude);

        leftLine.SetPosition(1, leftPoint);
        rightLine.SetPosition(1, rightPoint);
    }
}
