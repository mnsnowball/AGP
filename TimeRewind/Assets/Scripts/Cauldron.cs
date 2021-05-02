using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public GameObject playText;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.canPlay = true;
            playText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.canPlay = false;
            playText.SetActive(false);
        }
    }
}
