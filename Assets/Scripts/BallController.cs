using JetBrains.Annotations;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody rigid;
    void Start()
    {
         rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            rigid.AddForce(Vector3.right * speed);
        }

        else if (Input.GetAxis("Horizontal") < 0)
        {
            rigid.AddForce(-Vector3.right * speed);
        }


        if (Input.GetAxis("Vertical") > 0)
        {
            rigid.AddForce(Vector3.forward * speed);
        }

        else if (Input.GetAxis("Vertical") < 0)
        {
            rigid.AddForce(-Vector3.forward * speed);
        }
    }
}
