﻿using Newtonsoft.Json.Linq;
using StrawPollNET.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot_1_0.Services
{
    public class PollRequest
    {
        public PollRequest()
        {
        }

        public PollRequest(string title, List<string> options, DupCheck dupcheck, bool multi, bool captcha)
        {
            Title = title;
            Options = options;
            Dupcheck = dupcheck;
            Multi = multi;
            Captcha = captcha;
        }

        public string Title { get; set; }
        public List<string> Options { get; set; }
        public DupCheck? Dupcheck { get; set; }
        public bool? Multi { get; set; }
        public bool? Captcha { get; set; }
    }

    public class Request
    {
        public static string CreateRequest(PollRequest req)
        {
            JObject obj = new JObject
            {
                { "title", req.Title },
                { "options", new JArray(req.Options) },
                { "multi", req.Multi ?? true }
            };

            switch (req.Dupcheck ?? DupCheck.Normal)
            {
                case DupCheck.Normal:
                    obj.Add("dupcheck", "normal");
                    break;
                case DupCheck.Permissive:
                    obj.Add("dupcheck", "permissive");
                    break;
                case DupCheck.Disabled:
                    obj.Add("dupcheck", "disabled");
                    break;
            }

            obj.Add("captcha", req.Captcha ?? false);

            return obj.ToString();
        }

        internal static string CreateRequest(string title, List<string> options, bool multi = true, DupCheck dupcheck = DupCheck.Normal, bool capcha = false)
        {
            JObject obj = new JObject
            {
                { "title", title },
                { "options", new JArray(options) },
                { "multi", multi }
            };

            switch (dupcheck)
            {
                case DupCheck.Normal:
                    obj.Add("dupcheck", "normal");
                    break;
                case DupCheck.Permissive:
                    obj.Add("dupcheck", "permissive");
                    break;
                case DupCheck.Disabled:
                    obj.Add("dupcheck", "disabled");
                    break;
            }

            obj.Add("captcha", capcha);

            return obj.ToString();
        }
    }
}
