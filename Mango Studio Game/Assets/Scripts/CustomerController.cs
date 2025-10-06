using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = Vector3.forward;
    bool atCounter = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!atCounter)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        atCounter = true;
    }
}
