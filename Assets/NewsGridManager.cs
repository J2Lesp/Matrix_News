using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class NewsGridManager : MonoBehaviour
{
    float timer = 0;
    public List<string> words = new List<string>();
    Coroutine gatherWordCoroutine;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    List<string> badWords = new List<string>();
    private void Start()
    {
        gatherWordCoroutine = StartCoroutine(GatherWord());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 280.0f)
        {
            if (gatherWordCoroutine != null)
                StopCoroutine(gatherWordCoroutine);
            gatherWordCoroutine = StartCoroutine(GatherWord());
            timer = 0;
        }
    }

    IEnumerator GatherWord()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://newsapi.org/v2/top-headlines?country=us&apiKey=ea19e257ca36439ebf4096150c2bac63");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var text = www.downloadHandler.text;
            var data = JObject.Parse(text);
            foreach (var article in data["articles"])
            {
                string test = article["title"].ToString(Newtonsoft.Json.Formatting.None);

                test = test.Replace("\"", string.Empty).Replace("\'", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty);

                string finalResult = "";
                int countLetter = 0;

                foreach (string word in test.Split(' '))
                {
                    countLetter += word.Length;
                    if (countLetter > GetComponent<GridManager>().gridSizeX/2)
                        break;

                    if (word.Length > 0)
                    {
                        finalResult += " ";
                        finalResult += word;
                    }
                }

                AddWord(finalResult);



            }
                    
                /*
                foreach (string tmp in article["title"].ToString(Newtonsoft.Json.Formatting.None).Split(' '))
                    foreach (string tmp2 in tmp.Split('"'))
                        AddWord(tmp.Replace("\"", string.Empty).Replace(";", string.Empty));
                */
        }
    }

    void AddWord(string word)
    {
        if (!badWords.Contains(word))
            words.Add(word);
    }
}
