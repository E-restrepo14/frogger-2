using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator playerAnimator;
    [SerializeField]
    private bool islanded = true;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("groundTag"))
        {
            islanded = true;
            playerAnimator.SetBool("hasLanded", true);
        }

        if (other.CompareTag("moovingGroundTag"))
        {
            islanded = true;
            playerAnimator.SetBool("hasLanded", true);
            transform.parent.SetParent(other.transform);
        }

        if (other.CompareTag("obstacleTag"))
        {
            playerAnimator.SetBool("hasCrashed", true);

            Manager.instance.isdead = true;
            transform.parent.GetChild(1).GetComponent<Animator>().Play("take body", 0, 0);
        }

        if (other.CompareTag("Finish"))
        {
            islanded = true;
            playerAnimator.SetBool("hasLanded", true);
            Manager.instance.GanarNivel();
        }

        if (other.CompareTag("watterTag"))
        {
            if (islanded == false)
            {
                MorirAhogado();
            }
        }
    }

    public void MorirAhogado()
    {
        print("inserte linea para llamar el perdervida y actualizarhud() del manager");
        Manager.instance.VolverALinea1();
    }

    public void MorirAtropellado()
    {
        StartCoroutine(Morir("morir atropellado", 2f));
    }

   

    void Update()
    {
        if (islanded == true & Manager.instance.isdead == false & Manager.instance.haswin == false)
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                Saltar("Base Layer.salto adelante", Vector3.right);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Saltar("Base Layer.salto atras", Vector3.left);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Saltar("Base Layer.salto derecha", Vector3.back);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Saltar("Base Layer.salto izquierda", Vector3.forward);
            }
        }


    }

    public IEnumerator Morir(string m_animation, float secondsToRespawn)  //morir atropellado
    {
        playerAnimator.Play(m_animation, 0, 0);
        Manager.instance.isdead = true;
        transform.parent.GetChild(1).GetComponent<Animator>().Play("take body", 0, 0);
        yield return new WaitForSeconds(secondsToRespawn);
        Manager.instance.VolverALinea1();
    }

    public void Saltar( string m_animation, Vector3 direction )
    {
        transform.parent.SetParent(null);
        islanded = false;
        playerAnimator.SetBool("hasLanded", false);
        transform.parent.transform.position += direction;
        playerAnimator.Play(m_animation, 0, 0);
       
    }

}
