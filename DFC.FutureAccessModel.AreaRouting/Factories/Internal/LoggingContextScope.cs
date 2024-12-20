using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    /// the logging scope
    /// </summary>
    internal sealed class LoggingContextScope :
        IScopeLoggingContext
    {
        /// <summary>
        /// the microsoft logger
        /// </summary>
        public ILogger Log { get; }

        /// <summary>
        /// a 'correlation' id
        /// not sure of the benefit here, but it seems to be needed for some calls
        /// </summary>
        public Guid CorrelationID { get; }

        /// <summary>
        /// initialises an instance of <see cref="LoggingContextScope"/>
        /// </summary>
        /// <param name="log">the microsoft log</param>
        /// <param name="helper">the DFC logger helper</param>
        /// <param name="initialisingRoutine">the calling routine</param>
        public LoggingContextScope(ILogger log, string initialisingRoutine)
        {
            Log = log;

            CorrelationID = Guid.NewGuid();

            Information($"commencing scoped logging from: '{initialisingRoutine}' for correlation id: {CorrelationID}").ConfigureAwait(false);
        }

        /// <summary>
        /// (log) enter method
        /// </summary>
        /// <param name="theMethod">the method (name)</param>
        /// <returns>the currently running task</returns>
        public async Task EnterMethod([CallerMemberName] string theMethod = null) =>
            await Information($"entering method: '{theMethod}'");

        /// <summary>
        /// (log) exit method
        /// </summary>
        /// <param name="theMethod">the method (name)</param>
        /// <returns>the currently running task</returns>
        public async Task ExitMethod([CallerMemberName] string theMethod = null) =>
            await Information($"exiting method: '{theMethod}'");

        /// <summary>
        /// (log) information
        /// </summary>
        /// <param name="theMessage">the message</param>
        /// <returns>the currently running task</returns>
        public async Task Information(string theMessage) =>
            await Task.Run(() => Log.LogInformation("CorrelationId: {CorrelationId} Message: {TheMessage}", CorrelationID, theMessage));

        /// <summary>
        /// (log) exception detail
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <returns>the currently running task</returns>
        public async Task ExceptionDetail(Exception theException) =>
            await Task.Run(() => Log.LogError(theException, "CorrelationId: {CorrelationId} Message: {TheMessage} Exception: {TheException}", CorrelationID, string.Empty, theException));

        /// <summary>
        /// dispose, as this is used in a 'using' clause disposal should be guaranteed
        /// </summary>
        public void Dispose()
        {
            Log.LogInformation("CorrelationId: {CorrelationId} Message: request completed", CorrelationID);
        }
    }
}
