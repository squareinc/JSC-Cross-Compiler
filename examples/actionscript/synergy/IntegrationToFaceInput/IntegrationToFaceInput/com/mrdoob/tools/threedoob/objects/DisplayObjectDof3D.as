package com.mrdoob.tools.threedoob.objects {	import flash.display.DisplayObject;	import flash.filters.BlurFilter;			/**	 * @author mrdoob	 */	public class DisplayObjectDof3D extends DisplayObject3D	{		private var max_blur : int;				public function DisplayObjectDof3D(source : DisplayObject, max_blur : int = 20)		{			super(source);			this.max_blur = max_blur;		}		public function setBlur(level : Number) : void		{			level = (level > 1) ? 1 : level;			level = (level < 0) ? 0 : level;			content.filters = [new BlurFilter(max_blur * level, max_blur * level, 1)];		}	}}