namespace OnlineCamera.Core.Value.Api
{
    public class ParametersCollectionKeyValue
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public ParametersCollectionKeyValue(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
