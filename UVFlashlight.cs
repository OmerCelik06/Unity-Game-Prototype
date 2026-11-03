using UnityEngine;

public class UVFlashlight : MonoBehaviour
{
    [Header("Fener Ayarlarý")]
    public float range = 15f;
    public LayerMask hiddenLayer;

    private GameObject currentHiddenObject;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, hiddenLayer))
        {
            if (currentHiddenObject != hit.collider.gameObject)
            {
                if (currentHiddenObject != null)
                {
                    currentHiddenObject.GetComponent<Renderer>().enabled = false;
                }

                currentHiddenObject = hit.collider.gameObject;
                currentHiddenObject.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            if (currentHiddenObject != null)
            {
                currentHiddenObject.GetComponent<Renderer>().enabled = false;
                currentHiddenObject = null; 
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}