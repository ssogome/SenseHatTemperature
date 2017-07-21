using SenseHatTelemeter.AzureHelper;

namespace SenseHatTelemeter.Helpers
{
    public static class CommandHelper
    {
        public static string UpdateTelemetryStatusCommandName { get; } = "UpdateTelemetryStatus";
        private static string updateTelemetryStatusCommandParameterName = "IsOn";

        public static Command CreateUpdateTelemetryStatusCommand()
        {
            return new Command()
            {
                Name = UpdateTelemetryStatusCommandName,
                Parameters = new CommandParameter[] {
                    new CommandParameter()
                    {
                        Name = updateTelemetryStatusCommandParameterName,
                        Type = "Boolean"
                    }
                }
            };
        }
    }
}
