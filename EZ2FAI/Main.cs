﻿using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager.ModEntry;
using HarmonyLib;
using System.Reflection;
using TMPro;
using ADOFAI;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;

public class Main
{
    public static Harmony harmony;
    public static ModLogger Logger;
    public static bool Modon = false;
    public static bool isRunning = false;

    public static string username = "";
    public static string usernametemp = "";

    public static string level_author = "";
    public static string level_song = "";
    public static string level_artist = "";
    public static string level_songtist = "";

    public static float play_XA = 0;

    public static int play_TE = 0;
    public static int play_VE = 0;
    public static int play_EP = 0;
    public static int play_P = 0;
    public static int play_LP = 0;
    public static int play_VL = 0;
    public static int play_TL = 0;

    public static string nmargin;
    public static string smargin;
    public static string lmargin;
    public static float angle;
    public static HitMargin Lenient;
    public static HitMargin Normal;
    public static HitMargin Strict;

    public static GameObject mp;
    public static TextMeshProUGUI userNameTxt;
    public static TextMeshProUGUI autherTxt;
    public static TextMeshProUGUI songtistTxt;
    public static TextMeshProUGUI judgeRateTxt;
    public static TextMeshProUGUI[] judgeTexts = new TextMeshProUGUI[7];
    public static Image progressImage;
    public static AssetBundleLoadResult result;
    public static AssetBundle asset;

    public static float X = 0.16f;
    public static float Y = 0.1f;
    public static float S = 0.7f;



    public static bool Start(UnityModManager.ModEntry modEntry)
    {
        Logger = modEntry.Logger;
        username = "Guest-" + RandomStringGenerator.GenerateRandomString(8);
        usernametemp = username;
        

        modEntry.OnToggle = (entry,value) =>
        {
            Modon = value;
            if(value)
            {
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                InitializeMP();
            }
            else
            {
                UnLoad(Path.GetDirectoryName(modEntry.Path));
                if(isRunning)
                {
                    isRunning = false;
                }
            }

            return true;
        };

        void InitializeMP()
        {
            try
            {
                asset = LoadAssetBundle(Path.GetDirectoryName(modEntry.Path));
                mp = UnityEngine.Object.Instantiate(asset.LoadAsset<GameObject>("MPCanvas 12"));
                UnityEngine.Object.DontDestroyOnLoad(mp);

                MPSet mpSet = new GameObject().AddComponent<MPSet>();
                UnityEngine.Object.DontDestroyOnLoad(mpSet);

                userNameTxt = mp.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
                autherTxt = mp.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();
                songtistTxt = mp.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
                judgeRateTxt = mp.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();

                for(int i = 0;i <= 6;i++)
                {
                    judgeTexts[i] = mp.transform.GetChild(0).GetChild(6).GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
                }
                progressImage = mp.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

                autherTxt.text = Main.level_author;
                songtistTxt.text = Main.level_songtist;
                userNameTxt.text = username;
            }
            catch(Exception ex)
            {
                Logger.Error($"Exception during InitializeMP: {ex}");
            }
        }

        modEntry.OnGUI = (entry) =>
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:",GUILayout.Width(0));
            if(GUILayout.Button("Apply",GUILayout.Width(120)))
            {
                username = usernametemp;
                userNameTxt.text = username; 
            }

            usernametemp = GUILayout.TextField(usernametemp);
            GUILayout.EndHorizontal();

            GUILayout.Label("P1BoxSet");
            GUILayout.BeginHorizontal();
            GUILayout.Label("X:", GUILayout.Width(20));
            X = float.Parse(GUILayout.TextField(X.ToString(), 6, GUILayout.Width(80)));
            X = GUILayout.HorizontalSlider(X, -0.4f, 1.4f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Y:", GUILayout.Width(20));
            Y = float.Parse(GUILayout.TextField(Y.ToString(), 6, GUILayout.Width(80)));
            Y = GUILayout.HorizontalSlider(Y, -0.2f, 1.2f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("S:", GUILayout.Width(20));
            S = float.Parse(GUILayout.TextField(S.ToString(), 6, GUILayout.Width(80)));
            S = GUILayout.HorizontalSlider(S, 0f, 2f);
            GUILayout.EndHorizontal();
        };


        return true;
    }


    public static AssetBundle LoadAssetBundle(string modPath)
    {
        string path = Path.Combine(modPath, "mp");
        try
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            return assetBundle;
        }
        catch (Exception e)
        {
            Logger.Error($"Error loading asset bundle {modPath}");
            Logger.Error(e.Message);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            return assetBundle;
        }
    }

    public static void UnLoad(string modPath)
    {
        try
        {
            UnityEngine.Object.Destroy(mp);
            asset.Unload(true);
        }
        catch (Exception e)
        {
            Logger.Log("=================");
            Logger.Error(e.Message);
            UnityEngine.Object.Destroy(mp); 
        }
    }

}