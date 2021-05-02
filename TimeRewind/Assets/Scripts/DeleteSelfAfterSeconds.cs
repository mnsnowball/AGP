using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelfAfterSeconds : MonoBehaviour
{
    public float activeSeconds;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeleteMyself", activeSeconds);
    }

    void DeleteMyself() {
        Destroy(this.gameObject);
    }

}
