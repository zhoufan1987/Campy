﻿using Campy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;

namespace BitonicSort
{
    public class BitonicSorter
    {
        public static void swap(ref int i, ref int j)
        {
            int t = i;
            i = j;
            j = t;
        }

        // [Bat 68]	K.E. Batcher: Sorting Networks and their Applications. Proc. AFIPS Spring Joint Comput. Conf., Vol. 32, 307-314 (1968)
        // Work inefficient sort, because half the threads are unused.
        public static void SeqBitonicSort1(int[] a)
        {
            uint N = (uint)a.Length;
            int term = Bithacks.FloorLog2(N);
            for (int kk = 2; kk <= N; kk *= 2)
            {
                for (int jj = kk >> 1; jj > 0; jj = jj >> 1)
                {
                    int k = kk;
                    int j = jj;
                    for(int i = 0; i < N; ++i)
                    {
                        int ij = i ^ j;
                        if (ij > i)
                        {
                            if ((i & k) == 0)
                            {
                                if (a[i] > a[ij]) swap(ref a[i], ref a[ij]);
                            }
                            else // ((i & k) != 0)
                            {
                                if (a[i] < a[ij]) swap(ref a[i], ref a[ij]);
                            }
                        }
                    }
                }
            }
        }

        public static void BitonicSort1(int[] a)
        {
            Parallel.Sticky(a);
            uint N = (uint)a.Length;
            int term = Bithacks.FloorLog2(N);
            for (int kk = 2; kk <= N; kk *= 2)
            {
                for (int jj = kk >> 1; jj > 0; jj = jj >> 1)
                {
                    int k = kk;
                    int j = jj;
                    Campy.Parallel.For((int)N, (i) =>
                    {
                        int ij = i ^ j;
                        if (ij > i)
                        {
                            if ((i & k) == 0)
                            {
                                if (a[i] > a[ij]) swap(ref a[i], ref a[ij]);
                            }
                            else // ((i & k) != 0)
                            {
                                if (a[i] < a[ij]) swap(ref a[i], ref a[ij]);
                            }
                        }
                    });
                }
            }
            Parallel.Sync();
        }

        public static void SeqBitonicSort2(int[] a)
        {
            uint N = (uint)a.Length;
            int log2n = Bithacks.FloorLog2(N);
            for (int k = 0; k < log2n; ++k)
            {
                uint n2 = N / 2;
                int twok = Bithacks.Power2(k);
                for(int i = 0; i < n2; ++i)
                {
                    int imp2 = i % twok;
                    int cross = imp2 + 2 * twok * (int)(i / twok);
                    int paired = -1 - imp2 + 2 * twok * (int)((i + twok) / twok);
                    if (a[cross] > a[paired])
                    {
                        int t = a[cross];
                        a[cross] = a[paired];
                        a[paired] = t;
                    }
                }
                for (int j = k - 1; j >= 0; --j)
                {
                    int twoj = Bithacks.Power2(j);
                    for (int i = 0; i < n2; ++i)
                    {
                        int imp2 = i % twoj;
                        int cross = imp2 + 2 * twoj * (int)(i / twoj);
                        int paired = cross + twoj;
                        if (a[cross] > a[paired])
                        {
                            int t = a[cross];
                            a[cross] = a[paired];
                            a[paired] = t;
                        }
                    }
                }
            }
        }

        public static void BitonicSort2(int[] a)
        {
            Parallel.Sticky(a);
            uint N = (uint)a.Length;
            int log2n = Bithacks.FloorLog2(N);
            for (int k = 0; k < log2n; ++k)
            {
                uint n2 = N / 2;
                int twok = Bithacks.Power2(k);
                Campy.Parallel.For((int)n2, i =>
                {
                    int imp2 = i % twok;
                    int cross = imp2 + 2 * twok * (int)(i / twok);
                    int paired = -1 - imp2 + 2 * twok * (int)((i + twok) / twok);
                    if (a[cross] > a[paired])
                    {
                        int t = a[cross];
                        a[cross] = a[paired];
                        a[paired] = t;
                    }
                });
                for (int j = k - 1; j >= 0; --j)
                {
                    int twoj = Bithacks.Power2(j);
                    Campy.Parallel.For((int)n2, i =>
                    {
                        int imp2 = i % twoj;
                        int cross = imp2 + 2 * twoj * (int)(i / twoj);
                        int paired = cross + twoj;
                        if (a[cross] > a[paired])
                        {
                            int t = a[cross];
                            a[cross] = a[paired];
                            a[paired] = t;
                        }
                    });
                }
            }
            Parallel.Sync();
        }
    }

    [TestClass]
    public class BitonicSortT
    {
        [TestMethod]
        public void BitonicSort()
        {
            Random rnd = new Random();
            int N = 8;
            {
                int[] a = Enumerable.Range(0, N).ToArray().OrderBy(x => rnd.Next()).ToArray();
                BitonicSorter.SeqBitonicSort1(a);
                for (int i = 0; i < N; ++i)
                    if (a[i] != i)
                        throw new Exception();
            }
            {
                int[] a = Enumerable.Range(0, N).ToArray().OrderBy(x => rnd.Next()).ToArray();
                BitonicSorter.SeqBitonicSort2(a);
                for (int i = 0; i < N; ++i)
                    if (a[i] != i)
                        throw new Exception();
            }
            {
                int[] a = Enumerable.Range(0, N).ToArray().OrderBy(x => rnd.Next()).ToArray();
                BitonicSorter.BitonicSort1(a);
                for (int i = 0; i < N; ++i)
                    if (a[i] != i)
                        throw new Exception();
            }
            {
                int[] a = Enumerable.Range(0, N).ToArray().OrderBy(x => rnd.Next()).ToArray();
                BitonicSorter.BitonicSort2(a);
                for (int i = 0; i < N; ++i)
                    if (a[i] != i)
                        throw new Exception();
            }
        }
    }

