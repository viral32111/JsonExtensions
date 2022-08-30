using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace viral32111.JsonExtensions {
	public static class JsonExtensions {

		// Internally stores the file paths of each JSON object
		private static readonly Dictionary<JsonObject, string> objectFilePaths = new();

		// The character to separate names when referencing nested properties
		private static readonly char nestedPropertyDelimiter = '.';

		// Options that control how JSON seralization & deseralizaion happens
		public static readonly JsonSerializerOptions SerializerOptions = new() {

			// Keep property names as they are
			PropertyNamingPolicy = null,
			PropertyNameCaseInsensitive = false,

			// Ignore any human comments
			ReadCommentHandling = JsonCommentHandling.Skip,

			// Ignore minor human mistakes
			AllowTrailingCommas = true,

			// Make human editing easier
			WriteIndented = true

		};

		// Opens a JSON file
		public static JsonObject ReadFromFile( string filePath ) {

			// Open the file for reading
			using FileStream fileStream = File.Open( filePath, FileMode.Open, FileAccess.Read, FileShare.None );

			// Parse the file as JSON
			JsonObject? jsonObject = JsonSerializer.Deserialize<JsonObject>( fileStream, SerializerOptions );

			// Fail if the parsed JSON is invalid
			if ( jsonObject == null ) throw new JsonParseException( $"Failed to parse file '{filePath}' as JSON" );

			// Remember this object's file path
			objectFilePaths[ jsonObject ] = filePath;

			// Return the parsed JSON
			return jsonObject;

		}

		// Creates a JSON file
		public static JsonObject CreateNewFile( string filePath, JsonObject? jsonObject = null ) {

			// Default to an empty JSON object if a custom one was not provided
			jsonObject ??= new();

			// Save the JSON to the file
			jsonObject.SaveToFile( filePath );

			// Return the same (or new) JSON
			return jsonObject;

		}

		// Saves a JSON file
		public static void SaveToFile( this JsonObject jsonObject, string? filePath = null ) {

			// Use the remembered file path if one was not provided
			filePath ??= objectFilePaths[ jsonObject ];

			// Create (or overwrite) the file for writing
			using FileStream fileStream = File.Open( filePath, FileMode.Create, FileAccess.Write, FileShare.None );

			// Write the JSON to the file
			JsonSerializer.Serialize( fileStream, jsonObject, SerializerOptions );

		}

		// Set the value of a nested property
		public static void NestedSet( this JsonObject jsonObject, string propertyPath, JsonNode? newValue ) {

			// Separate the path into individual property names
			string[] propertyNames = propertyPath.Split( nestedPropertyDelimiter );

			// The JSON object to search, starts at the root
			JsonObject nextObject = jsonObject;

			// Loop through each property name...
			for ( int index = 0; index < propertyNames.Length; index++ ) {
				string propertyName = propertyNames[ index ];

				// Set the value of the property with this name if this is the last iteration
				if ( ( index + 1 ) == propertyNames.Length ) {
					nextObject[ propertyName ] = newValue;

					// If this is not the last iteration...
				} else {

					// Set the property with this name to an empty JSON object if it does not exist
					if ( !nextObject.ContainsKey( propertyName ) ) nextObject.Add( propertyName, new JsonObject() );

					// Get the value of the property with this name
					JsonNode? value = nextObject[ propertyName ];

					// Fail if the value is invalid
					if ( value == null ) throw new JsonPropertyNullException( $"Nested property '{propertyName}' in '{propertyPath}' is null, it cannot be used as the next object" );

					// Set this value as the next JSON object to search
					nextObject = value.AsObject();

				}

			}

		}

		// Gets the value of a nested property
		public static JsonNode? NestedGet( this JsonObject jsonObject, string propertyPath ) {

			// Separate the path into individual property names
			string[] propertyNames = propertyPath.Split( nestedPropertyDelimiter );

			// The JSON object to search, starts at the root
			JsonObject nextObject = jsonObject;

			// Loop through each property name...
			for ( int index = 0; index < propertyNames.Length; index++ ) {
				string propertyName = propertyNames[ index ];

				// Get the value of the property with this name
				if ( !nextObject.TryGetPropertyValue( propertyName, out JsonNode? value ) ) throw new JsonPropertyNotFoundException( $"Nested property '{propertyName}' in '{propertyPath}' does not exist" );

				// Return the value if this is the last iteration
				if ( ( index + 1 ) == propertyNames.Length ) {
					return value;

					// If this is not the last iteration...
				} else {

					// Fail if the value is invalid
					if ( value == null ) throw new JsonPropertyNullException( $"Nested property '{propertyName}' in '{propertyPath}' is null, it cannot be used as the next object" );

					// Set this value as the next JSON object to search
					nextObject = value.AsObject();

				}

			}

			// Fail if the loop never ran
			throw new JsonException( $"Could not find nested property '{propertyPath}'" );

		}

		// Gets the value of a nested property, as a certain type
		public static T NestedGet<T>( this JsonObject jsonObject, string propertyPath ) {

			// Get the value of the nested property
			JsonNode? value = jsonObject.NestedGet( propertyPath );

			// Fail if the value is invalid
			if ( value == null ) throw new JsonPropertyNullException( $"Nested property '{propertyPath}' is null, it cannot be of type {typeof( T )}" );

			// Return the value as a JSON object
			if ( typeof( T ) == typeof( JsonObject ) ) return ( T ) ( object ) value.AsObject();

			// Return the value as a JSON array
			else if ( typeof( T ) == typeof( JsonArray ) ) return ( T ) ( object ) value.AsArray();

			// Return the value as the desired type
			else return value.GetValue<T>();

		}

		// Convert a JSON array to an array of a certain type
		public static T[] AsArray<T>( this JsonNode array ) {

			// Create an empty list
			List<T> list = new();

			// Loop through each value in the JSON array...
			foreach ( JsonNode? value in array.AsArray() ) {

				// Fail if the value is invalid
				if ( value == null ) throw new JsonPropertyNullException( $"Value is null'" );

				// Add the value as the desired type to the list
				list.Add( value.GetValue<T>() );

			}

			// Return the list as an array
			return list.ToArray();

		}

	}

	// Exception for when parsing fails
	public class JsonParseException : Exception {
		public JsonParseException( string? message ) : base( message ) { }
		public JsonParseException( string? message, Exception? innerException ) : base( message, innerException ) { }
	}

	// Exception for when a property cannot be found
	public class JsonPropertyNotFoundException : Exception {
		public JsonPropertyNotFoundException( string? message ) : base( message ) { }
		public JsonPropertyNotFoundException( string? message, Exception? innerException ) : base( message, innerException ) { }
	}

	// Exception for when a property is not expected to be null
	public class JsonPropertyNullException : Exception {
		public JsonPropertyNullException( string? message ) : base( message ) { }
		public JsonPropertyNullException( string? message, Exception? innerException ) : base( message, innerException ) { }
	}

}
