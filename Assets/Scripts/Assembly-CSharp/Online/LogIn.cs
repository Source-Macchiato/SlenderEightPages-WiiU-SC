using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrewConnect
{
    public class LogIn : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GetUsernameRequest());
        }

        public IEnumerator GetUsernameRequest()
        {
            yield return new WaitForSeconds(1f);

            if (!string.IsNullOrEmpty(SaveManager.token) && !Application.isEditor)
            {
                string url = "https://api.brew-connect.com/v1/account/login";

                var data = new Login
                {
                    token = SaveManager.token
                };

                string json = JsonUtility.ToJson(data);
                byte[] post = Encoding.UTF8.GetBytes(json);

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");

                using (WWW www = new WWW(url, post, headers))
                {
                    yield return www;

                    string jsonResponse = www.text;
                    Response<LoginResponse> response = JsonUtility.FromJson<Response<LoginResponse>>(jsonResponse);

                    SaveManager.username = response.data.username;
                }
            }
        }
    }

    [Serializable]
    public class LoginResponse
    {
        public string username;
        public string token;
    }

    [Serializable]
    public class Login
    {
        public string token;
    }
}