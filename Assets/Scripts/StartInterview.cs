using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class StartInterview : MonoBehaviour
{
    private ChatGPT chatGPT;
    
    private void Start()
    {
        chatGPT = transform.GetComponent<ChatGPT>();
    }
 
    public void BeginInterview()
    {
        chatGPT.StartInterview();
    }
}
