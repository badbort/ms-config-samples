using Microsoft.Extensions.Options;

namespace SampleApp.Config;

public class ConfigureThoughts : IConfigureOptions<SampleSettings>
{
    public void Configure(SampleSettings options)
    {
        if(options.Thoughts?.Count > 0)
            return;

        options.Thoughts ??= new();
        options.Thoughts.Add("I want donuts");
    }
}

public class PostConfigureThoughts : IPostConfigureOptions<SampleSettings>
{
    public void PostConfigure(string name, SampleSettings options)
    {
        options.Thoughts ??= new();
        options.Thoughts.Add("I spend too much money on coffee");
    }
}