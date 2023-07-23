using System;
using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace viral32111.JsonExtensions;

public static class NestedExtensions {

	/*
	/// <summary>
	/// Sets the value of a nested JSON object property. If the property or parent JSON object does not exist, it will be created.
	/// <param name="propertyPath">The nested path to the property, separated by <paramref name="propertyDelimiter"/>.</param>
	/// <param name="newValue">The new value of the property.</param>
	/// <param name="propertyDelimiter">The character used to separate nested property names.</param>
	/// </summary>
	public static void NestedSet( this JsonObject jsonObject, string propertyPath, JsonNode? newValue, char propertyDelimiter = Defaults.NestedPropertyDelimiter ) {

		// Separate the path into individual property names
		string[] propertyNames = propertyPath.Split( propertyDelimiter, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries );

		// Find the object to set the value of, creating it if it does not exist
		JsonObject targetObject = propertyNames
			.Take( propertyNames.Length - 1 )
			.Aggregate( jsonObject, ( currentObject, currentPropertyName ) => {
				if ( !currentObject.ContainsKey( currentPropertyName ) ) currentObject.Add( currentPropertyName, new JsonObject() );

				return currentObject[ currentPropertyName ]?.AsObject() ?? throw new JsonPropertyNullException( $"Nested JSON object property '{ currentPropertyName }' in '{ propertyPath }' is null, it cannot be used as the next object" );
			} );

		// Set the value of the property in the found object
		targetObject[ propertyNames.Last() ] = newValue;

	}
	*/

	// Sets the value of a nested property
	public static void NestedSet( this JsonObject jsonObject, string propertyPath, JsonNode? newValue ) {

		// Separate the path into individual property names
		string[] propertyNames = propertyPath.Split( Defaults.NestedPropertyDelimiter );

		// The JSON object to search, starts at the root
		JsonObject nextObject = jsonObject;

		// Loop through each property name...
		for ( int index = 0; index < propertyNames.Length; index++ ) {
			string propertyName = propertyNames[ index ];

			// Set the value of the property with this name if this is the last iteration
			if ( ( index + 1 ) == propertyNames.Length ) nextObject[ propertyName ] = newValue;

			// If this is not the last iteration...
			else {

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
	public static JsonNode? NestedGet( this JsonObject jsonObject, string propertyPath, char nestedPropertyDelimiter = Defaults.NestedPropertyDelimiter ) {

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
			if ( ( index + 1 ) == propertyNames.Length ) return value;

			// If this is not the last iteration...
			else {

				// Fail if the value is invalid
				if ( value == null ) throw new JsonPropertyNullException( $"Nested property '{propertyName}' in '{propertyPath}' is null, it cannot be used as the next object" );

				// Set this value as the next JSON object to search
				nextObject = value.AsObject();

			}

		}

		// Fail if the loop never ran
		throw new JsonPropertyNotFoundException( $"Could not find nested property '{propertyPath}'" );

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

	// Checks if a nested property exists
	public static bool NestedHas( this JsonObject jsonObject, string propertyPath ) {

		// Separate the path into individual property names
		string[] propertyNames = propertyPath.Split( Defaults.NestedPropertyDelimiter );

		// The JSON object to search, starts at the root
		JsonObject nextObject = jsonObject;

		// Loop through each property name...
		for ( int index = 0; index < propertyNames.Length; index++ ) {
			string propertyName = propertyNames[ index ];

			// Fail if we cannot get the property
			if ( !nextObject.TryGetPropertyValue( propertyName, out JsonNode? value ) ) return false;

			// If this is not the last iteration...
			if ( ( index + 1 ) < propertyNames.Length ) {

				// Fail if the value is invalid
				if ( value == null ) throw new JsonPropertyNullException( $"Nested property '{propertyName}' in '{propertyPath}' is null, it cannot be used as the next object" );

				// Set this value as the next JSON object to search
				nextObject = value.AsObject();

			}

		}

		// Success if we got here
		return true;

	}

	// Gets a list of nested property paths in the JSON object
	public static string[] NestedList( this JsonObject jsonObject ) {
		return ListNestedProperties( jsonObject ).ToArray();
	}

	// Recursively creates a list of nested property paths in the JSON object
	private static List<string> ListNestedProperties( JsonObject jsonObject, string currentPath = "", List<string>? propertyPaths = null ) {

		// Create an empty list to hold the property paths if it is not set
		propertyPaths ??= new();

		// Loop through each valid property in the object...
		foreach ( KeyValuePair<string, JsonNode?> property in jsonObject ) {
			
			// Create the path to this property by using the last call's path and this property name
			string propertyPath = currentPath + property.Key;

			// If the value is null then it must be a value so add it to the list
			if ( property.Value == null ) {
				propertyPaths.Add( propertyPath );
				continue;
			}

			// Attempt to repeat this call with the value as a JSON object, otherwise add it to the list as it must be a value
			try {
				ListNestedProperties( property.Value.AsObject(), propertyPath + Defaults.NestedPropertyDelimiter, propertyPaths );
			} catch ( InvalidOperationException ) {
				propertyPaths.Add( propertyPath );
			}

		}

		// Return the list
		return propertyPaths;

	}

}
