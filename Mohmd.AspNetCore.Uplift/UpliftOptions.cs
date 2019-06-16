namespace Mohmd.AspNetCore.Uplift
{
    public class UpliftOptions
    {
        public bool UseDefaultTempPath { get; set; } = true;

        public string CustomTempPath { get; set; } = "App_Data/TemporaryFiles";
    }
}
