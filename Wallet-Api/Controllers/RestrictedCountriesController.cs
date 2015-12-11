using System.Collections.Generic;
using System.Web.Http;
using Wallet_Api.Models;

namespace Wallet_Api.Controllers
{
    public class RestrictedCountriesController : ApiController
    {

        private static IEnumerable<string> Countries()
        {
            yield return "USA";
            yield return "Iraq";
            yield return "Iran";
            yield return "Afghanistan";
            yield return "Pakistan";
            yield return "Sudan";
            yield return "Yemen";
            yield return "Ivory coast";
            yield return "Liberia";
            yield return "North Korea";
            yield return "Burma / MYANMAR";
            yield return "Syria";
            yield return "Cuba";
        }

        public ResponseModel<RestrictedCountriesResponseModel> Get()
        {
            return ResponseModel<RestrictedCountriesResponseModel>
                .CreateOk(new RestrictedCountriesResponseModel
            {
                RestrictedCountires = Countries()
            });
        }

    }
}