    // Support
    public class Bithacks
    {
        static bool preped;

        static int[] LogTable256 = new int[256];

        static void prep()
        {
            LogTable256[0] = LogTable256[1] = 0;
            for (int i = 2; i < 256; i++)
            {
                LogTable256[i] = 1 + LogTable256[i / 2];
            }
            LogTable256[0] = -1; // if you want log(0) to return -1

            // Prepare the reverse bits table.
            prep_reverse_bits();
        }

        public static int FloorLog2(uint v)
        {
            if (!preped)
            {
                prep();
                preped = true;
            }
            int r; // r will be lg(v)
            uint tt; // temporaries

            if ((tt = v >> 24) != 0)
            {
                r = (24 + LogTable256[tt]);
            }
            else if ((tt = v >> 16) != 0)
            {
                r = (16 + LogTable256[tt]);
            }
            else if ((tt = v >> 8) != 0)
            {
                r = (8 + LogTable256[tt]);
            }
            else
            {
                r = LogTable256[v];
            }
            return r;
        }

        public static long FloorLog2(ulong v)
        {
            if (!preped)
            {
                prep();
                preped = true;
            }
            long r; // r will be lg(v)
            ulong tt; // temporaries

            if ((tt = v >> 56) != 0)
            {
                r = (56 + LogTable256[tt]);
            }
            else if ((tt = v >> 48) != 0)
            {
                r = (48 + LogTable256[tt]);
            }
            else if ((tt = v >> 40) != 0)
            {
                r = (40 + LogTable256[tt]);
            }
            else if ((tt = v >> 32) != 0)
            {
                r = (32 + LogTable256[tt]);
            }
            else if ((tt = v >> 24) != 0)
            {
                r = (24 + LogTable256[tt]);
            }
            else if ((tt = v >> 16) != 0)
            {
                r = (16 + LogTable256[tt]);
            }
            else if ((tt = v >> 8) != 0)
            {
                r = (8 + LogTable256[tt]);
            }
            else
            {
                r = LogTable256[v];
            }
            return r;
        }

        public static int CeilingLog2(uint v)
        {
            int r = Bithacks.FloorLog2(v);
            if (r < 0)
                return r;
            if (v != (uint)Bithacks.Power2((uint)r))
                return r + 1;
            else
                return r;
        }

        public static int Power2(uint v)
        {
            if (v == 0)
                return 1;
            else
                return (int)(2 << (int)(v - 1));
        }

        public static int Power2(int v)
        {
            if (v == 0)
                return 1;
            else
                return (int)(2 << (int)(v - 1));
        }

        static byte[] BitReverseTable256 = new byte[256];

        static void R2(ref int i, byte v)
        {
            BitReverseTable256[i++] = v;
            BitReverseTable256[i++] = (byte)(v + 2 * 64);
            BitReverseTable256[i++] = (byte)(v + 1 * 64);
            BitReverseTable256[i++] = (byte)(v + 3 * 64);
        }

        static void R4(ref int i, byte v)
        {
            R2(ref i, v);
            R2(ref i, (byte)(v + 2 * 16));
            R2(ref i, (byte)(v + 1 * 16));
            R2(ref i, (byte)(v + 3 * 16));
        }

        static void R6(ref int i, byte v)
        {
            R4(ref i, v);
            R4(ref i, (byte)(v + 2 * 4));
            R4(ref i, (byte)(v + 1 * 4));
            R4(ref i, (byte)(v + 3 * 4));
        }

        static void prep_reverse_bits()
        {
            int i = 0;
            R6(ref i, 0);
            R6(ref i, 2);
            R6(ref i, 1);
            R6(ref i, 3);
        }

        public static byte ReverseBits(byte from)
        {
            if (!preped)
            {
                prep();
                preped = true;
            }
            return BitReverseTable256[from];
        }

        public static Int32 ReverseBits(Int32 from)
        {
            if (!preped)
            {
                prep();
                preped = true;
            }
            Int32 result = 0;
            for (int i = 0; i < sizeof(Int32); ++i)
            {
                result = result << 8;
                result |= BitReverseTable256[(byte)(from & 0xff)];
                from = from >> 8;
            }
            return result;
        }

        public static UInt32 ReverseBits(UInt32 from)
        {
            if (!preped)
            {
                prep();
                preped = true;
            }
            UInt32 result = 0;
            for (int i = 0; i < sizeof(UInt32); ++i)
            {
                result = result << 8;
                result |= BitReverseTable256[(byte)(from & 0xff)];
                from = from >> 8;
            }
            return result;
        }

        static int Ones(uint x)
        {
            // 32-bit recursive reduction using SWAR...  but first step is mapping 2-bit values
            // into sum of 2 1-bit values in sneaky way
            x -= ((x >> 1) & 0x55555555);
            x = (((x >> 2) & 0x33333333) + (x & 0x33333333));
            x = (((x >> 4) + x) & 0x0f0f0f0f);
            x += (x >> 8);
            x += (x >> 16);
            return (int)(x & 0x0000003f);
        }

        public static int xFloorLog2(uint x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (Bithacks.Ones(x) - 1);
        }

        public static int Log2(uint x)
        {
            return FloorLog2(x);
        }

        public static int Log2(int x)
        {
            return FloorLog2((uint)x);
        }
    }
}
