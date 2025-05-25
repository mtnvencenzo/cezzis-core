namespace Cezzi.Azure.ServiceBus.Tests;

using FluentAssertions;
using System;
using Xunit;

public class ServiceBusDataLossExceptionTests
{
    [Fact]
    public void datalossex___includes_string_data_object()
    {
        var ex = new ServiceBusDataLossException<string>("Test exception", new Exception("Inne Ex"), "test-string");

        ex.ToString().Should().EndWith("\"test-string\"");
    }

    [Fact]
    public void datalossex___includes_string_data_object_no_message()
    {
        var ex = new ServiceBusDataLossException<string>(new Exception("Inne Ex"), "test-string");

        ex.ToString().Should().EndWith("\"test-string\"");
    }

    [Fact]
    public void datalossex___includes_data_object()
    {
        var guid = Guid.Parse("d8ea7a4f-19af-474b-908e-c9c68f79fa78");

        var ex = new ServiceBusDataLossException<TestDataObject>("Test exception", new Exception("Inne Ex"), new TestDataObject
        {
            JobGuid = guid,
            JobId = 1,
            JobType = 162.00M,
            TestProp = "My-Test-Prop"
        });

        ex.ToString().Should().EndWith("{\"testProp\":\"My-Test-Prop\",\"jobId\":1,\"jobType\":162.00,\"jobGuid\":\"d8ea7a4f-19af-474b-908e-c9c68f79fa78\"}");
    }

    [Fact]
    public void datalossex___is_ok_with_null_object()
    {
        var ex = new ServiceBusDataLossException<TestDataObject>("Test exception", new Exception("Inne Ex"), null);

        ex.ToString().Should().EndWith($"::Data Object::{Environment.NewLine}");
    }
}

public class TestDataObject
{
    public string TestProp { get; set; }

    /// <summary>Gets or sets the job identifier.</summary>
    /// <value>The job identifier.</value>
    public virtual long JobId { get; set; }

    /// <summary>Gets or sets the type of the job.</summary>
    /// <value>The type of the job.</value>
    public virtual decimal JobType { get; set; }

    /// <summary>Gets or sets the job unique identifier.</summary>
    /// <value>The job unique identifier.</value>
    public virtual Guid JobGuid { get; set; }
}