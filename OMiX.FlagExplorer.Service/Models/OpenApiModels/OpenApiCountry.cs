namespace OMiX.FlagExplorer.Service.Models.OpenApiCountry
{
    public class OpenApiCountry
    {
        public OpenApiName Name { get; set; }
        public OpenApilags Flags { get; set; }
        public int Population { get; set; }
        public List<string> Capital { get; set; }
    }

    public class OpenApiName
    {
        public string Common { get; set; }
        public string Official { get; set; }
    }

    public class OpenApilags
    {
        public string Png { get; set; }
        public string Svg { get; set; }
    }
}
