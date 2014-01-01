﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptCoreLib.ActionScript.BCLImplementation.System.Text
{
    [Script(Implements = typeof(global::System.Text.StringBuilder))]
    internal class __StringBuilder
    {
        public virtual int Capacity
        {
            get
            {
                return _Value.Length;

            }
            set
            {

            }
        }

        public __StringBuilder()
        {

        }

        string _Value = "";

        public __StringBuilder Append(bool e)
        {
            _Value += e;

            return this;
        }

        public __StringBuilder Append(double e)
        {
            _Value += e;

            return this;
        }

        public __StringBuilder Append(uint e)
        {
            _Value += e;

            return this;
        }

        public __StringBuilder Append(int e)
        {
            _Value += e;

            return this;
        }

        public __StringBuilder Append(string e)
        {
            _Value += e;

            return this;
        }

        public __StringBuilder Append(char e)
        {
            _Value += new string(e, 1);

            return this;
        }

        public __StringBuilder Append(object value)
        {
            if (value != null)
            {
                _Value += value.ToString();
            }

            return this;
        }

        public __StringBuilder AppendLine()
        {
            return Append(Environment.NewLine);
        }

        public __StringBuilder AppendLine(string value)
        {
            return Append(value).AppendLine();
        }


        public override string ToString()
        {
            return _Value;
        }
    }
}
