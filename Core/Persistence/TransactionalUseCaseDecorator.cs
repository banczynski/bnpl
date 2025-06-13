using Core.Persistence.Interfaces;

namespace Core.Persistence
{
    public class TransactionalUseCaseDecorator<TRequest, TResponse>(IUseCase<TRequest, TResponse> decorated, IUnitOfWork unitOfWork) : IUseCase<TRequest, TResponse>
        where TResponse : Models.Interfaces.IResult 
    {
        public async Task<TResponse> ExecuteAsync(TRequest request)
        {
            try
            {
                unitOfWork.Begin();
                var result = await decorated.ExecuteAsync(request);

                if (result.IsSuccess)
                {
                    unitOfWork.Commit();
                }
                else
                {
                    unitOfWork.Rollback();
                }
                return result;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}