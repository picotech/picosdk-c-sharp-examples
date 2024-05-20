using System;
using System.Runtime.Serialization;
using PL1000Imports;

namespace PL1000CSConsole
{
  /// <summary>
  /// Represents an exception that is thrown when a PicoErrorCode occurs.
  /// </summary>
  [Serializable]
  internal class PicoErrorCodeException : Exception
  {
    public PicoErrorCodeException(Imports.PicoErrorCode errorCode) : this($"PicoErrorCode {errorCode}")
    {
    }

    public PicoErrorCodeException(string message) : base(message)
    {
    }

    public PicoErrorCodeException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PicoErrorCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}