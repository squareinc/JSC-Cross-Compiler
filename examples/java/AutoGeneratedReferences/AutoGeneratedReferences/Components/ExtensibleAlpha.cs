﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoGeneratedReferences.Components
{
	internal class ExtensibleAlpha : Extensible
	{
		public string More;

		public string ToMoreText()
		{
			return More + ": " + this.ToString();
		}
	}
}
