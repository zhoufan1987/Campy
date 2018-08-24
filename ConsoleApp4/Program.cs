﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Campy;
using System.Linq;

namespace ConsoleApp4
{
    class Base
    {
        public int a;
        public static int b;
    }
    class Sub1 : Base
    {
        public int c;
        public static int d;
    }
    class Sub2 : Base
    {
        public int c2;
        public static int d2;
    }
    class Sub3 : Sub1
    {
        public int e;
    }
    //public class IfThenElse
    //{
    //    public static void IfThenElseT()
    //    {
    //        int n = 4;

    //        var t1 = new List<int>();
    //        for (int i = 0; i < n; ++i) t1.Add(0);
    //        Campy.Parallel.For(n, i =>
    //        {
    //            if (i % 2 == 0)
    //                t1[i] = i * 20;
    //            else
    //                t1[i] = i * 30;
    //        });
    //        for (int i = 0; i < n; ++i)
    //            if (i % 2 == 0)
    //            {
    //                if (t1[i] != i * 20) throw new Exception();
    //            }
    //            else
    //            {
    //                if (t1[i] != i * 30) throw new Exception();
    //            }
    //    }
    //}
    class Program
    {
        static void StartDebugging()
        {
            Campy.Utils.Options.Set("debug_info_off");
            Campy.Utils.Options.Set("graph_trace");
            Campy.Utils.Options.Set("module_trace");
            Campy.Utils.Options.Set("name_trace");
            //     Campy.Utils.Options.Set("cfg_construction_trace");
 //                 Campy.Utils.Options.Set("cfg_construction_trace");
            Campy.Utils.Options.Set("dot_graph");
            Campy.Utils.Options.Set("jit_trace");
            Campy.Utils.Options.Set("memory_trace");
            Campy.Utils.Options.Set("ptx_trace");
            Campy.Utils.Options.Set("state_computation_trace");
            Campy.Utils.Options.Set("overview_import_computation_trace");
            //     Campy.Utils.Options.Set("detailed_import_computation_trace");
 //               Campy.Utils.Options.Set("detailed_import_computation_trace");
            Campy.Utils.Options.Set("continue_with_no_resolve");
            Campy.Utils.Options.Set("copy_trace");
        //    Campy.Utils.Options.Set("runtime_trace");
        }

        static void Main(string[] args)
        {
            StartDebugging();
            //Campy.Parallel.For(3, i =>
            //{
            //    var x = new System.ArgumentNullException("hi");
            //});
            Campy.Parallel.For(3, i =>
            {
                System.Console.WriteLine(i.ToString());
            });
            //Campy.Parallel.For(3, i =>
            //{
            //    Base o1 = new Base();
            //    Sub1 o2 = new Sub1();
            //    Sub2 o3 = new Sub2();
            //    Sub3 o4 = new Sub3();
            //    o4.a = 1;
            //    o4.c = 2;
            //    o4.e = 3;
            //});
        }
    }
}
