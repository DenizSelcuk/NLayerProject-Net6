using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        public readonly IService<T> _service; //Entity olup olmadığını service'i kullanarak kontrol edeceğiz. Repository'de kullanıbilirdi.

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault();
            if(idValue==null) //Aslında bu blok hiç çalışmaz.
            {
                await next.Invoke(); // eğer id değeri null ise geri program.cs de geri döner metodun controllerın içine girmez.
                return; 
            }
            var id = (int)idValue; //Eğer id var ise idValue^yu id'ye atar. 
            var anyEntity = await _service.AnyAsync(x=>x.Id==id); //id'ye sahip entity'nin olup olmadığını kontrol ettik. id ile arama yapabilmek için IAsyncActionFilter'i kalıtım alırken BaseEntity'i göstermelisin.

            if (anyEntity) //Eğer entity varsa controller metodu çalıştırılır
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name} ({id}) not found"));//Entity yoksa CustomResponsoDto modeli NoContentDto olarak dönülür.

        }
    }
}
