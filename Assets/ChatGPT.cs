using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChatGPT : MonoBehaviour
{
    // OpenAI API endpoint URL
    string endpoint = "https://api.openai.com/v1/engines/davinci-codex/completions";
    // OpenAI API key
    string apiKey = "011"; // apiKey 받아서 수정하기

    // Start is called before the first frame update
    void Start()
    {
        // Print prompt message to console
        Debug.Log("ChatGPT와 대화하기Test 콘솔에서 진행");

        // Start coroutine to read input from console
        StartCoroutine(ReadConsoleInput());
    }

    IEnumerator ReadConsoleInput()
    {
        while (true)
        {
            // Wait for user input in console
            yield return new WaitForEndOfFrame();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Get user input from console
                string userInput = Input.inputString.Trim();

                // If user input is not empty
                if (userInput.Length > 0)
                {
                    // Create HTTP request
                    StartCoroutine(CreateRequest(userInput));

                    // Print user input to console
                    Debug.Log("You: " + userInput);
                }
            }
        }
    }

    IEnumerator CreateRequest(string userInput)
    {
        // Request body as JSON string
        string requestBody = "{\"prompt\": \"" + userInput + "\", \"max_tokens\": 10}";

        // Create UnityWebRequest object
        UnityWebRequest www = UnityWebRequest.Post(endpoint, requestBody);

        // Set authorization header with API key
        www.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Send HTTP request
        yield return www.SendWebRequest();

        // Check for errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("HTTP error: " + www.error);
        }
        else
        {
            // Parse JSON response
            string responseJson = www.downloadHandler.text;
            ChatGPTResponse response = JsonUtility.FromJson<ChatGPTResponse>(responseJson);

            // Log generated sentence
            Debug.Log("ChatGPT: " + response.choices[0].text);
        }
    }

    // Response class for JSON deserialization
    [System.Serializable]
    public class ChatGPTResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public string text;
        public float logprobs;
        public float finish_reason;
    }
}
