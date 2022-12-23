namespace BotwRegistryToolkit
{
    public static class Meta
    {
        public static string Name { get; set; } = "Botw Registry Toolkit";
        public static string? Version { get; set; } = typeof(Meta).Assembly.GetName().Version?.ToString(3);
        public static string Title { get; set; } = $"{Name} - v{Version}";
        public static string BaseUrl { get; } = "https://raw.githubusercontent.com/ArchLeaders/BotwRegistryToolkit/master";
    }
}
