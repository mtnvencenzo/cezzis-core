namespace Cezzi.Azure.ServiceBus.Tests;

using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

public class ServiceBusMessageSerializerTests : ServiceTestBase
{
    [Fact]
    public void serializer___no_nulls_success()
    {
        var obj = GetCompleteTestObject();

        var bytes = ServiceBusMessageSerializer.SerializeToUtf8Bytes(obj);

        var json = Encoding.UTF8.GetString(bytes);

        var rebuilt = ServiceBusMessageSerializer.FromJsonString<TestObj>(json);
        rebuilt.ListChildren.Should().ContainSingle();
        rebuilt.EnumerableChildren.Should().ContainSingle();

        rebuilt.Should().BeEquivalentTo(obj);
    }

    [Fact]
    public void serializer___with_nulls_success()
    {
        var obj = GetCompleteTestObjectWithNulls();

        var bytes = ServiceBusMessageSerializer.SerializeToUtf8Bytes(obj);

        var json = Encoding.UTF8.GetString(bytes);

        var rebuilt = ServiceBusMessageSerializer.FromJsonString<TestObj>(json);
        rebuilt.ListChildren.Should().ContainSingle();
        rebuilt.EnumerableChildren.Should().ContainSingle();

        rebuilt.Should().BeEquivalentTo(obj);
    }

    private static TestObj GetCompleteTestObject()
    {
        return new TestObj
        {
            StringProp = "test",
            BoolProp = true,
            NullBoolProp = true,
            DateTimeProp = DateTime.Now.AddMinutes(100),
            NullDateTimeProp = DateTime.Now.AddMinutes(100),
            DateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
            NullDateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
            TimeSpanProp = TimeSpan.FromMinutes(-100),
            NullTimeSpanProp = TimeSpan.FromMinutes(-100),
            //DateOnlyProp = DateOnly.FromDateTime(DateTime.Now.AddHours(-9))
            IntProp = 78,
            NullIntProp = 78,
            ShortProp = 2,
            NullShortProp = 2,
            LongProp = int.MaxValue + 100L,
            NullLongProp = int.MaxValue + 100L,
            UIntProp = 3,
            NullUIntProp = 3,
            UShortProp = 5,
            NullUShortProp = 5,
            ULongProp = 10,
            NullULongProp = 10,
            DecimalProp = 199.01M,
            NullDecimalProp = 199.01M,
            FloatProp = 920.1F,
            NullFloatProp = 920.1F,
            DoubleProp = 3892.83339d,
            NullDoubleProp = 3892.83339d,
            Enum1 = TestEnum.Item2 | TestEnum.Item8,
            NullEnum1 = TestEnum.Item2 | TestEnum.Item8,
            ListChildren =
            [
                new TestObj
                {
                    StringProp = "test",
                    BoolProp = true,
                    NullBoolProp = true,
                    DateTimeProp = DateTime.Now.AddMinutes(100),
                    NullDateTimeProp = DateTime.Now.AddMinutes(100),
                    DateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
                    NullDateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
                    TimeSpanProp = TimeSpan.FromMinutes(-100),
                    NullTimeSpanProp = TimeSpan.FromMinutes(-100),
                    //DateOnlyProp = DateOnly.FromDateTime(DateTime.Now.AddHours(-9))
                    IntProp = 78,
                    NullIntProp = 78,
                    ShortProp = 2,
                    NullShortProp = 2,
                    LongProp = int.MaxValue + 100L,
                    NullLongProp = int.MaxValue + 100L,
                    UIntProp = 3,
                    NullUIntProp = 3,
                    UShortProp = 5,
                    NullUShortProp = 5,
                    ULongProp = 10,
                    NullULongProp = 10,
                    DecimalProp = 199.01M,
                    NullDecimalProp = 199.01M,
                    FloatProp = 920.1F,
                    NullFloatProp = 920.1F,
                    DoubleProp = 3892.83339d,
                    NullDoubleProp = 3892.83339d,
                    Enum1 = TestEnum.Item2 | TestEnum.Item16,
                    NullEnum1 = TestEnum.Item2 | TestEnum.Item16
                }
            ],
            EnumerableChildren =
            [
                new()
                {
                    StringProp = "test",
                    BoolProp = true,
                    NullBoolProp = true,
                    DateTimeProp = DateTime.Now.AddMinutes(100),
                    NullDateTimeProp = DateTime.Now.AddMinutes(100),
                    DateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
                    NullDateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
                    TimeSpanProp = TimeSpan.FromMinutes(-100),
                    NullTimeSpanProp = TimeSpan.FromMinutes(-100),
                    //DateOnlyProp = DateOnly.FromDateTime(DateTime.Now.AddHours(-9))
                    IntProp = 78,
                    NullIntProp = 78,
                    ShortProp = 2,
                    NullShortProp = 2,
                    LongProp = int.MaxValue + 100L,
                    NullLongProp = int.MaxValue + 100L,
                    UIntProp = 3,
                    NullUIntProp = 3,
                    UShortProp = 5,
                    NullUShortProp = 5,
                    ULongProp = 10,
                    NullULongProp = 10,
                    DecimalProp = 199.01M,
                    NullDecimalProp = 199.01M,
                    FloatProp = 920.1F,
                    NullFloatProp = 920.1F,
                    DoubleProp = 3892.83339d,
                    NullDoubleProp = 3892.83339d,
                    Enum1 = TestEnum.Item2 | TestEnum.Item16,
                    NullEnum1 = TestEnum.Item2 | TestEnum.Item16
                }
            ]
        };
    }

