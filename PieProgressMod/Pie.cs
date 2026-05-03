using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace PieProgressMod
{
    public class Pie : MonoBehaviour
    {
        public static Pie instance { get; private set; }
        public enum ProgressState
        {
            Normal,
            Passed
        }

        private ProgressState state = ProgressState.Normal;

        private Image pie;
        private Image bg;
        private Image outline;
        private Text xtext;
        private RectTransform bgrt;
        private RectTransform fillrt;
        private RectTransform outlinert;
        private RectTransform xtextrt;
        private bool wasplay = false;
        private GameObject canvasobj;

        void Awake()
        {
            instance = this;
        }
        void OnDestroy()
        {
            if (instance == this)
            {
                if (canvasobj != null)
                {
                    Destroy(canvasobj);
                }
                instance = null;
            }
        }
        public void SetState(ProgressState newstate)
        {
            state = newstate;
        }

        void Start()
        {
            canvasobj = new GameObject("PieProgressCanvas");
            Canvas canvas = canvasobj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasobj.AddComponent<CanvasScaler>();
            canvasobj.AddComponent<GraphicRaycaster>();
            DontDestroyOnLoad(canvasobj);

            Texture2D circletex = new Texture2D(128, 128);
            Texture2D ringtex = new Texture2D(128, 128);
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(64f, 64f));
                    if (dist <= 60f)
                    {
                        circletex.SetPixel(x, y, Color.white);
                    }
                    else
                    {
                        circletex.SetPixel(x, y, Color.clear);
                    }

                    if (dist >= 56f && dist <= 60f)
                    {
                        ringtex.SetPixel(x, y, Color.white);
                    }
                    else
                    {
                        ringtex.SetPixel(x, y, Color.clear);
                    }
                }
            }
            circletex.Apply();
            ringtex.Apply();

            Sprite circlespr = Sprite.Create(circletex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
            Sprite ringspr = Sprite.Create(ringtex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));

            var bgobj = new GameObject("PieBackground");
            bgobj.transform.SetParent(canvasobj.transform);
            bg = bgobj.AddComponent<Image>();
            bg.sprite = circlespr;

            var outlineobj = new GameObject("PieOutline");
            outlineobj.transform.SetParent(canvasobj.transform);
            outline = outlineobj.AddComponent<Image>();
            outline.sprite = ringspr;

            var fillobj = new GameObject("PieProgressImage");
            fillobj.transform.SetParent(canvasobj.transform);
            pie = fillobj.AddComponent<Image>();
            pie.type = Image.Type.Filled;
            pie.fillMethod = Image.FillMethod.Radial360;
            pie.fillOrigin = (int)Image.Origin360.Top;
            pie.fillClockwise = true;
            pie.sprite = circlespr;
            pie.color = new Color32(255, 255, 255, 150);
            pie.fillAmount = 0.0f;

            bgrt = bgobj.GetComponent<RectTransform>();
            fillrt = fillobj.GetComponent<RectTransform>();
            outlinert = outlineobj.GetComponent<RectTransform>();
            foreach (RectTransform rt in new RectTransform[] { bgrt, fillrt, outlinert })
            {
                rt.anchorMin = new Vector2(1, 1);
                rt.anchorMax = new Vector2(1, 1);
                rt.pivot = new Vector2(1, 1);
            }

            var textobj = new GameObject("PieXAccText");
            textobj.transform.SetParent(canvasobj.transform);
            xtext = textobj.AddComponent<Text>();
            xtext.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            xtext.fontSize = 14;
            xtext.alignment = TextAnchor.MiddleCenter;
            xtext.color = Color.white;
            xtextrt = textobj.GetComponent<RectTransform>();
            xtextrt.anchorMin = new Vector2(1, 1);
            xtextrt.anchorMax = new Vector2(1, 1);
            xtextrt.pivot = new Vector2(0.5f, 0.5f);
            xtextrt.sizeDelta = new Vector2(200, 30);
        }

        void Update()
        {
            if (pie != null)
            {
                bool isplay = scrController.instance != null && scrConductor.instance != null && scrConductor.instance.isGameWorld && !scrController.instance.paused;

                if (isplay)
                {
                    if (!wasplay)
                    {
                        state = ProgressState.Normal;
                        wasplay = true;
                    }

                    pie.gameObject.SetActive(true);
                    bg.gameObject.SetActive(true);
                    outline.gameObject.SetActive(true);
                    bg.color = new Color32((byte)Main.sets.bgr, (byte)Main.sets.bgg, (byte)Main.sets.bgb, (byte)Main.sets.bga);
                    outline.color = new Color32((byte)Main.sets.borderr, (byte)Main.sets.borderg, (byte)Main.sets.borderb, (byte)Main.sets.bordera);

                    Vector2 pos = new Vector2(Main.sets.posx, Main.sets.posy);
                    Vector2 size = new Vector2(64f * Main.sets.scale, 64f * Main.sets.scale);
                    bgrt.anchoredPosition = pos;
                    bgrt.sizeDelta = size;
                    fillrt.anchoredPosition = pos;
                    fillrt.sizeDelta = size;
                    outlinert.anchoredPosition = pos;
                    outlinert.sizeDelta = size;

                    switch (state)
                    {
                        case ProgressState.Passed:
                            pie.color = new Color32((byte)Main.sets.passr, (byte)Main.sets.passg, (byte)Main.sets.passb, (byte)Main.sets.passa);
                            break;
                        case ProgressState.Normal:
                        default:
                            pie.color = new Color32((byte)Main.sets.fillr, (byte)Main.sets.fillg, (byte)Main.sets.fillb, (byte)Main.sets.filla);
                            break;
                    }

                    float pcomp = scrController.instance.percentComplete;
                    pie.fillAmount = Mathf.Clamp01(pcomp);

                    if (xtext != null)
                    {
                        if (Main.sets.usex)
                        {
                            xtext.gameObject.SetActive(true);
                            float xval = 1f;
                            try
                            {
                                if (scrController.instance != null && scrController.instance.mistakesManager != null)
                                {
                                    float perx = scrController.instance.mistakesManager.percentXAcc;
                                    xval = float.IsNaN(perx) ? 1f : perx;
                                }
                            }
                            catch
                            {
                                xval = 1f;
                            }

                            xtext.text = string.Format(Main.sets.xfmt, (xval * 100f).ToString("F1"));

                            Font customfont = null;
                            try
                            {
                                customfont = (Font)Resources.GetBuiltinResource(typeof(Font), Main.sets.font + ".ttf");
                            }
                            catch { }

                            if (customfont == null)
                            {
                                customfont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                            }
                            xtext.font = customfont;

                            Vector2 textpos = new Vector2(Main.sets.posx + Main.sets.xoffx, Main.sets.posy + Main.sets.xoffy);
                            xtextrt.anchoredPosition = textpos;
                        }
                        else
                        {
                            xtext.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    wasplay = false;
                    pie.gameObject.SetActive(false);
                    bg.gameObject.SetActive(false);
                    outline.gameObject.SetActive(false);
                    if (xtext != null)
                    {
                        xtext.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
    public static class WinPagePatch
    {
        public static void Postfix(scrController inst)
        {
            if (!Main.enabled) return;
            if (Pie.instance != null)
            {
                Pie.instance.SetState(Pie.ProgressState.Passed);
            }
        }
    }
}