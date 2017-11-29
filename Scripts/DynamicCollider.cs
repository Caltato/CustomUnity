using HoloToolkit.Unity;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DynamicCollider : MonoBehaviour
{
    [HideInInspector] public float height { get { return Vector3.Distance(head.transform.position, transform.position); } }

    [Tooltip("Maximum Height in Metres")] [SerializeField] [Range(0.90f, 2.00f)] private float maxHeight = 1.60f;
    [SerializeField] private LayerMask floorLayer;

    private bool belowHead { get { return Physics.Raycast(head.transform.position, Vector3.down, maxHeight, floorLayer); } }
    private GameObject head;
    private Collider col;
    
    private void Start()
    {
        head = Camera.main.gameObject;
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        UpdatePos();
        UpdateCollider();
    }

    private void UpdatePos()
    {
        transform.eulerAngles = RotateY;

        if (belowHead)
        {
            transform.position = new Vector3(
                head.transform.position.x,
                0,//SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceAlignment().FloorYValue,
                head.transform.position.z);
        }

        else
        {
            // Move Vertical
            transform.position = new Vector3(head.transform.position.x, head.transform.position.y - maxHeight, head.transform.position.z);
        }
    }

    private void UpdateCollider()
    {
        switch (col.GetType().ToString())
        {
            case "UnityEngine.CapsuleCollider":
                CapsuleCollider c_col = (CapsuleCollider)col;
                
                // Collider Height
                c_col.height = height;
                if (c_col.height > maxHeight)
                {
                    c_col.height = maxHeight;
                }

                // Collider Center
                c_col.center = new Vector3(c_col.center.x, c_col.height / 2, c_col.center.z);
                break;

            case "UnityEngine.BoxCollider":
                BoxCollider b_col = (BoxCollider)col;

                // Collider Height 
                b_col.size = new Vector3(b_col.size.x, height, b_col.size.z);
                if (b_col.size.y > maxHeight)
                {
                    b_col.size = new Vector3(b_col.size.x, maxHeight, b_col.size.z); ;
                }

                // Collider Center
                b_col.center = new Vector3(b_col.center.x, b_col.size.y / 2, b_col.center.z);
                break;

            case "UnityEngine.SphereCollider":
                SphereCollider s_col = (SphereCollider)col;

                // Collider Height
                s_col.radius = height / 2;
                if (s_col.radius * 2 > maxHeight)
                {
                    s_col.radius = maxHeight / 2;
                }

                // Collider Center
                s_col.center = new Vector3(s_col.center.x, s_col.radius, s_col.center.z);
                break;

            default:
                Debug.LogError("The attached Collider \"" + col.GetType().ToString() + "\" on " + gameObject.name + " is not supported by the DynamicCollider Script");
                break;
        }
    }
       
    private Vector3 RotateY
    {
        get { return new Vector3(belowHead ? 0 : transform.eulerAngles.x,
              head.transform.eulerAngles.y < 0 ? head.transform.eulerAngles.y + 360 : head.transform.eulerAngles.y,
              belowHead ? 0 : transform.eulerAngles.z); }
    }
}
