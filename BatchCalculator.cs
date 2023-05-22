void Main()
{
	int batchSize = 3;
	List<string> ids = new List<string>
	{
		"12",
		"76",
		"-909",
		"44",
		"545",
		"991",
		"4545",
		"-90834",
		"2",
		"943551"
	};
	
	var numberOfBatches = (ids.Count / batchSize) + (ids.Count % batchSize == 0 ? 0 : 1);
	for (int batch = 1; batch <= numberOfBatches; batch++){
		var batchedData = ids.GetNextBatch(batch, batchSize, numberOfBatches);
		batchedData.ForEach(d => Console.Write($"{d},"));
		Console.WriteLine();
	}

	/* Output	
		12,76,-909,
		44,545,991,
		4545,-90834,2,
		943551,
	*/
}

// You can define other methods, fields, classes and namespaces here

public static class ListExtensions
{
	public static List<T> GetNextBatch<T>(this List<T> list, int batch,
		int batchSize,
		int numberOfBatches
	) where T : class 	 
	{
		var startIndex = (batch - 1) * batchSize;
		if (batch < numberOfBatches)
			return list.GetRange(startIndex, batchSize);

		return list.GetRange(startIndex, list.Count - startIndex);
	}
}
