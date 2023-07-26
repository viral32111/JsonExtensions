using System;
using System.Linq;
using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace viral32111.JsonExtensions;

/// <summary>
/// Extension methods for nested JSON object properties.
/// </summary>
public static class NestedExtensions {

	/// <summary>Sets the value of a nested JSON object property. If the property or parent JSON object does not exist, it will be created.</summary>
	/// <param name="jsonObject">The JSON object.</param>
	/// <param name="propertyPath">The nested path to the property, separated by <paramref name="propertyDelimiter"/>.</param>
	/// <param name="newValue">The new value of the property.</param>
	/// <param name="propertyDelimiter">The character used to separate nested property names.</param>
	/// <exception cref="ArgumentException">Thrown if the nested property path is null or whitespace.</exception>
	/// <exception cref="JsonPropertyNullException">Thrown if one of the nested parent JSON object properties is null.</exception>
	public static void NestedSet( this JsonObject jsonObject, string propertyPath, JsonNode? newValue, char propertyDelimiter = Defaults.NestedPropertyDelimiter ) {
		if ( string.IsNullOrWhiteSpace( propertyPath ) ) throw new ArgumentException( "Nested property path cannot be null or whitespace", nameof( propertyPath ) );

		// Separate the path into individual property names
		string[] propertyNames = propertyPath.Split( propertyDelimiter, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries );

		// Find the object to set the property in...
		JsonObject targetObject = propertyNames
			.Take( propertyNames.Length - 1 ) // No need to include the last property name as we are using it as the index below
			.Aggregate( jsonObject, ( currentObject, currentPropertyName ) => {
				if ( !currentObject.ContainsKey( currentPropertyName ) ) currentObject.Add( currentPropertyName, new JsonObject() );

				return currentObject[ currentPropertyName ]?.AsObject() ?? throw new JsonPropertyNullException( $"Nested JSON object property '{ currentPropertyName }' in '{ propertyPath }' is null, it cannot be used as the next object" );
			} );

		// Set the value of the property in the found object
		targetObject[ propertyNames.Last() ] = newValue;

	}

	/// <summary>Gets the value of a nested JSON object property.</summary>
	/// <param name="jsonObject">The JSON object.</param>
	/// <param name="propertyPath">The nested path to the property, separated by <paramref name="propertyDelimiter"/>.</param>
	/// <param name="propertyDelimiter">The character used to separate nested property names.</param>
	/// <returns>The value of the nested JSON object property.</returns>
	/// <exception cref="ArgumentException">Thrown if the nested property path is null or whitespace.</exception>
	/// <exception cref="JsonPropertyNullException">Thrown if one of the nested parent JSON object properties is null.</exception>
	/// <exception cref="JsonPropertyNotFoundException">Thrown if one of the nested JSON object properties does not exist.</exception>
	public static JsonNode? NestedGet( this JsonObject jsonObject, string propertyPath, char propertyDelimiter = Defaults.NestedPropertyDelimiter ) {
		if ( string.IsNullOrWhiteSpace( propertyPath ) ) throw new ArgumentException( "Nested property path cannot be null or whitespace", nameof( propertyPath ) );

		// Separate the path into individual property names
		string[] propertyNames = propertyPath.Split( propertyDelimiter, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries );

		// Find the object to get the property in...
		JsonObject targetObject = propertyNames
			.Take( propertyNames.Length - 1 ) // No need to include the last property name as we are using it as the index below
			.Aggregate( jsonObject, ( currentObject, currentPropertyName ) => {
				if ( !currentObject.TryGetPropertyValue( currentPropertyName, out JsonNode? currentPropertyValue ) ) throw new JsonPropertyNotFoundException( $"Nested JSON object property '{ currentPropertyName }' in '{ propertyPath }' does not exist, it cannot be used as the next object" );
				return currentObject[ currentPropertyName ]?.AsObject() ?? throw new JsonPropertyNullException( $"Nested JSON object property '{ currentPropertyName }' in '{ propertyPath }' is null, it cannot be used as the next object" );
			} );

		// Return the value of the property in the found object, or fail
		if ( !targetObject.TryGetPropertyValue( propertyNames.Last(), out JsonNode? propertyValue ) ) throw new JsonPropertyNotFoundException( $"Nested JSON object property '{ propertyNames.Last() }' from '{ propertyPath }' in '{ jsonObject }' does not exist" );
		return propertyValue;

	}

