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

    struct MySTruct7
    {
        public byte _a;
        public bool Equals(object obj)
        {
            return false;
        }

        public int GetHashCode()
        {
            return 0;
        }
    }

    struct MyStruct8
    {
        MySTruct7 x;
        MySTruct7 y;
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

        [Benchmark(InnerIterationCount = iterCount)]
        /*[InlineData("R", 104234.343)]
        [InlineData("R", double.MinValue / 2)]
        [InlineData("R", Math.PI)]
        [InlineData("R", Math.E)]
        [InlineData("R", double.MaxValue)]
        [InlineData("R", double.MinValue)]
        [InlineData("R", double.NaN)]
        [InlineData("R", double.PositiveInfinity)]
        [InlineData("R", double.NegativeInfinity)]
        [InlineData("R", 2.2250738585072009E-308)]
        [InlineData("R", -2.2250738585072009E-308)]
        [InlineData("R", 1.1125369292536E-308)]
        [InlineData("R", -0.0)]
        [InlineData("R", 0.0)]*/
        public void TestDoubleZero()
        {
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
