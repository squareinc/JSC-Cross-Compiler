﻿using ScriptCoreLib;
using ScriptCoreLib.JavaScript;
using ScriptCoreLib.JavaScript.Extensions;
using ScriptCoreLib.JavaScript.DOM.HTML;
using ScriptCoreLib.Shared.Drawing;
using ScriptCoreLib.Shared.Lambda;


namespace NatureBoyTestPad.js
{
    [Script, ScriptApplicationEntryPoint]
    public class NatureBoyTestPad
    {
        public NatureBoyTestPad()
        {

            var btn = new IHTMLButton("Hello World!").AttachToDocument();

            var counter = new IHTMLSpan().AttachTo(btn);

            counter.style.margin = "1em";

            var i = 0;

            btn.onclick += ev =>
                {
                    i++;

                    counter.innerText = "(" + i + ")";

                    counter.style.color = Color.FromRGB(
                        0xff.Random(),
                        0xff.Random(),
                        0xff.Random()
                    );
                };
        }

        static NatureBoyTestPad()
        {
            typeof(NatureBoyTestPad).SpawnTo(i => new NatureBoyTestPad());
        }

    }

}
