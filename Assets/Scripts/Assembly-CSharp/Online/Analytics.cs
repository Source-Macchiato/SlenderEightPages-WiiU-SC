using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

namespace BrewConnect
{
    public class Analytics : MonoBehaviour
    {
        public static Analytics analytics;

        [SerializeField] private string projectToken = "31bd0a33956e67961b6cc12117530e188d95d89d35cd435339883c67c0e370bc";
        private string analyticsToken;

        MenuManager menuManager;

        void Awake()
        {
            if (analytics != null)
            {
                Destroy(this.gameObject);
                return;
            }
            analytics = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            menuManager = FindObjectOfType<MenuManager>();

            CanShareAnalytics();
        }

        private IEnumerator SendAnalytics()
        {
            if (string.IsNullOrEmpty(analyticsToken) && SaveManager.saveData.settings.shareAnalytics == 1 && !Application.isEditor)
            {
                string url = "https://api.brew-connect.com/v1/online/send_analytics";

                var data = new AnalyticsData
                {
                    project_token = projectToken,
                    category_name = "game",
                    analytics_entries = new List<AnalyticsEntry>
                    {
                        new AnalyticsEntry { name = "username", value = GetAccountName() },
                        new AnalyticsEntry { name = "version", value = GetVersion() },
                        new AnalyticsEntry { name = "language", value = GetLanguage() },
                        new AnalyticsEntry { name = "pages", value = GetPages() }
                    }
                };

                string json = JsonUtility.ToJson(data);
                byte[] post = Encoding.UTF8.GetBytes(json);

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("content-Type", "application/json");

                using (WWW www = new WWW(url, post, headers))
                {
                    yield return www;

                    string jsonResponse = www.text;
                    Response<AddDataResponse> response = JsonUtility.FromJson<Response<AddDataResponse>>(jsonResponse);

                    Debug.Log(response.message[0]);

                    if (StatusCode(www) == 201)
                    {
                        analyticsToken = response.data.token;
                    }
                }
            }
        }

        public IEnumerator UpdateAnalytics(string key, string value)
        {
            if (!string.IsNullOrEmpty(analyticsToken) && SaveManager.saveData.settings.shareAnalytics == 1 && !Application.isEditor)
            {
                string url = "https://api.brew-connect.com/v1/online/update_analytics";

                var data = new AnalyticsData
                {
                    analytics_token = analyticsToken,
                    project_token = projectToken,
                    category_name = "game",
                    analytics_entries = new List<AnalyticsEntry>
                    {
                        new AnalyticsEntry { name = key, value = value }
                    }
                };

                string json = JsonUtility.ToJson(data);
                byte[] post = Encoding.UTF8.GetBytes(json);

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("content-Type", "application/json");

                using (WWW www = new WWW(url, post, headers))
                {
                    yield return www;

                    string jsonResponse = www.text;
                    Response<object> response = JsonUtility.FromJson<Response<object>>(jsonResponse);

                    Debug.Log(response.message[0]);
                }
            }
        }

        private int StatusCode(WWW www)
        {
            string statusLine;
            if (www.responseHeaders.TryGetValue("STATUS", out statusLine))
            {
                string[] parts = statusLine.Split(' ');
                int statusCode;
                if (parts.Length > 1 && int.TryParse(parts[1], out statusCode))
                {
                    return statusCode;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private string GetAccountName()
        {
            string username = SaveManager.token;

            if (string.IsNullOrEmpty(username) || username == "<WiiU_AccountName>")
            {
                return "Unknown";
            }
            else
            {
                return username;
            }
        }

        private string GetNintendoName()
        {
            string username = WiiU.Core.accountName;

            if (string.IsNullOrEmpty(username) || username == "<WiiU_AccountName>")
            {
                return "Unknown";
            }
            else
            {
                return username;
            }
        }

        private string GetVersion()
        {
            TextAsset versionAsset = Resources.Load<TextAsset>("Meta/version");

            return versionAsset.text;
        }

        public string GetLanguage()
        {
            string language = SaveManager.saveData.settings.language;

            if (string.IsNullOrEmpty(language))
            {
                language = "en";
            }

            return language.ToUpper();
        }

        public string GetPages()
        {
            SharedVar shared = FindObjectOfType<SharedVar>();
            int pages = 0;

            if (shared != null)
            {
                pages = shared.pages;
            }

            return pages.ToString();
        }

        public void ShareAnalytics(bool share)
        {
            menuManager.CloseCurrentPopup();

            SaveManager.saveData.settings.shareAnalytics = share ? 1 : 0;
            SaveManager.Save();

            if (share)
            {
                StartCoroutine(SendAnalytics());
            }
        }

        public void CanShareAnalytics()
        {
            int canShareAnalytics = SaveManager.saveData.settings.shareAnalytics;

            if (canShareAnalytics == -1)
            {
                menuManager.AddPopup(1);
            }
            else if (canShareAnalytics == 1)
            {
                StartCoroutine(SendAnalytics());
            }
        }
    }

    // Send and update analytics
    [Serializable]
    public class AnalyticsEntry
    {
        public string name;
        public string value;
    }

    [Serializable]
    public class AnalyticsData
    {
        public string analytics_token;
        public string project_token;
        public string category_name;
        public List<AnalyticsEntry> analytics_entries;
    }

    [Serializable]
    public class Login
    {
        public string token;
    }

    // Responses
    [Serializable]
    public class Response<T>
    {
        public string[] message;
        public T data;
    }

    [Serializable]
    public class AddDataResponse
    {
        public string token;
    }

    [Serializable]
    public class LoginResponse
    {
        public string username;
        public string token;
    }
}