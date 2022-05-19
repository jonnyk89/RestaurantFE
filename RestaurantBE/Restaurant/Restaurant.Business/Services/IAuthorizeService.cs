
using Restaurant.Business.Requests;
using Restaurant.Business.Responses;

namespace Restaurant.Business.Services
{
    public interface IAuthorizeService
    {
        Task<AuthorizeResponse> Authorize(AuthorizeRequest request);
    }
}
