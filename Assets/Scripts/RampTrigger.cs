using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampTrigger : MonoBehaviour
{

    private Vector3 impulseForce = new Vector3(0,5,10);
    private bool aplicarFuerzaRelativaRampa = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Pelota")){
            Rigidbody pelotaRigidBody = other.GetComponent<Rigidbody>();
            if(pelotaRigidBody !=null){

                Vector3 fuerzaAplicar = impulseForce;
                
                if(aplicarFuerzaRelativaRampa){
                    fuerzaAplicar = transform.TransformDirection(impulseForce);
                }
            pelotaRigidBody.AddForce(fuerzaAplicar, ForceMode.Impulse);
            }
        }
    }
}