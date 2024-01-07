using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Todo.API.Validations
{
    public class AllRequiredInputValuesValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var parametros = context.ActionArguments;

            if (parametros == null)
            {
                context.Result = new BadRequestObjectResult("Os parâmetros da requisição não foram informados.");
                return;
            }

            foreach (var argument in parametros)
            {
                if (argument.Value == null)
                {
                    context.Result = new BadRequestObjectResult($"Valor obrigatorio não informado {argument.Key}.");
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
