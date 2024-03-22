public static class SaveKeyCreator
{
    public static string CreateFullKey(string uniqueIdentifier, string keyToSave)
    {
        return uniqueIdentifier + "_" + keyToSave;
    }
}
