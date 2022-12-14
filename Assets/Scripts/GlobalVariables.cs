using System.Collections.Generic;

/// <summary> 
/// A simple static class to get and set globally accessible variables through a key-value approach.
/// </summary>
/// <remarks>
/// <para>Uses a key-value approach (dictionary) for storing and modifying variables.</para>
/// <para>It also uses a lock to ensure consistency between the threads.</para>
/// </remarks>
/// <source>https://stackoverflow.com/questions/42393259/load-scene-with-param-variable-unity</source>

public static class GlobalVariables
{
    private static readonly object LockObject = new();

    /// <summary>
    /// The underlying key-value storage (dictionary).
    /// </summary>
    /// <value>Gets the underlying variables dictionary</value>
    private static Dictionary<GlobalVariablesKey, object> VariablesDictionary { get; set; } = new();
    
    public static bool HasKey(GlobalVariablesKey key)
    {
        return VariablesDictionary.ContainsKey(key);
    }
    
    /// <summary>
    /// Gets a variable and casts it to the provided type argument.
    /// </summary>
    /// <typeparam name="T">The type of the variable</typeparam>
    /// <param name="key">The variable key</param>
    /// <returns>The casted variable value</returns>
    public static T Get<T>(GlobalVariablesKey key)
    {
        return (T) VariablesDictionary[key];
    }

    /// <summary>
    /// Sets the variable, the existing value gets overridden.
    /// </summary>
    /// <remarks>It uses a lock under the hood to ensure consistensy between threads</remarks>
    /// <param name="key">The variable name/key</param>
    /// <param name="value">The variable value</param>
    public static void Set(GlobalVariablesKey key, object value)
    {
        lock (LockObject)
        {
            VariablesDictionary[key] = value;
        }
    }
}

public enum GlobalVariablesKey { ArgosFile }