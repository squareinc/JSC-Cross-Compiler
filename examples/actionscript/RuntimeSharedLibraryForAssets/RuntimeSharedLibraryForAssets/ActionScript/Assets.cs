﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.ActionScript;

////[assembly: ScriptResources(RuntimeSharedLibraryForAssets.ActionScript.Assets.Path)]

namespace RuntimeSharedLibraryForAssets.ActionScript
{
	/// <summary>
	/// This class should define all links and references to 
	/// external and embeded assets.
	/// </summary>
	[Script]
	public class Assets
	{
		/// <summary>
		/// The files from solution folder 'web/assets/RuntimeSharedLibraryForAssets' 
		/// that are marked as 'Embedded Resource' will be extracted by jsc
		/// to this path. The value only reflects the real folder.
		/// </summary>
		public const string Path = "assets/RuntimeSharedLibraryForAssets";

		public static readonly Assets Default = new Assets();

		public Class this[string e]
		{
			[EmbedByFileName]
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
