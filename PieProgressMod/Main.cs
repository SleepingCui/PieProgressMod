using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace PieProgressMod
{
    public static class Main
    {
        public static bool enabled { get; private set; }
        public static Harmony harm { get; private set; }
        public static MainSettings sets { get; private set; }
        private static GameObject uiobj;

        public static bool Load(UnityModManager.ModEntry entry)
        {
            sets = UnityModManager.ModSettings.Load<MainSettings>(entry);

            entry.OnToggle = OnToggle;
            entry.OnSaveGUI = OnSaveGUI;
            entry.OnGUI = OnGUI;

            harm = new Harmony(entry.Info.Id);

            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry entry, bool val)
        {
            enabled = val;
            if (val)
            {
                harm.PatchAll(Assembly.GetExecutingAssembly());

                if (uiobj == null)
                {
                    uiobj = new GameObject("PieProgressManager");
                    uiobj.AddComponent<Pie>();
                    Object.DontDestroyOnLoad(uiobj);
                }
            }
            else
            {
                harm.UnpatchAll(harm.Id);

                if (uiobj != null)
                {
                    Object.Destroy(uiobj);
                    uiobj = null;
                }
            }
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry entry)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("X:", GUILayout.Width(160));
            sets.posx = GUILayout.HorizontalSlider(sets.posx, -2000f, 1000f, GUILayout.Width(200));
            GUILayout.Label(sets.posx.ToString("F0"), GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Y:", GUILayout.Width(160));
            sets.posy = GUILayout.HorizontalSlider(sets.posy, -1000f, 1000f, GUILayout.Width(200));
            GUILayout.Label(sets.posy.ToString("F0"), GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", GUILayout.Width(160));
            sets.scale = GUILayout.HorizontalSlider(sets.scale, 0.1f, 100f, GUILayout.Width(200));
            GUILayout.Label(sets.scale.ToString("F1"), GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("默认颜色");

            GUILayout.BeginHorizontal();
            GUILayout.Label("R:", GUILayout.Width(160));
            sets.fillr = (int)GUILayout.HorizontalSlider(sets.fillr, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.fillr.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("G:", GUILayout.Width(160));
            sets.fillg = (int)GUILayout.HorizontalSlider(sets.fillg, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.fillg.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("B:", GUILayout.Width(160));
            sets.fillb = (int)GUILayout.HorizontalSlider(sets.fillb, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.fillb.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("A:", GUILayout.Width(160));
            sets.filla = (int)GUILayout.HorizontalSlider(sets.filla, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.filla.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.Label("通关填充颜色");

            GUILayout.BeginHorizontal();
            GUILayout.Label("R:", GUILayout.Width(160));
            sets.passr = (int)GUILayout.HorizontalSlider(sets.passr, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.passr.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("G:", GUILayout.Width(160));
            sets.passg = (int)GUILayout.HorizontalSlider(sets.passg, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.passg.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("B:", GUILayout.Width(160));
            sets.passb = (int)GUILayout.HorizontalSlider(sets.passb, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.passb.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("A:", GUILayout.Width(160));
            sets.passa = (int)GUILayout.HorizontalSlider(sets.passa, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.passa.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.Label("背景颜色");

            GUILayout.BeginHorizontal();
            GUILayout.Label("R:", GUILayout.Width(160));
            sets.bgr = (int)GUILayout.HorizontalSlider(sets.bgr, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.bgr.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            GUILayout.Label("G:", GUILayout.Width(160));
            sets.bgg = (int)GUILayout.HorizontalSlider(sets.bgg, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.bgg.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            GUILayout.Label("B:", GUILayout.Width(160));
            sets.bgb = (int)GUILayout.HorizontalSlider(sets.bgb, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.bgb.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("A:", GUILayout.Width(160));
            sets.bga = (int)GUILayout.HorizontalSlider(sets.bga, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.bga.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.Label("边框颜色");

            GUILayout.BeginHorizontal();
            GUILayout.Label("R:", GUILayout.Width(160));
            sets.borderr = (int)GUILayout.HorizontalSlider(sets.borderr, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.borderr.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("G:", GUILayout.Width(160));
            sets.borderg = (int)GUILayout.HorizontalSlider(sets.borderg, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.borderg.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("B:", GUILayout.Width(160));
            sets.borderb = (int)GUILayout.HorizontalSlider(sets.borderb, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.borderb.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("A:", GUILayout.Width(160));
            sets.bordera = (int)GUILayout.HorizontalSlider(sets.bordera, 0, 255, GUILayout.Width(200));
            GUILayout.Label(sets.bordera.ToString(), GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private static void OnSaveGUI(UnityModManager.ModEntry entry)
        {
            sets.Save(entry);
        }
    }
}