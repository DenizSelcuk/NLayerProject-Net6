using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using System.Linq;

namespace NLayer.API.Filters
{
    //ActionFilterAttribute miras alarak yeni bir filter oluşturduk
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context) //mevcutdaki OnActionExecuting metodunu override ettik. 
        {
            if(!context.ModelState.IsValid) //Context geçerli değilse çalışır.
            {
                var errors = context.ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage).ToList(); //Context error mesajları listelenir.
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors)); //CustomResponseDto oluşturularak context resulta atanır.
            } 
        }
    }
}
