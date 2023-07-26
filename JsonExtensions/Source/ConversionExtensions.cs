using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace viral32111.JsonExtensions;

/// <summary>
/// Extension methods for conversion of JSON types.
/// </summary>
public static class ConversionExtensions {

	/// <summary>Converts a JSON array to a typed array.</summary>
	/// <typeparam name="T">The type of the array. Can be nullable.</typeparam>
	/// <param name="jsonArray">The JSON array.</param>
	/// <returns>The JSON array as a typed array.</returns>
	/// <exception cref="JsonPropertyNullException">Thrown if a value in the JSON array is null.</exception>
	public static T[] AsArray<T>( this JsonNode jsonArray ) {

		// Create an empty list of the desired type
		List<T> list = new();

		// Add each value in the JSON array to the list, failing if it's null
		foreach ( JsonNode? value in jsonArray.AsArray() ) {
			if ( value == null ) throw new JsonPropertyNullException( $"Value '{ value }' within the JSON array is null" );
			list.Add( value.GetValue<T>() );
		}

		// Convert the list to an array then return it
		return list.ToArray();

	}

	/// <summary>Copies a JSON node. Useful for reusing it in another JSON object.</summary>
	/// <param name="jsonNode">Any JSON type.</param>
	/// <returns>The copy of the given JSON node.</returns>
	public static JsonNode? Clone( this JsonNode? node ) => JsonSerializer.Deserialize<JsonNode>( node ); // https://stackoverflow.com/a/71590703

}
