﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class UILogin : AHotBase
{
    public static string CachedUsername
    {
        get
        {
            return PlayerPrefs.GetString("un");
        }
        set
        {
            PlayerPrefs.SetString("un", value);
        }
    }
    public static string token { get; set; }
    protected override void InitComponents()
    {
        var inputUsername = FindWidget<InputField>("inputUsername");
        if (!string.IsNullOrEmpty(CachedUsername))
        {
            inputUsername.text = CachedUsername;
        }

        var inputPassword = FindWidget<InputField>("inputPassword");
        var btnLogin = FindWidget<Button>("btnLogin");
        btnLogin.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(inputUsername.text))
            {
                return;
            }
            if (string.IsNullOrEmpty(inputPassword.text))
            {
                return;
            }
            btnLogin.enabled = false;
            var username = inputUsername.text;
            var password = inputPassword.text;

            UStaticWebRequests.DoLogin(username, Utils.MD5Hash(password),
                (jres) =>
                {
                    btnLogin.enabled = true;
                    CachedUsername = jres["username"].ToString();
                    token = jres["token"].ToString();

                    UnloadThis();
                    LoadAnother<UIMain>();
                }
                , (err) =>
                {
                    btnLogin.enabled = true;
                    UIAlert.Show("登录失败，" + err);
                }
                , (error) =>
                {
                    btnLogin.enabled = true;
                    UIAlert.Show("登录失败，网络错误：" + error);
                });
        });
        var btnRegister = FindWidget<Button>("btnRegister");
        btnRegister.onClick.AddListener(() =>
        {
            UnloadThis();

            LoadAnother<UIRegister>();
        });
    }
}