	/// <summary>Gets the value of a nested JSON object property, casted to the given type.</summary>
	/// <typeparam name="T">The type to cast the property value to. Can be nullable.</typeparam>
	/// <param name="jsonObject">The JSON object.</param>
	/// <param name="propertyPath">The nested path to the property, separated by <paramref name="propertyDelimiter"/>.</param>
	/// <param name="propertyDelimiter">The character used to separate nested property names.</param>
	/// <returns>The value of the nested JSON object property, as the given type.</returns>
	/// <exception cref="ArgumentException">Thrown if the nested property path is null or whitespace.</exception>
	/// <exception cref="JsonPropertyNullException">Thrown if the property cannot be casted because it is null.</exception>
	public static T NestedGet<T>( this JsonObject jsonObject, string propertyPath, char propertyDelimiter = Defaults.NestedPropertyDelimiter ) {
		if ( string.IsNullOrWhiteSpace( propertyPath ) ) throw new ArgumentException( "Nested property path cannot be null or whitespace", nameof( propertyPath ) );

		// Get the value of the property, or fail if it does not exist
		JsonNode propertyValue = jsonObject.NestedGet( propertyPath ) ?? throw new JsonPropertyNullException( $"Nested JSON object property '{ propertyPath }' is null, it cannot be casted to type '{ typeof( T ) }'" );

		// Return as JSON object/array, or generic - Switch statement does not work for typeof()!
		if ( typeof( T ) == typeof( JsonObject ) ) return ( T ) ( object ) propertyValue.AsObject();
		else if ( typeof( T ) == typeof( JsonArray ) ) return ( T ) ( object ) propertyValue.AsArray();
		else return propertyValue.GetValue<T>();

	}

	/// <summary>Checks if a nested JSON object property exists.</summary>
	/// <param name="jsonObject">The JSON object.</param>
	/// <param name="propertyPath">The nested path to the property, separated by <paramref name="propertyDelimiter"/>.</param>
	/// <param name="propertyDelimiter">The character used to separate nested property names.</param>
	/// <returns>If the given property exists.</returns>
	/// <exception cref="ArgumentException">Thrown if the nested property path is null or whitespace.</exception>
	/// <exception cref="JsonPropertyNullException">Thrown if the property cannot be casted because it is null.</exception>
	public static bool NestedHas( this JsonObject jsonObject, string propertyPath, char propertyDelimiter = Defaults.NestedPropertyDelimiter ) {
		if ( string.IsNullOrWhiteSpace( propertyPath ) ) throw new ArgumentException( "Nested property path cannot be null or whitespace", nameof( propertyPath ) );

		// Separate the path into individual property names
		string[] propertyNames = propertyPath.Split( propertyDelimiter, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries );

		// Find the object to check the property in...
		JsonObject targetObject = jsonObject;
		foreach ( string propertyName in propertyNames.Take( propertyNames.Length - 1 ) ) {
			if ( !targetObject.ContainsKey( propertyName ) ) return false; // Short-circuit return
			targetObject = targetObject[ propertyName ]?.AsObject() ?? throw new JsonPropertyNullException( $"Nested JSON object property '{ propertyName }' in '{ propertyPath }' is null, it cannot be used as the next object" );
		}

		// Return if the property exists in the found object
		return targetObject.ContainsKey( propertyNames.Last() );

	}

	/// <summary>Gets a list of nested property paths within the JSON object .</summary>
	/// <param name="jsonObject">The JSON object.</param>
	/// <param name="propertyDelimiter">The character used to separate nested property names.</param>
	/// <returns>An array of nested property paths.</returns>
	public static string[] NestedList( this JsonObject jsonObject, char propertyDelimiter = Defaults.NestedPropertyDelimiter, string currentPath = "" ) {

		// Create an empty list for the property paths
		List<string> propertyPaths = new();

		// Loop through each property in the JSON object and create its path
		foreach ( KeyValuePair<string, JsonNode?> property in jsonObject ) {
			string propertyPath = string.IsNullOrWhiteSpace( currentPath ) ? property.Key : currentPath + propertyDelimiter + property.Key;

			// Recurse if the property is an object, otherwise add the path to the list
			try {
				JsonObject? nestedObject = property.Value?.AsObject();
				if ( nestedObject != null ) propertyPaths.AddRange( nestedObject.NestedList( propertyDelimiter, propertyPath ) );
				else propertyPaths.Add( propertyPath );
			} catch ( InvalidOperationException ) {
				propertyPaths.Add( propertyPath );
			}
		}

		// Convert the list to an array then return it
		return propertyPaths.ToArray();

	}

}
