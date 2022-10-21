using System.ComponentModel.DataAnnotations;

namespace SampleApp.Config;

public record SampleSettings
{
    public const string ConfigKey = "Sample";
    
    public string Name { get; set; }
    
    [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Age { get; set; }
    
    public string Address { get; set; }
    
    public List<string> Thoughts { get; set; }
    
    public Dictionary<string, string> Extensions { get; set; }
}