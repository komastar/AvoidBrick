using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Assets.Framework.Foundation.Network
{
    public class RestApiController : MonoBehaviour
    {
        public IEnumerator GetCoroutine(string url, Action<string> onResponse)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var send = request.SendWebRequest();
                while (!send.isDone)
                {
                    yield return null;
                }

                if (null == send.webRequest.downloadHandler
                    || string.IsNullOrEmpty(send.webRequest.downloadHandler.text))
                {
                    Log.Error($"GetRequest FAIL. {url}");
                    onResponse?.Invoke(null);
                }
                else
                {
                    onResponse?.Invoke(send.webRequest.downloadHandler.text);
                }
            }
        }

        public async Task<string> GetAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var send = request.SendWebRequest();
                while (!send.isDone)
                {
                    await Task.Yield();
                }

                if (null == send.webRequest.downloadHandler
                    || string.IsNullOrEmpty(send.webRequest.downloadHandler.text))
                {
                    Log.Error($"GetRequest FAIL. {url}");

                    return null;
                }
                else
                {
                    return send.webRequest.downloadHandler.text;
                }
            }
        }
    }
}
