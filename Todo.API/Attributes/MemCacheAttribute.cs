using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Security.Cryptography;

namespace Todo.API.Attributes
{
    public class MemCacheAttribute : ActionFilterAttribute
    {
        #region Private Parametros
        private readonly int _timeout_seconts;
        private string _key { get; set; }
        private string _keyMem { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Action Filter para Adicionar serviço de cache de memória da API
        /// </summary>
        /// <param name="timeout_seconts">valor inteiro representando os segundos de tempo de cache</param>
        /// <param name="key">chave e nome do cache</param>
        public MemCacheAttribute(int timeout_seconts, string key)
        {
            this._timeout_seconts = timeout_seconts;
            this._key = key;
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IMemoryCache? _cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            var parametros = context.ActionArguments;

            if (parametros == null)
            {
                context.Result = new BadRequestObjectResult("Os parâmetros da requisição não foram informados.");
                return;
            }

            _keyMem = GenerateHash(string.Join("_", parametros.Values) + "_" + _key);

            if (_cache is not null && _cache.TryGetValue(_keyMem, out var cachedResult))
            {
                context.Result = (IActionResult)cachedResult;
                return;
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            IMemoryCache? _cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

            if (_cache is not null && !_cache.TryGetValue(_keyMem, out var cachedResult))
            {
                var resultado = context.Result;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(_timeout_seconts))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(_timeout_seconts));
                _cache.Set(_keyMem, resultado, cacheEntryOptions);
            }
        }

        #region private Method
        private string GenerateHash(string keyValue)
        {
            byte[] hashBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(keyValue));
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "");
            return hashString;
        }
        #endregion
    }
}
