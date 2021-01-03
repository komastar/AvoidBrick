using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Foundation.UI.Common
{
    public class UIRootPanel : MonoBehaviour
    {
        public RectTransform[] leftRightPanel;
        public RectTransform[] topBotPanel;

        private void Awake()
        {
            float refRatio = 16f / 9f;
            float ratio = Screen.height / Screen.width;
            float diffRatio = refRatio - ratio;
            Vector2 sizeDelta = Vector2.zero;
            Rect camRect = Camera.main.rect;
            if (0f > diffRatio)
            {
                //  21:9
                float widthElem = Screen.width / 9f;
                float heightOverride = widthElem * 16f;
                sizeDelta.y = heightOverride - Screen.height;
                float heightRatio = Mathf.Abs(diffRatio) * (refRatio / ratio);
                camRect.height = 1f - heightRatio;
                camRect.y = (1f - camRect.height) * 0.5f;
                for (int i = 0; i < topBotPanel.Length; i++)
                {
                    topBotPanel[i].sizeDelta = sizeDelta * -0.5f;
                }
            }
            else if (0f < diffRatio)
            {
                //  16:10
                float heightElem = Screen.height / 16f;
                float widthOverride = heightElem * 9f;
                sizeDelta.x = widthOverride - Screen.width;
                for (int i = 0; i < topBotPanel.Length; i++)
                {
                    leftRightPanel[i].sizeDelta = sizeDelta * -0.5f;
                }
            }

            Camera.main.rect = camRect;
            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = sizeDelta;
        }
    }
}