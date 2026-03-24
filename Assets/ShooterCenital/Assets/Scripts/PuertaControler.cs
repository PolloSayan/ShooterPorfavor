using UnityEngine;

public class PuertaControler : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject puertaText;
    public bool hasKey;
    [SerializeField]
    private GameObject puertaCerrada;
    [SerializeField]
    private GameObject puertaAbierta;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (hasKey == false)
            { 
                puertaText.SetActive(true);
            }
            else if (hasKey == true)
            {
                puertaAbierta.SetActive(true);
                puertaCerrada.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            puertaText.SetActive(false);
        }
    }





}
