using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float RotationSpeed;
    public float FallingSpeed;
    public bool ShouldFall = false;

    private float Angle = 0f;
    private int RotationDirection = 1;
    public float Height = 6;
    public float fixedRotationX;
    public float fixedRotationY;
    public float fixedRotationZ;
    public bool SetRotationFromParent = false;
    
    // Start is called before the first frame update

    private void Start()
    {
        
        
    }
    public void RandomizeAngle()
    {
        Angle = UnityEngine.Random.Range(0, 360f);
    }

    public void RandomizeValues()
    {
        RotationSpeed += UnityEngine.Random.Range(0, 5);
        Angle = UnityEngine.Random.Range(0, 360f);
        RotationDirection = UnityEngine.Random.Range(0, 2);
        RotationDirection = RotationDirection == 0 ? -1 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(ShouldFall) Height -= FallingSpeed * Time.deltaTime;
        Angle += RotationSpeed * Time.deltaTime * RotationDirection;
        if (SetRotationFromParent)
        {
            Debug.LogError(transform.parent.rotation.y);
            Angle = -transform.parent.rotation.y;
        }
        transform.rotation =  Quaternion.Euler(new Vector3(transform.rotation.x + fixedRotationX, Angle + fixedRotationY, transform.rotation.z + fixedRotationZ));
        transform.position = new Vector3(transform.position.x, ShouldFall ? Height : transform.position.y, transform.position.z);
        
        if (ShouldFall && Height < 1 && Height > 6)
        {
            Destroy(this.gameObject.GetComponentInChildren<ObjectRotator>().gameObject);
            Destroy(this);
        }
    }
}
