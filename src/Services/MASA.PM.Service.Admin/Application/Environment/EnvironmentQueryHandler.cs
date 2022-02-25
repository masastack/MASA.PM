namespace MASA.PM.Service.Admin.Application.Environment
{
    public class EnvironmentQueryHandler
    {
        private readonly IEnvironmentRepository _environmentRepository;

        public EnvironmentQueryHandler(IEnvironmentRepository environmentRepository)
        {
            _environmentRepository = environmentRepository;
        }

        [EventHandler]
        public async Task EnvironmentHandleAsync(EnvironmentQuery query)
        {
            query.Result = await _environmentRepository.GetAsync(query.EnvironmentId);
        }

        [EventHandler]
        public async Task EnvironmentListHandleAsync(EnvironmentsQuery query)
        {
            query.Result = await _environmentRepository.GetListAsync();
        }
    }
}
