﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConsoleApp1
{
    public class UnitTest1
    {
        public static void TestMethod1()
        {
            var A = new SquareMatrix(new Collection<double>() { 3, 2, 2, 6 });
            var b = new Vector(new Collection<double> { 2, -8 });
            var x = new Vector(new Collection<double> { -2, -2 });
            var r = SD.SteepestDescent(A, b, x);
            if ((r[0] - 2) >= 1.0e-2) throw new Exception();
            if ((r[1] + 2) >= 1.0e-2) throw new Exception();
        }
    }

    class SquareMatrix
    {
        public int N { get; private set; }
        private List<double> data;
        public SquareMatrix(int n)
        {
            N = n;
            data = new List<double>();
            for (int i = 0; i < n * n; ++i) data.Add(0);
        }

        public SquareMatrix(Collection<double> c)
        {
            data = new List<double>(c);
            var s = Math.Sqrt(c.Count);
            N = (int)Math.Floor(s);
            if (s != (double)N)
            {
                throw new Exception("Need to provide square matrix sized initializer.");
            }
        }

        public static Vector operator *(SquareMatrix a, Vector b)
        {
            Vector result = new Vector(a.N);
            Campy.Parallel.For(result.N, i =>
            {
                for (int j = 0; j < result.N; ++j)
                    result[i] += a.data[i * result.N + j] * b[j];
            });
            return result;
        }
    }

    class Vector
    {
        public int N { get; private set; }
        private List<double> data;

        public Vector(int n)
        {
            N = n;
            data = new List<double>();
            for (int i = 0; i < n; ++i) data.Add(0);
        }

        public double this[int i]
        {
            get
            {
                return data[i];
            }
            set
            {
                data[i] = value;
            }
        }

        public Vector(Collection<double> c)
        {
            data = new List<double>(c);
            N = c.Count;
        }

        public static double operator *(Vector a, Vector b)
        {
            double result = 0;
            for (int i = 0; i < a.N; ++i) result += a[i] * b[i];
            return result;
        }

        public static Vector operator *(double a, Vector b)
        {
            Vector result = new Vector(b.N);
            Campy.Parallel.For(b.N, i => { result[i] = a * b[i]; });
            return result;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            Vector result = new Vector(a.N);
            Campy.Parallel.For(a.N, i => { result[i] = a[i] - b[i]; });
            return result;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            Vector result = new Vector(a.N);
            Campy.Parallel.For(a.N, i => { result[i] = a[i] + b[i]; });
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Count; ++i)
            {
                sb.Append(data[i] + " ");
            }
            return sb.ToString();
        }
    }

    class SD
    {
        public static Vector SteepestDescent(SquareMatrix A, Vector b, Vector x)
        {
            // Similar to http://ta.twi.tudelft.nl/nw/users/mmbaumann/projects/Projekte/MPI2_slides.pdf
            for (; ; )
            {
                Vector r = b - A * x;
                double rr = r * r;
                double rAr = r * (A * r);
                if (Math.Abs(rAr) <= 1.0e-3) break;
                double a = (double)rr / (double)rAr;
                x = x + (a * r);
            }
            return x;
        }

        // https://www.coursera.org/learn/predictive-analytics/lecture/RhkFB/parallelizing-gradient-descent
        // "Hogwild! A lock-free approach to parallelizing stochastic gradient descent"
        // https://arxiv.org/abs/1106.5730

        // Parallelize vector and matrix operations
        // http://www.dcs.warwick.ac.uk/pmbs/pmbs14/PMBS14/Workshop_Schedule_files/8-CUDAHPCG.pdf

        // An introduction to the conjugate gradient method without the agonizing pain
        // https://www.cs.cmu.edu/~quake-papers/painless-conjugate-gradient.pdf

        // https://github.com/gmarkall/cuda_cg/blob/master/gpu_solve.cu
    }

    public class TwoDimArrayInts
    {

        public static void TwoDimArrayIntsT()
        {
            int e = 10;
            int ex0 = 3;
            int ex1 = 5;
            int[,] b = new int[ex0, ex1];
            for (int i = 0; i < ex0; ++i)
            for (int j = 0; j < ex1; ++j)
                b[i, j] = (i + 1) * (j + 1);
            Campy.Parallel.For(5, d =>
            {
                b[d % 3, d] = 33 + d;
            });
            if (b[0, 0] != 33) throw new Exception();
            if (b[1, 1] != 34) throw new Exception();
            if (b[2, 2] != 35) throw new Exception();
            if (b[0, 3] != 36) throw new Exception();
            if (b[1, 4] != 37) throw new Exception();
        }
    }

    class Program
    {
        static void StartDebugging()
        {
            Campy.Utils.Options.Set("graph_trace");
            Campy.Utils.Options.Set("module_trace");
            Campy.Utils.Options.Set("name_trace");
            Campy.Utils.Options.Set("cfg_construction_trace");
            Campy.Utils.Options.Set("dot_graph");
            Campy.Utils.Options.Set("jit_trace");
            Campy.Utils.Options.Set("memory_trace");
            Campy.Utils.Options.Set("ptx_trace");
            Campy.Utils.Options.Set("state_computation_trace");
            Campy.Utils.Options.Set("continue_with_no_resolve");
            Campy.Utils.Options.Set("copy_trace");
            Campy.Utils.Options.Set("runtime_trace");
        }

        static void Main(string[] args)
        {
            StartDebugging();
            TwoDimArrayInts.TwoDimArrayIntsT();

        }
    }
}
