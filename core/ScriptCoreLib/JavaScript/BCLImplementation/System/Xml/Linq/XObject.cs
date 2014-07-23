﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScriptCoreLib.JavaScript.BCLImplementation.System.Xml.Linq
{

    [Script(Implements = typeof(XObject))]
    internal class __XObject
    {
        // X:\jsc.svn\examples\javascript\Test\TestShadowIFrame\TestShadowIFrame\Application.cs
        public event EventHandler<XObjectChangeEventArgs> Changed;

        internal virtual XElement InternalGetParent()
        {
            throw new NotSupportedException();
        }

        public XElement Parent
        {
            get
            {
                return this.InternalGetParent();
            }
        }
    }
}
