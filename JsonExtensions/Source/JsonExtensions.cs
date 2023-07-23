using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;

namespace viral32111.JsonExtensions;

public static class JsonExtensions {

	// Retains the filesystem paths of each JSON object read from a file
	private static readonly Dictionary<JsonObject, string> objectFilePaths = new();

	// Opens a JSON file
	public static JsonObject ReadFromFile( string filePath ) {

		// Open the file for reading
		using FileStream fileStream = File.Open( filePath, FileMode.Open, FileAccess.Read, FileShare.None );

		// Parse the file as JSON
		JsonObject? jsonObject = JsonSerializer.Deserialize<JsonObject>( fileStream, Defaults.SerializerOptions );

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

		// Remember this object's file path
		objectFilePaths[ jsonObject ] = filePath;

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
		JsonSerializer.Serialize( fileStream, jsonObject, Defaults.SerializerOptions );

	}

}
