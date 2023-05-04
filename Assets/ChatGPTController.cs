using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ChatGPTController : MonoBehaviour
{
    public Text chatLogText;
    private string apiKey = "YOUR_API_KEY"; // API KEY ¿€º∫
    private string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";
    private int maxTokens = 128;

    public void GetChatGPTResponse(string prompt)
    {
        StartCoroutine(RequestChatGPT(prompt));
    }

    private IEnumerator RequestChatGPT(string prompt)
    {
        WWWForm form = new WWWForm();
        form.AddField("prompt", prompt);
        form.AddField("max_tokens", maxTokens);

        UnityWebRequest www = UnityWebRequest.Post(apiUrl, form);
        www.SetRequestHeader("Authorization", "Bearer " + apiKey);
        www.SetRequestHeader("Content-Type", "application/json");
        www.uploadHandler.contentType = "application/json";

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseJson = www.downloadHandler.text;
            ChatGPTResponse response = JsonUtility.FromJson<ChatGPTResponse>(responseJson);
            string answer = response.choices[0].text;

            AddChatLog("ChatGPT: " + answer);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }

    private void AddChatLog(string text)
    {
        chatLogText.text += "\n" + text;
    }

    [System.Serializable]
    private class ChatGPTResponse
    {
        public ChatGPTChoice[] choices;
    }

    [System.Serializable]
    private class ChatGPTChoice
    {
        public string text;
    }
}