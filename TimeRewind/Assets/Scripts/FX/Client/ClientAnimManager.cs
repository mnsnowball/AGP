using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientAnimManager : MonoBehaviour
{
    public Transform loveInterest;
    public Transform client;
    public void SetClientParent() 
    {
        client.SetParent(loveInterest);
    }
}
