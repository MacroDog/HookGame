using System.Collections.Generic;

public static class DictionaryExt
{
	public static V Get<K, V>(this IDictionary<K, V> dict, K key)
	{
		V value;
		dict.TryGetValue(key, out value);
		return value;
	}

	public static V Opt<K, V>(this IDictionary<K, V> dict, K key) where V : new()
	{
		V value;
		dict.TryGetValue(key, out value);
		if (value == null) {
			dict[key] = value = new V();
		}
		return value;
	}
}
