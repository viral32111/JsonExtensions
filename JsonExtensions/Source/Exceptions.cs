using System;

namespace viral32111.JsonExtensions;

/// <summary>
/// Thrown when parsing JSON fails.
/// </summary>
public class JsonParseException : Exception {
	public JsonParseException( string? message ) : base( message ) { }
	public JsonParseException( string? message, Exception? innerException ) : base( message, innerException ) { }
}

/// <summary>
/// Thrown when a JSON property name cannot be found.
/// </summary>
public class JsonPropertyNotFoundException : Exception {
	public JsonPropertyNotFoundException( string? message ) : base( message ) { }
	public JsonPropertyNotFoundException( string? message, Exception? innerException ) : base( message, innerException ) { }
}

/// <summary>
/// Thrown when a JSON property value is not expected to be null.
/// </summary>
public class JsonPropertyNullException : Exception {
	public JsonPropertyNullException( string? message ) : base( message ) { }
	public JsonPropertyNullException( string? message, Exception? innerException ) : base( message, innerException ) { }
}