    private static TestObj GetCompleteTestObjectWithNulls()
    {
        return new TestObj
        {
            StringProp = "test",
            BoolProp = true,
            NullBoolProp = null,
            DateTimeProp = DateTime.Now.AddMinutes(100),
            NullDateTimeProp = null,
            DateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
            NullDateTimeOffsetProp = null,
            TimeSpanProp = TimeSpan.FromMinutes(-100),
            NullTimeSpanProp = null,
            //DateOnlyProp = DateOnly.FromDateTime(DateTime.Now.AddHours(-9))
            IntProp = 78,
            NullIntProp = null,
            ShortProp = 2,
            NullShortProp = null,
            LongProp = int.MaxValue + 100L,
            NullLongProp = null,
            UIntProp = 3,
            NullUIntProp = null,
            UShortProp = 5,
            NullUShortProp = null,
            ULongProp = 10,
            NullULongProp = null,
            DecimalProp = 199.01M,
            NullDecimalProp = null,
            FloatProp = 920.1F,
            NullFloatProp = null,
            DoubleProp = 3892.83339d,
            NullDoubleProp = null,
            Enum1 = TestEnum.Item2 | TestEnum.Item8,
            NullEnum1 = null,
            ListChildren =
            [
                new TestObj
                {
                    StringProp = "test",
                    BoolProp = true,
                    NullBoolProp = null,
                    DateTimeProp = DateTime.Now.AddMinutes(100),
                    NullDateTimeProp = null,
                    DateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
                    NullDateTimeOffsetProp = null,
                    TimeSpanProp = TimeSpan.FromMinutes(-100),
                    NullTimeSpanProp = null,
                    //DateOnlyProp = DateOnly.FromDateTime(DateTime.Now.AddHours(-9))
                    IntProp = 78,
                    NullIntProp = null,
                    ShortProp = 2,
                    NullShortProp = null,
                    LongProp = int.MaxValue + 100L,
                    NullLongProp = null,
                    UIntProp = 3,
                    NullUIntProp = null,
                    UShortProp = 5,
                    NullUShortProp = null,
                    ULongProp = 10,
                    NullULongProp = null,
                    DecimalProp = 199.01M,
                    NullDecimalProp = null,
                    FloatProp = 920.1F,
                    NullFloatProp = null,
                    DoubleProp = 3892.83339d,
                    NullDoubleProp = null,
                    Enum1 = TestEnum.Item2 | TestEnum.Item16,
                    NullEnum1 = null
                }
            ],
            EnumerableChildren =
            [
                new()
                {
                    StringProp = "test",
                    BoolProp = true,
                    NullBoolProp = null,
                    DateTimeProp = DateTime.Now.AddMinutes(100),
                    NullDateTimeProp = null,
                    DateTimeOffsetProp = DateTimeOffset.Now.AddMinutes(100),
                    NullDateTimeOffsetProp = null,
                    TimeSpanProp = TimeSpan.FromMinutes(-100),
                    NullTimeSpanProp = null,
                    //DateOnlyProp = DateOnly.FromDateTime(DateTime.Now.AddHours(-9))
                    IntProp = 78,
                    NullIntProp = null,
                    ShortProp = 2,
                    NullShortProp = null,
                    LongProp = int.MaxValue + 100L,
                    NullLongProp = null,
                    UIntProp = 3,
                    NullUIntProp = null,
                    UShortProp = 5,
                    NullUShortProp = null,
                    ULongProp = 10,
                    NullULongProp = null,
                    DecimalProp = 199.01M,
                    NullDecimalProp = null,
                    FloatProp = 920.1F,
                    NullFloatProp = null,
                    DoubleProp = 3892.83339d,
                    NullDoubleProp = null,
                    Enum1 = TestEnum.Item2 | TestEnum.Item16,
                    NullEnum1 = null
                }
            ]
        };
    }

    [Flags]
    private enum TestEnum
    {
        None = 0,
        Item1 = 1,
        Item2 = 2,
        Item4 = 4,
        Item8 = 8,
        Item16 = 16
    }

    private class TestObj
    {
        public string StringProp { get; set; }

        public bool BoolProp { get; set; }
        public bool? NullBoolProp { get; set; }

        public DateTime DateTimeProp { get; set; }
        public DateTime? NullDateTimeProp { get; set; }

        public DateTimeOffset DateTimeOffsetProp { get; set; }
        public DateTimeOffset? NullDateTimeOffsetProp { get; set; }

        public TimeSpan TimeSpanProp { get; set; }
        public TimeSpan? NullTimeSpanProp { get; set; }

        // DateOnly does not work yet
        //public DateOnly DateOnlyProp { get; set; }

        public int IntProp { get; set; }
        public int? NullIntProp { get; set; }

        public short ShortProp { get; set; }
        public short? NullShortProp { get; set; }

        public long LongProp { get; set; }
        public long? NullLongProp { get; set; }

        public uint UIntProp { get; set; }
        public uint? NullUIntProp { get; set; }

        public ushort UShortProp { get; set; }
        public ushort? NullUShortProp { get; set; }

        public ulong ULongProp { get; set; }
        public ulong? NullULongProp { get; set; }

        public decimal DecimalProp { get; set; }
        public decimal? NullDecimalProp { get; set; }

        public float FloatProp { get; set; }
        public float? NullFloatProp { get; set; }

        public double DoubleProp { get; set; }
        public double? NullDoubleProp { get; set; }

        public TestEnum Enum1 { get; set; }
        public TestEnum? NullEnum1 { get; set; }

        public IList<TestObj> ListChildren { get; set; }
        public IEnumerable<TestObj> EnumerableChildren { get; set; }
    }
}
