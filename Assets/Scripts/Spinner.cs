using Unity.VisualScripting;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public  float spinSpeed = 3000.0f;
    [SerializeField] bool doSpin = false;
    Rigidbody rb;
    GameObject playerGraphics;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerGraphics = GameObject.Find("PlayerGraphics");

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
     
        if(doSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0,spinSpeed*Time.deltaTime,0));
        }
    }
}
