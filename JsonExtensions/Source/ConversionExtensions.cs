using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace viral32111.JsonExtensions;

public static class ConversionExtensions {

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

	// Copies a JSON node so it can be set in another JSON object
	public static JsonNode? Clone( this JsonNode? node ) {
		return JsonSerializer.Deserialize<JsonNode>( node ); // https://stackoverflow.com/a/71590703
	}

}
