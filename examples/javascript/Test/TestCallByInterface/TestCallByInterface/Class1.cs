﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
[assembly: Obfuscation(Feature = "script")]

namespace TestCallByInterface
{
    public interface IClass1
    {
         object Foo(object e);
    }

    public class Class1 : IClass1
    {
        public object Foo(object e)
        {
            return e;
        }
    }

    public static class Class2
    {
        static object InvokeFoo(this IClass1 e)
        {
            return e.Foo(null);
        }
    }
}
