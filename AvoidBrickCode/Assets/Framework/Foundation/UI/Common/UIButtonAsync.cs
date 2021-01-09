using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Komastar.UI.Common
{
    public class UIButtonAsync
    {
        public static bool IsRunning = true;

        public async static Task<T> SelectButton<T>(Button[] buttons) where T : Component
        {
            var tasks = buttons.Select(PressButton);
            Task<Button> finish = await Task.WhenAny(tasks);

            var result = finish.Result;
            if (ReferenceEquals(null, result))
            {
                return null;
            }
            else
            {
                return result.GetComponent<T>();
            }
        }

        public async static Task<Button> PressButton(Button button)
        {
            bool isPressed = false;
            button.onClick.AddListener(() => isPressed = true);
            while (!isPressed)
            {
                if (!IsRunning)
                {
                    Debug.LogWarning("UIButtonAsync null button");

                    return null;
                }
                await Task.Yield();
            }

            return button;
        }
    }
}
