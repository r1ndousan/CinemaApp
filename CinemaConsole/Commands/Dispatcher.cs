using Microsoft.Extensions.Logging;

namespace CinemaConsole.Commands
{
    public class CommandDispatcher
    {
        private readonly ILogger<CommandDispatcher> _logger;

        public CommandDispatcher(ILogger<CommandDispatcher> logger)
        {
            _logger = logger;
        }

        public async Task DispatchAsync(ICommand command)
        {
            _logger.LogInformation("Executing command {CommandType}", command.GetType().Name);
            await command.ExecuteAsync();
        }
    }
}
