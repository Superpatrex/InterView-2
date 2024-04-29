using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using NaughtyAttributes;

// This class is used to interact with the MongoDB database
public class MongoDBAPI: MonoBehaviour
{
    // Public Serialized Fields
    [SerializeField] public string code;
    [SerializeField] public string resumeData = null; // Replace with your UI text object
    [SerializeField] public List<TMP_Text> inputFieldFromKeyboard; // Replace with your UI input field object 

    private TMP_Text inputFieldFromKeyboardNow;

    [SerializeField] public List<GameObject> codeConsoles;
    [SerializeField] public List<GameObject> codeConsolesSend;
    [SerializeField] public List<GameObject> codeConsolesExit;

    private bool? resumeGetSuccess = null;


    // Public Fields
    // This is the LeetCodeQuestion object that will be used to store the data from the MongoDB database
    public static LeetCodeQuestion question;

    public static MongoDBAPI Instance = null;
    public static bool hasResume = false;
    public static bool hasQuestion = false;

    // Private Fields
    private static readonly string apiUrl = "https://interver2servver-c50bd6ac3bdd.herokuapp.com"; // Replace with your API endpoint
    private static readonly string difficultyLevel = "Easy";

    public void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (resumeGetSuccess != null)
        {
            resumeGetSuccess = null;
            inputFieldFromKeyboardNow = null;
        }
    }

    // This can be changed to have the code be from a different input field or source
    [Button("Get Resume String")]
    public void ButtonHandlerGetResumeString()
    {
        //Debug.Log(inputFieldFromKeyboard.text);
        //string code = inputFieldFromKeyboard.text;
        StartCoroutine(SendDataToMongoDBGetPDFString(code));
    }

    [Button("Show Code Resume Console")]
    public void ShowCodeResumeConsole()
    {
        foreach (GameObject codeConsole in codeConsoles)
        {
            codeConsole.SetActive(true);
        }

        foreach (GameObject codeConsole in codeConsolesSend)
        {
            codeConsole.SetActive(true);
        }

        foreach (GameObject codeConsole in codeConsolesExit)
        {
            codeConsole.SetActive(true);
        }
    }

    [Button("Send Code")]
    public void SendCode()
    {
        bool flag = false;

        foreach (TMP_Text inputs in inputFieldFromKeyboard)
        {
            if (inputs.text != null && inputs.text.Length > 0)
            {
                inputFieldFromKeyboardNow = inputs;
                flag = true;
                break;
            }
        }
        
        if (!flag)
        {
            resumeGetSuccess = false;
            Debug.Log("No input field found!");
            return;
        }

        this.code = inputFieldFromKeyboardNow.text;
        StartCoroutine(SendDataToMongoDBGetPDFString(code));
    }

    [Button("Hide Code Resume Console")]
    public void HideCodeResumeConsole()
    {
        foreach (GameObject codeConsole in codeConsoles)
        {
            codeConsole.SetActive(false);
        }

        foreach (GameObject codeConsole in codeConsolesSend)
        {
            codeConsole.SetActive(false);
        }

        foreach (GameObject codeConsole in codeConsolesExit)
        {
            codeConsole.SetActive(false);
        }
    }

    // This can be changed to have the difficulty be from a different input field or source
    // This can also be changed to have the difficulty be a parameter
    [Button("Get LeetCode Question")]
    public void ButtonHandlerGetLeetCodeQuestion()
    {
        Debug.Log("Pulling a question from MongoDB with a difficulty of " + difficultyLevel);
        StartCoroutine(SendDataToMongoDBGetLeetCodeQuestion(difficultyLevel));
    }

    /// <summary>
    /// This method sends a request to the MongoDB database to retrieve a resume string
    /// </summary>
    /// <param name="code">6 alphanumeric character string that is linked to the MongoDB collection</param>
    /// <returns>Nothing, will update a field that is a string within the script</returns>
    IEnumerator SendDataToMongoDBGetPDFString(string code)
    {
        string endpoint = "/api/getPDF"; // Replace with your specific API endpoint

        // Create a JSON object or format your data as needed
        string jsonBody = "{\"code\": \"" + code + "\"}";

        Debug.Log(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(MongoDBAPI.apiUrl + endpoint, "POST"))
        {
            // Set the request as a POST request
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Data sent successfully for the resume retrieval!");
                // Handle response if needed
                MongoDBAPI.Instance.resumeData = request.downloadHandler.text;
            }
        }

        if (MongoDBAPI.Instance.resumeData != null && !string.IsNullOrEmpty(MongoDBAPI.Instance.resumeData))
        {
            hasResume = true;
            resumeGetSuccess = true;
            Debug.Log("Set tp true");
        }
        else
        {
            hasResume = false;
            resumeGetSuccess = false;
            Debug.Log("Set tp false");
        }
    }

    /// <summary>
    /// This method sends a request to the MongoDB database to retrieve a LeetCode question
    /// </summary>
    /// <param name="difficulty">Specifies which question difficulty is being queried ("Easy", "Medium", and "Hard")</param>
    /// <returns>None, will update a field that is a string within the script</returns>
    IEnumerator SendDataToMongoDBGetLeetCodeQuestion(string difficulty)
    {
        string endpoint = "/api/getLeetCodeQuestion"; // Replace with your specific API endpoint

        // Create a JSON object or format your data as needed
        string jsonBody = "{\"difficulty\": \"" + difficulty + "\"}";

        Debug.Log(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(MongoDBAPI.apiUrl + endpoint, "POST"))
        {
            // Set the request as a POST request
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Data sent successfully for the LeetCode Question Retrieval!");

                // From the json data, create a LeetCodeQuestion object
                RootObject temp = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(request.downloadHandler.text);
                MongoDBAPI.question = temp.question;
                //Debug.Log(request.downloadHandler.text);
                Debug.Log(MongoDBAPI.question.ToString());
            }

            if (MongoDBAPI.question != null)
            {
                hasQuestion = true;
            }
            else
            {
                hasQuestion = false;
            }
        }
    }
}
