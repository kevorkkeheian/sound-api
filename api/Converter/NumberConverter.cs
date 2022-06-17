
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

namespace Application.Converter;

public class NumberConverter : IPropertyConverter
{
	public object FromEntry(DynamoDBEntry entry)
	{
		Primitive primitive = entry as Primitive;
		// if (primitive == null) return String;

        if(entry == null) return new Primitive { Value = null };


		if (primitive.Type != DynamoDBEntryType.String)
		{
			throw new InvalidCastException(string.Format("Address cannot be converted as its type is {0} with a value of {1}"
				, primitive.Type, primitive.Value));
		}

        Console.WriteLine($"-----------------1---{entry}-----------------");

		string json = primitive.AsString();
        
		return JsonConvert.DeserializeObject<string>(json);
	}


	public DynamoDBEntry ToEntry(object value)
	{

		// Address address = value as Address;
		// if (address == null) return null;
        int loudness = (int)value;
        Console.WriteLine($"-----------------2---{loudness}-----------------");
		string json = JsonConvert.SerializeObject(loudness);
		return new Primitive(json);
	}
}