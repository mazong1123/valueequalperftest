using System;
using Xunit;
using Microsoft.Xunit.Performance;
using Microsoft.Xunit.Performance.Api;
using System.Reflection;
using System.Globalization;

namespace perftest
{
    struct DoubleStruct
    {
        public double value1;
        public double value2;
    }

    struct MyStruct2
    {
        public NeverEquals _x;
        public NeverEquals _y;
    }

    struct MyStruct3
    {
        public MyStruct2 _x;
        public MyStruct2 _y;
    }

    struct NeverEquals
    {
        public byte _a;

        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    struct CanEquals
    {
        public byte _a;
    }

    struct ComplexStruct
    {
        CanEquals x;
        CanEquals y;
        NeverEquals z;
    }

    struct MyStruct5
    {
        ComplexStruct x;
        ComplexStruct y;
        NeverEquals z;
    }

    class Cls
    {
        public byte _a;

        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    struct MyStruct6
    {
        public Cls _x;
        public Cls _y;
    }

    struct MyStruct7
    {
        public byte _a;
    }

    struct MyStruct8
    {
        MyStruct7 x;
        MyStruct7 y;
    }

    public class ValueEqualTest
    {
        const int iterCount = 200000;

        public static void Main(string[] args)
        {
            using (XunitPerformanceHarness h = new XunitPerformanceHarness(args))
            {
                string entryAssemblyPath = Assembly.GetEntryAssembly().Location;
                h.RunBenchmarks(entryAssemblyPath);
            }
        }

        public static void Warmup()
        {
            DoubleStruct d1 = new DoubleStruct();
            d1.value1 = 1;
            d1.value2 = 0.0;

            DoubleStruct d2 = new DoubleStruct();
            d2.value1 = 1;
            d2.value2 = -0.0;

            d1.Equals(d2);

            MyStruct8 s1 = new MyStruct8();
            MyStruct8 s2 = new MyStruct8();

            s1.Equals(s2);

            MyStruct3 t1 = new MyStruct3();
            MyStruct3 t2 = new MyStruct3();

            t1.Equals(t2);
        }

        [Benchmark(InnerIterationCount = iterCount)]
        public void TestDoubleZero()
        {
            Warmup();

            DoubleStruct d1 = new DoubleStruct();
            d1.value1 = 1;
            d1.value2 = 0.0;

            DoubleStruct d2 = new DoubleStruct();
            d2.value1 = 1;
            d2.value2 = -0.0;

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        d1.Equals(d2);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = iterCount)]
        public void TestStructNonOverriddenEquals()
        {
            Warmup();

            MyStruct8 s1 = new MyStruct8();
            MyStruct8 s2 = new MyStruct8();

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        s1.Equals(s2);
                    }
                }
            }
        }

        [Benchmark(InnerIterationCount = iterCount)]
        static void TestDeepOverriddenEquals()
        {
            Warmup();
            
            MyStruct3 s1 = new MyStruct3();
            MyStruct3 s2 = new MyStruct3();

            foreach (var iteration in Benchmark.Iterations)
            {
                using (iteration.StartMeasurement())
                {
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                    {
                        s1.Equals(s2);
                    }
                }
            }
        }
    }
}
