﻿using ScriptCoreLib.JavaScript.BCLImplementation.System.Reflection;
using ScriptCoreLib.JavaScript.Runtime;
using ScriptCoreLib.JavaScript.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCoreLib.JavaScript.DOM
{
    public static class HistoryExtensions
    {
        static void location_reload()
        {
            //Native.document.title = "manual reload";
            //Native.document.body.style.backgroundColor = "gray";

            //since we do not know how to unwind, restart the engines
            // unless that stat tells us that it can unwind!

            // a slow down may also cause this?
            Native.document.location.reload();
        }

        class __entry
        {
            public Func<bool> unwind;
            public HistoryDetails unwind_data;

        }

        static Action later = delegate { };
        public static Action<Action> yield = y => later += y;


        static void onpopstate(PopStateEvent e)
        {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();



            var x_history_state = new Stack<Func<Task<__entry>>>();
            var x_e_state = new Stack<Func<Task<__entry>>>();

            #region y
            Action<string, Stack<Func<Task<__entry>>>, HistoryDetails> y = null;

            y = (sourcehint, x, state) =>
            {
                if (state == null)
                    return;

                //dynamic state = xstate;

                // if there is parent, we have to restore that first?

                string hint = state.hint;
                string url = state.url;
                bool exclusive = state.exclusive;

                Console.WriteLine("onpopstate " + new { sourcehint, hint, url, exclusive, state.state });

                dynamic invoke = state.invoke;

                string MethodToken = invoke.function;
                object arguments = invoke.arguments;
                object arg0 = ((object[])arguments)[0];


                #region __unwind
                TaskCompletionSource<HistoryScope<object>> __unwind = null;

                Func<TaskCompletionSource<HistoryScope<object>>> __get_unwind =
                    delegate
                    {
                        // ok, something is listening to inline unwind.
                        // lets wait for the event then and not reload

                        Console.WriteLine("__get_unwind");

                        if (__unwind == null)
                            __unwind = new TaskCompletionSource<HistoryScope<object>>();

                        return __unwind;
                    };
                #endregion



                var scope = new HistoryScope<object>
                {
                    __state = arg0,
                    __TaskCompletionSource = __get_unwind
                };


                x.Push(
                    delegate
                    {
                        var z = new TaskCompletionSource<__entry>();

                        #region missing ?
                        var sw = new Stopwatch();
                        sw.Start();

                        if (!Expando.Of(Native.self).Contains(MethodToken))
                        {
                            //{ hint = typeof(y) } missing { MethodToken = CAAABoz2jD6nJMVTcci_a_bQ }


                            Console.WriteLine(new { hint } + " missing " + new { MethodToken });

                            //Could not load type 'ctor>b__6>d__17' from assembly 'WorkerInsideSecondaryApplicationWithStateReplace.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'.
                            // script: error JSC1000: No implementation found for this native method, please implement [System.Threading.Tasks.Task.ContinueWith(System.Action`1[[System.Threading.Tasks.Task, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]])]
                        }
                        #endregion



                        IFunction.ByName(MethodToken).ContinueWithResult(
                            f =>
                            {
                                // { hint = typeof(y) } ok { MethodToken = CQAABoz2jD6nJMVTcci_a_bQ, ElapsedMilliseconds = 171 }
                                Console.WriteLine(new { hint } + " ok " + new { MethodToken, sw.ElapsedMilliseconds });



                                f.apply(null, scope);
                                //go(source);

                                z.SetResult(
                                    new __entry
                                    {
                                        unwind_data = state,
                                        unwind =
                                           delegate
                                           {
                                               if (__unwind == null)
                                                   return true;

                                               // time to do inline unwind.
                                               __unwind.SetResult(scope);

                                               return false;
                                           }
                                    }
                                );
                            }
                        );

                        return z.Task;
                    }
                );

                var parent = (HistoryDetails)state.state;
                y(sourcehint + " parent", x, parent);
            };
            #endregion

            // crude cast. 
            y("e.state ", x_e_state, (HistoryDetails)e.state);
            y("history.state ", x_history_state, (HistoryDetails)Native.window.history.state);

            //Application onpopstate { e = { state = [object Object] }, history = { state = [object Object] } }

            // Application onpopstate { e = { state = 2 }, history = { state = 2 } }

            Console.WriteLine(
                "HistoryExtensions onpopstate " + new
                {
                    e.state,

                    e = new { state = x_e_state.Count },
                    history = new { state = x_history_state.Count },

                    //previous_Count,
                    HistoryScope.inline_unwind.Count
                }
            );

            //HistoryExtensions onpopstate { state = , e = { state = 0 }, history = { state = 0 }, Count = 1 }

            #region restoreall
            Action restoreall = null;

            restoreall = delegate
            {
                Console.WriteLine("at restoreall " + new { x_history_state.Count });

                if (x_history_state.Count > 0)
                {
                    x_history_state.Pop()().ContinueWithResult(
                        z =>
                        {
                            //Application onpopstate { e = { state = 2 }, history = { state = 2 }, Count = 1 }

                            #region exclusive
                            if (z.unwind_data.exclusive)
                            {
                                foreach (var unwind in HistoryScope.inline_unwind.AsEnumerable())
                                {
                                    var reload = unwind();

                                    if (reload)
                                    {
                                        throw new NotImplementedException("exclusve scope needs to unwind prior states, yet one of them asks for full reload. what to do?");
                                    }
                                }
                            }
                            #endregion

                            Console.WriteLine("restored: " + new { z.unwind_data });

                            HistoryScope.inline_unwind_data.Push(z.unwind_data);
                            HistoryScope.inline_unwind.Push(z.unwind);


                            restoreall();
                        }
                    );

                    return;
                }


                Console.WriteLine("at restoreall done! " + new { HistoryScope.inline_unwind.Count });
            };

            #endregion

            #region did we just move backward
            if ((HistoryScope.inline_unwind.Count - 1) == x_history_state.Count)
            {
                // Application onpopstate { e = { state = 0 }, history = { state = 1 }, Count = 0 }


                Console.WriteLine(" did we just move backward?");

                var unwind_data = (HistoryDetails)HistoryScope.inline_unwind_data.Pop();
                var unwind = HistoryScope.inline_unwind.Pop();

                // if exclusive we have to reactivete the remaining as if full reload?
                Console.WriteLine(new { unwind, unwind_data.url, unwind_data.exclusive, HistoryScope.inline_unwind.Count });

                var reload = unwind();

                Console.WriteLine(new { reload, HistoryScope.inline_unwind.Count });

                if (reload)
                {
                    location_reload();
                    return;
                }

                //previous_Count--;

                if (unwind_data.exclusive)
                {

                    HistoryScope.inline_unwind_data.Clear();
                    HistoryScope.inline_unwind.Clear();

                    restoreall();
                }

                return;
            }
            #endregion

            // HistoryExtensions onpopstate { state = [object Object], e = { state = 1 }, history = { state = 1 }, Count = 0 }


            #region did we just move forward?
            if ((HistoryScope.inline_unwind.Count + 1) == x_history_state.Count)
            {
                Console.WriteLine("onpopstate did we just move forward?");

                //var unwind = await x_history_state.First()();
                x_history_state.First()().ContinueWithResult(
                    z =>
                    {
                        Console.WriteLine("onpopstate go forward " + new { z.unwind_data.url, z.unwind_data.exclusive });

                        #region exclusive
                        if (z.unwind_data.exclusive)
                        {
                            foreach (var unwind in HistoryScope.inline_unwind.AsEnumerable())
                            {
                                var reload = unwind();

                                if (reload)
                                {
                                    throw new NotImplementedException("exclusve scope needs to unwind prior states, yet one of them asks for full reload. what to do?");
                                }
                            }
                        }
                        #endregion

                        HistoryScope.inline_unwind_data.Push(z.unwind_data);
                        HistoryScope.inline_unwind.Push(z.unwind);

                        Console.WriteLine(new { HistoryScope.inline_unwind.Count });

                    }
                );


                return;
            }
            #endregion

            //HistoryExtensions onpopstate { state = [object Object], e = { state = 2 }, history = { state = 2 }, Count = 2 }

            #region ? reload
            if (e.state != null)
            {
                // back: { e = { state = 2 }, history = { state = 2 }, previous_Count = 3 }
                // forward: { e = { state = 3 }, history = { state = 3 }, previous_Count = 2 }




                location_reload();

                return;
            }
            #endregion


            //previous_Count = x_history_state.Count;




            restoreall();
        }

        static HistoryExtensions()
        {
            //var previous_Count = -1;

            // each entry an try to prevent full reload and do inline unwind

            Console.WriteLine("HistoryExtensions");

            if (Native.window != null)
            {
                Native.window.onpopstate += e =>
                    {
                        onpopstate(e);

                        yield = y => y();
                        var z = later;
                        later = null;
                        z();
                    };
            }
        }

        public static void replaceState<T>(this History h, T state, Action<HistoryScope<T>> yield)
        {

            replaceState(h, state,

                Native.document.location.pathname,

                exclusive: false,
                yield: yield
                );
        }

        public static void replaceState<T>(this History h,
            T state,
            string url,
            bool exclusive,
            Action<HistoryScope<T>> yield
            )
        {
            // tested by
            // X:\jsc.svn\examples\javascript\async\AsyncHistoricActivities\AsyncHistoricActivities\Application.cs

            HistoryExtensions.yield(
                delegate
                {

                    Console.WriteLine("enter replaceState");

                    if (yield.Target != null)
                        if (yield.Target != Native.self)
                            throw new InvalidOperationException("we can only continue with global methods for now... " + new { yield.Target });

                    var MethodToken = ((__MethodInfo)yield.Method).MethodToken;


                    var data_state = (HistoryDetails)Native.window.history.state;

                    // HistoryExtensions onpopstate { state = [object Object], e = { state = 7 }, history = { state = 7 }, Count = 2 }

                    //        m.state = h.state;
                    //m.hint = 'ScriptCoreLib.JavaScript.DOM.HistoryExtensions.replaceState';

                    if (data_state != null)
                        data_state = (HistoryDetails)data_state.state;


                    var data = new HistoryDetails
                    {
                        // parent
                        state = data_state,

                        hint = "ScriptCoreLib.JavaScript.DOM.HistoryExtensions.replaceState",

                        exclusive = exclusive,
                        url = url,

                        // arguments:

                        invoke = new { function = MethodToken, arguments = new object[] { state } }
                    };

                    Console.WriteLine("before history.replaceState");
                    // IE throws __exc	Argument not optional
                    Native.window.history.replaceState(data, "", url);
                    Console.WriteLine("after history.replaceState");

                    #region __unwind
                    TaskCompletionSource<HistoryScope<T>> __unwind = null;

                    Func<TaskCompletionSource<HistoryScope<T>>> __get_unwind =
                        delegate
                        {
                            // ok, something is listening to inline unwind.
                            // lets wait for the event then and not reload

                            Console.WriteLine("__get_unwind [inline]");

                            if (__unwind == null)
                                __unwind = new TaskCompletionSource<HistoryScope<T>>();

                            return __unwind;
                        };
                    #endregion

                    var scope = new HistoryScope<T> { __state = state, __TaskCompletionSource = __get_unwind };

                    if (HistoryScope.inline_unwind.Count > 0)
                    {
                        HistoryScope.inline_unwind_data.Pop();
                        var unwind = HistoryScope.inline_unwind.Pop();
                        var reload = unwind();

                        if (reload)
                        {
                            throw new NotImplementedException("cant reload here can we");
                        }
                    }

                    #region exclusive
                    if (exclusive)
                    {
                        // this state is market exlusive.
                        // as such. we shall destroy any other state before we continue.
                        // if we go back in time
                        // we have to reactivate them ofcourse.

                        Console.WriteLine("HistoryExtensions pushState exlusive scope " + new { url });

                        foreach (var unwind in HistoryScope.inline_unwind.AsEnumerable())
                        {
                            var reload = unwind();

                            if (reload)
                            {
                                throw new NotImplementedException("exclusve scope needs to unwind prior states, yet one of them asks for full reload. what to do?");
                            }
                        }
                    }
                    #endregion

                    HistoryScope.inline_unwind_data.Push(data);
                    HistoryScope.inline_unwind.Push(
                         delegate
                         {
                             if (__unwind == null)
                                 return true;

                             // time to do inline unwind.
                             __unwind.SetResult(scope);

                             return false;
                         }
                    );

                    Console.WriteLine("before yield");
                    yield(scope);

                    Console.WriteLine("replaceState: " + new { HistoryScope.inline_unwind.Count });
                }
             );

        }



        public static void pushState<T>(this History h, T state, Action<HistoryScope<T>> yield)
        {

            pushState(h, state, Native.document.location.pathname, exclusive: false, yield: yield);
        }

        public static void pushState<T>(
            this History h,
            T state,
            string url,
            bool exclusive,
            Action<HistoryScope<T>> yield
            )
        {
            // exclusive parent means a sub state will undo parent, so they wont exist at the same time
            // https://sites.google.com/a/jsc-solutions.net/backlog/knowledge-base/2013/201312/20131222-form

            HistoryExtensions.yield(
                 delegate
                 {

                     if (yield.Target != null)
                         if (yield.Target != Native.self)
                             throw new InvalidOperationException(
                                 "we can only continue with global methods for now... " + new { yield.Target }
                             );



                     var MethodToken = ((__MethodInfo)yield.Method).MethodToken;

                     var data = new HistoryDetails
                     {
                         state = Native.window.history.state,

                         hint = "ScriptCoreLib.JavaScript.DOM.HistoryExtensions.pushState",

                         exclusive = exclusive,
                         url = url,

                         // arguments:

                         invoke = new { function = MethodToken, arguments = new object[] { state } }
                     };

                     // http://stackoverflow.com/questions/6460377/html5-history-api-what-is-the-max-size-the-state-object-can-be
                     Console.WriteLine("HistoryExtensions pushState before: " + new { exclusive, Native.window.history.length });

                     // fck ie
                     Native.window.history.pushState(data, "", url);

                     Console.WriteLine("HistoryExtensions pushState after: " + new { Native.window.history.length });


                     #region __unwind
                     TaskCompletionSource<HistoryScope<T>> __unwind = null;

                     Func<TaskCompletionSource<HistoryScope<T>>> __get_unwind =
                         delegate
                         {
                             // ok, something is listening to inline unwind.
                             // lets wait for the event then and not reload

                             Console.WriteLine("HistoryExtensions pushState __get_unwind [inline]");

                             if (__unwind == null)
                                 __unwind = new TaskCompletionSource<HistoryScope<T>>();

                             return __unwind;
                         };
                     #endregion

                     var scope = new HistoryScope<T>
                     {
                         __state = state,
                         __TaskCompletionSource = __get_unwind
                     };


                     #region exclusive
                     if (exclusive)
                     {
                         // this state is market exlusive.
                         // as such. we shall destroy any other state before we continue.
                         // if we go back in time
                         // we have to reactivate them ofcourse.

                         Console.WriteLine("HistoryExtensions pushState exlusive scope " + new { url });

                         foreach (var unwind in HistoryScope.inline_unwind.AsEnumerable())
                         {
                             var reload = unwind();

                             if (reload)
                             {
                                 throw new NotImplementedException("exclusve scope needs to unwind prior states, yet one of them asks for full reload. what to do?");
                             }
                         }
                     }
                     #endregion

                     HistoryScope.inline_unwind_data.Push(data);
                     HistoryScope.inline_unwind.Push(
                          delegate
                          {
                              if (__unwind == null)
                                  return true;

                              // time to do inline unwind.
                              __unwind.SetResult(scope);

                              return false;
                          }
                     );



                     // activate the scope
                     Console.WriteLine("HistoryExtensions pushState before enter scope " + new { url });
                     yield(scope);
                     Console.WriteLine("HistoryExtensions pushState " + new { HistoryScope.inline_unwind.Count });
                 }
             );

        }


        sealed class HistoryDetails
        {
            public object state;

            public string hint;

            public bool exclusive;
            public string url;

            public object invoke;

        }
    }


}
