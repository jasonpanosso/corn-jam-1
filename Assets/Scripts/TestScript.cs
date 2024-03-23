using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        var newPosition = new Vector3(0, 0, 0);
        var newRotation = new Quaternion(0, 0, 0, 0);
        transform.position = newPosition;
        transform.rotation = newRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
    }
}
