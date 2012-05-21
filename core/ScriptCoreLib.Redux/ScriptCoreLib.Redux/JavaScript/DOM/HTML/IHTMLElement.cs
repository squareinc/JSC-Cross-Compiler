﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib.JavaScript.FileAPI;

namespace ScriptCoreLib.JavaScript.DOM.HTML
{
    public class IHTMLElement : IElement
    {
        #region event ondragover
        public event System.Action<DragEvent> ondragover
        {
            [Script(DefineAsStatic = true)]
            add
            {
                base.InternalEvent(true, value, "dragover");
            }
            [Script(DefineAsStatic = true)]
            remove
            {
                base.InternalEvent(false, value, "dragover");
            }
        }
        #endregion

        #region event ondrop
        public event System.Action<DragEvent> ondrop
        {
            [Script(DefineAsStatic = true)]
            add
            {
                base.InternalEvent(true, value, "drop");
            }
            [Script(DefineAsStatic = true)]
            remove
            {
                base.InternalEvent(false, value, "drop");
            }
        }
        #endregion
    }
}
