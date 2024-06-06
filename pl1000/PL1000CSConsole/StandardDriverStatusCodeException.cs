using System;
using System.Runtime.Serialization;
using DriverImports;

namespace PL1000CSConsole
{
  /// <summary>
  /// Represents an exception that is thrown when a <see cref="StandardDriverStatusCode"/> occurs.
  /// </summary>
  [Serializable]
  internal class StandardDriverStatusCodeException : Exception
  {
    public StandardDriverStatusCodeException(StandardDriverStatusCode errorCode) : this($"StandardDriverStatusCode {errorCode}")
    {
    }

    public StandardDriverStatusCodeException(string message) : base(message)
    {
    }

    public StandardDriverStatusCodeException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected StandardDriverStatusCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}